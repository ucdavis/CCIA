using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{   
     
    public class ClientTagRequestViewModel
    {
        public TagsRequest request { get; set; }  
        public List<AbbrevClassSeeds> possibleClasses { get; set; }  
        public List<AbbrevClassProduced> potatoClasses { get; set; }
        public List<AbbrevTagType> TagTypes { get; set; }   
        public List<Countries> Countries { get; set; }
        public List<AbbrevOECDClass> OECDTagTypes { get; set; }
        public bool AllowOECD { get; set; }
        public bool AllowSeries { get; set; }
        public bool AllowPreTag { get; set; }
        public bool AllowAnalysis { get; set; }

        public ClientTagRequestViewModel()
        {
            AllowSeries = false;
            AllowPreTag = false;
            AllowAnalysis = false;
            AllowOECD = false;
        }
        
               
        public static async Task<ClientTagRequestViewModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id, string tagTarget, int orgId)
        {                   
            var model = new ClientTagRequestViewModel();
            
            
            var request = new TagsRequest();
            request.Id = id;
            request.Target = tagTarget;
            if(tagTarget == "SID")
            {
                var seed = await _helper.FullSeeds().Where(s => s.Id == id).FirstOrDefaultAsync(); 
                var previousTags = await _dbContext.Tags.Where(t => t.SeedsID == id).ToListAsync();
                var previousBlends = await _dbContext.LotBlends.Where(b => b.Sid == id).SumAsync(b => b.Weight);
                var previousBSC = await _dbContext.BulkSalesCertificates.Where(b => b.SeedsID == id).SumAsync(b => b.Pounds);
                model.possibleClasses = await _dbContext.AbbrevClassSeeds.Where(c => c.Program == seed.AppTypeTrans.AppTypeId && c.Id >= seed.Class).OrderBy(c => c.SortOrder).ToListAsync();
                model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync();                                
                request.Crop = seed.GetCropName();
                request.Variety = seed.GetVarietyName();
                request.CertNumber = seed.CertNumber;
                request.LotNumber = seed.LotNumber;
                request.LotWeight = decimal.ToInt32(seed.PoundsLot);
                request.WeightBalance = decimal.ToInt32((previousTags.Any() ? previousTags.Sum(t => t.LotWeightRequested.Value) : 0) + previousBlends + previousBSC);
                request.ClassProduced = seed.ClassProduced.CertClass;
                request.Program = seed.AppTypeTrans.AppTypeTrans;
                request.TagClass = seed.Class.Value;
                request.AllowOECD = true;
                var countries =  await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
                countries.Insert(0, new Countries { Id=0, Name="Select country..."});
                model.Countries = countries;
                model.OECDTagTypes = await _dbContext.AbbrevOECDClass.OrderBy(c => c.SortOrder).ToListAsync();
            }
            if(tagTarget == "BID")
            {
                var blend = await _helper.FullBlendRequest().Where(b => b.Id == id).FirstOrDefaultAsync();
                var previousTags = await _dbContext.Tags.Where(t => t.BlendId == id).ToListAsync();
                model.possibleClasses = await _dbContext.AbbrevClassSeeds.Where(c => c.Id == 4).ToListAsync();
                model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync(); 
                request.Program = "Blends";
                request.Crop = blend.GetCrop();
                request.Variety = blend.GetVarietyName();
                request.CertNumber = blend.CertNumber;
                request.LotNumber = "";
                request.LotWeight = blend.LbsLot.HasValue ? decimal.ToInt32(blend.LbsLot.Value) : 0;
                request.WeightBalance = previousTags.Any() ? decimal.ToInt32(previousTags.Sum(t => t.LotWeightRequested.Value)) : 0;
                request.ClassProduced = "Certified";
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            if(tagTarget == "LT")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                var previousTags = await _dbContext.Tags.Where(t => t.AppId == id).ToListAsync();
                model.possibleClasses = await _dbContext.AbbrevClassSeeds.Where(c => c.Id == 79).ToListAsync();
                model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync(); 
                request.Program = app.AppTypeTrans.AppTypeTrans;
                request.Crop = app.CropName;
                request.Variety = app.VarietyName;
                request.CertNumber = app.FullCert;
                request.LotNumber = "";
                request.LotWeight = app.FieldInspectionReport != null ? app.FieldInspectionReport.PotatoPoundsHarvested : 0;
                request.WeightBalance = previousTags.Any() ? decimal.ToInt32(previousTags.Sum(t => t.LotWeightRequested.Value)) : 0;
                request.ClassProduced = "Certified";
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            if(tagTarget == "PO")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                var previousTags = await _dbContext.Tags.Where(t => t.AppId == id).ToListAsync();
                model.potatoClasses = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == app.AppTypeTrans.AppTypeId && c.ClassProducedId >= app.ClassProducedId).OrderBy(c => c.SortOrder).ToListAsync();
                model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.PotatoTag).OrderBy(t => t.SortOrder).ToListAsync(); 
                request.Program = app.AppTypeTrans.AppTypeTrans;
                request.Crop = app.CropName;
                request.Variety = app.VarietyName;
                request.CertNumber = app.FullCert;
                request.LotNumber = "";
                request.LotWeight = app.FieldInspectionReport != null ? app.FieldInspectionReport.PotatoPoundsHarvested : 0;
                request.WeightBalance = previousTags.Any() ? decimal.ToInt32(previousTags.Sum(t => t.LotWeightRequested.Value)) : 0;
                request.ClassProduced = "Certified";
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            model.request = request;            
            var status = await _dbContext.CondStatus.Where(c => c.OrgId == orgId && c.Year == Helpers.CertYearFinder.ConditionerYear).FirstOrDefaultAsync();
            if(status.AllowPretag)
            {
                model.AllowPreTag = true;
            }
            if(status.PrintSeries)
            { 
                model.AllowSeries = true;
            }
            if(orgId == 37 || orgId ==168)
            {
                model.AllowAnalysis = true;
            }
            return model;
        }
    } 

    public class TagsRequest
    {
        public TagsRequest() 
        {
            BagWeightUnits = "L";
            HowDeliver = "UPS Ground";
            TagType = 1;
            DateNeeded = DateTime.Now.AddDays(2).Date;
            OECD = false;
            OECDTagType = 5;
            Pretagging = false;
            SeriesRequest = false;
            AnalysisRequested = false;
        }


        [Display(Name ="SID/BID/AppID")]
        public int Id { get; set; }
        public string Target { get; set; }
        public string Program { get; set; }
        public string Crop { get; set; }
        public string VarietyLabel { get; set; }
        public string Variety { get; set; }
        [Display(Name ="Cert #")]
        public string CertNumber { get; set; }
        [Display(Name ="Lot #")]
        public string LotNumber { get; set; }
        [Display(Name ="Lot Weight")]
        public int LotWeight { get; set; }
        [Display(Name ="Weight Balance")]
        public int WeightBalance { get; set; }
        [Display(Name ="Class Produced")]
        public string ClassProduced { get; set; }
        public string Alias { get; set; }
        [Display(Name ="Number of Tags")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
        public int NumberOfTags { get; set; }
        [Display(Name ="Bag Weight")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
        public int BagWeight { get; set; }
        public string BagWeightUnits { get; set; }
        [Display(Name ="Coating (%)")]
        public decimal CoatingPercent { get; set; }
        public bool OECD { get; set; }
        public bool AllowOECD { get; set; }
        [Display(Name ="Planting Stock Lot Number")]
        public string PlantingStockLotNumber { get; set; }
        [Display(Name ="OECD Tag")]
        public int OECDTagType { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Sealed")]
        [Required]
        public DateTime? DateSealed { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Please select a destination country.")]
        public int OECDCountryId { get; set; }

        [Display(Name ="Class Produced of Tag")]
        public int TagClass { get; set; }
        [Display(Name ="Type of Tag")]
        public int TagType { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Needed")]
        [Required]
        public DateTime? DateNeeded { get; set; }

        [Display(Name ="How Deliver")]
        public string HowDeliver { get; set; }

        public string Comments { get; set; }
        public bool Pretagging { get; set; }

        [Display(Name = "Series Request")] 
        public bool SeriesRequest { get; set; }

        [Display(Name ="Analysis Requested?")]
        public bool AnalysisRequested { get; set; }


    }   
}
