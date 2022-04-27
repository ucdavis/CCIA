using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models.SeedsCreateOOSViewModel;
using CCIA.Models.SeedsCreateViewModel;
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
        public Crops crop { get; set; }
        public VarFull variety { get; set; }

        public bool AllowSeries { get; set; }
        public bool AllowPreTag { get; set; }
        public bool AllowAnalysis { get; set; }
        public Applications Application { get; set; }
        public NewSeeds Seed { get; set; }
        public List<StateProvince> States { get; set; }
        public NewOOSSeeds OOSSeed { get; set; }
        public List<int> CertYears { get; set; }
        public List<Crops> crops { get; set; }

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
                if(seed == null)
                {
                    return model;
                }
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
                if(blend == null)
                {
                    return model;
                }
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
                if(app == null)
                {
                    return model;
                }
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
                if(app == null)
                {
                    return model;
                }
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
        public static async Task<ClientTagRequestViewModel> CreateBulk(CCIAContext _dbContext, IFullCallService _helper , int cropId, string variety, int orgId)
        {                   
            var model = new ClientTagRequestViewModel();
            model.crop = await _dbContext.Crops.Where(c => c.CropId == cropId).FirstOrDefaultAsync();
            model.variety = await _dbContext.VarFull.Where(v => v.CropId == cropId && EF.Functions.Like(v.Name, "%" +  variety + "%")).FirstOrDefaultAsync();            
            model.request = new TagsRequest(); 
            model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync();                      
            return model;
        }

        public static async Task<ClientTagRequestViewModel> CreateBulkRetry(CCIAContext _dbContext, IFullCallService _helper , int cropId, string variety, int orgId, TagsRequest submittedTag)
        {                   
            var model = new ClientTagRequestViewModel();
            model.crop = await _dbContext.Crops.Where(c => c.CropId == cropId).FirstOrDefaultAsync();
            model.variety = await _dbContext.VarFull.Where(v => v.CropId == cropId && EF.Functions.Like(v.Name, "%" +  variety + "%")).FirstOrDefaultAsync();            
            model.request =submittedTag; 
            model.TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync();                      
            return model;
        }

        public static async Task<ClientTagRequestViewModel> Edit(CCIAContext _dbContext, IFullCallService _helper , int id, string tagTarget, int orgId, TagsRequest submittedTag)
        {                   
            var model = new ClientTagRequestViewModel();
            
            
            var request = new TagsRequest();
            request.Id = id;
            request.Target = tagTarget;
            if(tagTarget == "SID")
            {
                var seed = await _helper.FullSeeds().Where(s => s.Id == id).FirstOrDefaultAsync(); 
                if(seed == null)
                {
                    return model;
                }
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
                if(blend == null)
                {
                    return model;
                }
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
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            if(tagTarget == "LT")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                if(app == null)
                {
                    return model;
                }
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
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            if(tagTarget == "PO")
            {
                var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
                if(app == null)
                {
                    return model;
                }
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
                request.TagClass = 4;
                request.AllowOECD = false;
            }
            request.Alias = submittedTag.Alias;
            request.CountRequested = submittedTag.CountRequested;
            request.BagSize = submittedTag.BagSize;
            request.WeightUnit = submittedTag.WeightUnit;
            request.CoatingPercent = submittedTag.CoatingPercent;
            request.TagClass = submittedTag.TagClass;
            request.TagType = submittedTag.TagType;
            request.Pretagging = submittedTag.Pretagging;
            request.SeriesRequest = submittedTag.SeriesRequest;
            request.AnalysisRequested = submittedTag.AnalysisRequested;
            request.DateNeeded = submittedTag.DateNeeded;
            request.HowDeliver = submittedTag.HowDeliver;
            request.Comments = submittedTag.Comments;
            request.OECD = submittedTag.OECD;
            request.PlantingStockLotNumber = submittedTag.PlantingStockLotNumber;
            request.OECDTagType = submittedTag.OECDTagType;
            request.DateSealed = submittedTag.DateSealed;
            request.OECDCountryId = submittedTag.OECDCountryId;

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

        public static async Task<ClientTagRequestViewModel> CreateOOSGrayTag (CCIAContext _dbContext)
        {            
            var countyId = await _dbContext.Organizations.Where(o => o.Id == 168).Select(o => o.CountyId).FirstAsync();            
            var seed = new NewOOSSeeds();
            seed.CountyDrawn = countyId; 
            seed.CertYear = CertYearFinder.CertYear;            

            var viewModel = new ClientTagRequestViewModel
            {
                States =  await _dbContext.StateProvince.Where(s => s.Name != "California").Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),               
                OOSSeed = seed,
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true).Select(c => new Crops { CropId = c.CropId, Crop = c.Crop, CropKind = c.CropKind}).ToListAsync(),                
                Countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                CertYears =  CertYearFinder.certYearListReverse.ToList(),
                request = new TagsRequest(),
            };
            return viewModel;
        }

        public static async Task<ClientTagRequestViewModel> CreateOOSGrayTagRetry (CCIAContext _dbContext, ClientTagRequestViewModel model)
        {               
            var viewModel = new ClientTagRequestViewModel
            {
                States =  await _dbContext.StateProvince.Where(s => s.Name != "California").Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),               
                OOSSeed = model.OOSSeed,
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true).Select(c => new Crops { CropId = c.CropId, Crop = c.Crop, CropKind = c.CropKind}).ToListAsync(),                
                Countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                CertYears =  CertYearFinder.certYearListReverse.ToList(),
                request = model.request,
            };

            return viewModel;

        }

        public static async Task<ClientTagRequestViewModel> CreateGrayTag(CCIAContext _dbContext, int[] appId, int certYear, int certNum, int certRad)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == appId.First())
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.ClassProduced)
                .FirstAsync();          
           
            var seed = new NewSeeds();           
            seed.AppId = appId;
            seed.CertYear = certYear;
            seed.SampleFormCertNumber = certNum;
            if(certRad == 0)
            {
                seed.SampleFormRad = null;
            } else
            {
                seed.SampleFormRad = certRad;
            }      
            var countries =  await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
            countries.Insert(0, new Countries { Id=0, Name="Select country..."});

            var viewModel = new ClientTagRequestViewModel
            {
                Application = app,
                Seed = seed,
                Countries = countries,
                TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync(),
                request = new TagsRequest(),
            };

            return viewModel;
        }

        public static async Task<ClientTagRequestViewModel> CreateGrayTagRetry(CCIAContext _dbContext, ClientTagRequestViewModel model )
        {      
            var app = await _dbContext.Applications.Where(a => a.Id == model.Seed.AppId.First())
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.ClassProduced)
                .FirstAsync();                       
           
            var countries =  await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();
            countries.Insert(0, new Countries { Id=0, Name="Select country..."});

            var viewModel = new ClientTagRequestViewModel
            {
                Application = app,
                Seed = model.Seed,
                Countries = countries,
                TagTypes = await _dbContext.AbbrevTagType.Where(t => t.StandardTagForm).OrderBy(t => t.SortOrder).ToListAsync(),
                request = model.request,
            };

            return viewModel;
        }
    } 

    public class TagsRequest
    {
        public TagsRequest() 
        {
            WeightUnit = "L";
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
        public int CountRequested { get; set; }
        [Display(Name ="Bag Weight")]
        [Range(1, int.MaxValue, ErrorMessage = "Please enter a value greater than 0")]
        public int BagSize { get; set; }
        public string WeightUnit { get; set; }
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
