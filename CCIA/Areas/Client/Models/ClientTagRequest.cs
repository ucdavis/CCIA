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
        
               
        public static async Task<ClientTagRequestViewModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id, string tagTarget)
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
                request.Program = seed.CertProgram;                
                request.Crop = seed.GetCropName();
                request.Variety = seed.GetVarietyName();
                request.CertNumber = seed.CertNumber;
                request.LotNumber = seed.LotNumber;
                request.LotWeight = decimal.ToInt32(seed.PoundsLot);
                request.WeightBalance = decimal.ToInt32(previousTags.Sum(t => t.LotWeightRequested.Value) + previousBlends + previousBSC);
                request.ClassProduced = seed.ClassProduced.CertClass;
                request.Program = seed.AppTypeTrans.AppTypeTrans;
            }
            model.request = request;
            return model;
        }
    } 

    public class TagsRequest
    {
        public TagsRequest() 
        {
            BagWeightUnits = "L";
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
        public int NumberOfTags { get; set; }
        [Display(Name ="Bag Weight")]
        public int BagWeight { get; set; }
        public string BagWeightUnits { get; set; }
        [Display(Name ="Coating (%)")]
        public decimal CoatingPercent { get; set; }
        public bool OECD { get; set; }
        public bool AllowOECD { get; set; }
        public string PlantingStockLotNumber { get; set; }
        public int OECDTagType { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Sealed")]
        public DateTime? DateSealed { get; set; }

        public int OECDCountryId { get; set; }

        [Display(Name ="Class Produced of Tag")]
        public int TagClass { get; set; }
        [Display(Name ="Type of Tag")]
        public int TagType { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Needed")]
        public DateTime? DateNeeded { get; set; }

        [Display(Name ="How Deliver")]
        public string HowDeliver { get; set; }

        public string Comments { get; set; }


    }   
}
