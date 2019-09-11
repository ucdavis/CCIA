using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SampleLabResults
    {
        public int SeedsId { get; set; }
        public DateTime? DateComplete { get; set; }

        [Display(Name = "Germination")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? GermPercent { get; set; }
        public decimal GermValue 
        { 
            get
            {
                if(GermPercent.HasValue)
             {
                 return GermPercent.Value * 100;
             } 
             return 0;
            }           
        }         
        public int? GermDays { get; set; }
        public string GermResults { get; set; }
        public string GermTemp { get; set; }
        [Display(Name = "Inert")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? InertPercent { get; set; }
        public decimal InertValue 
        { 
            get
            {
                if(InertPercent.HasValue)
             {
                 return InertPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string InertComments { get; set; }
        [Display(Name = "Other Variety")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? OtherVarietyPercent { get; set; }
        public decimal OtherVarietyValue 
        { 
            get
            {
                if(OtherVarietyPercent.HasValue)
             {
                 return OtherVarietyPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string OtherVarietyComments { get; set; }
        public int? OtherVarietyCount { get; set; }
        [Display(Name = "Other Crop")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? OtherCropPercent { get; set; }
        public decimal OtherCropValue 
        { 
            get
            {
                if(OtherCropPercent.HasValue)
             {
                 return OtherCropPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string OtherCropComments { get; set; }
        public int? OtherCropCount { get; set; }
        [Display(Name = "Purity")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? PurityPercent { get; set; }
        public decimal PurityValue 
        { 
            get
            {
                if(PurityPercent.HasValue)
             {
                 return PurityPercent.Value * 100;
             } 
             return 0;
            }           
        }         
        public string PurityComments { get; set; }
        public string PurityResults { get; set; }
        [Display(Name = "Comments")]
        public string Comments { get; set; }
        [Display(Name = "Weed Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? WeedSeedPercent { get; set; }
        public decimal WeedSeedValue 
        { 
            get
            {
                if(WeedSeedPercent.HasValue)
             {
                 return WeedSeedPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string WeedSeedComments { get; set; }
        [DisplayFormat(DataFormatString = "{0} seed(s)")]
        public int? WeedSeedCount { get; set; }
        [Display(Name = "Noxious Grams")]
        public decimal? NoxiousGrams { get; set; }
        public string NoxiousComments { get; set; }
        [Display(Name = "Noxious Weeds")]        
        [DisplayFormat(DataFormatString = "{0} seed(s)")]
        public int? NoxiousCount { get; set; }
        [Display(Name = "Bushel Weight")]
        public decimal? BushelWeight { get; set; }
        [Display(Name = "Purity Grams")]
        public decimal? PurityGrams { get; set; }
        public bool? CciaGerm { get; set; }
        [Display(Name = "Hard Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? GermHardSeed { get; set; }
        public decimal GermHardSeedValue 
        { 
            get
            {
                if(WeedSeedPercent.HasValue)
             {
                 return WeedSeedPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string AssayResults { get; set; }
        public bool? AssayTest { get; set; }
        public bool? AssayTest2 { get; set; }
        public string AssayResults2 { get; set; }
        [Display(Name = "Dodder Grams")]
        public decimal? DodderGrams { get; set; }
        public decimal? LbsCanceled { get; set; }
        public decimal? LbsPassed { get; set; }
        public decimal? LbsRejected { get; set; }
        public DateTime? DataEntryDate { get; set; }
        public string DataEntryUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        [Display(Name = "Lab Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? PrivateLabDate { get; set; }
        [Display(Name = "Lab Name")]
        public string PrivateLabName { get; set; }

        [ForeignKey("PrivateLabId")]
        public Organizations LabOrganization { get; set; }
        public int? PrivateLabId { get; set; }
        public string PrivateLabNumber { get; set; }
        public bool? CciaConfirmed { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string ConfirmUser { get; set; }
        [Display(Name = "Badly Discolored")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? BadlyDiscoloredPercent { get; set; }
        public decimal BadlyDiscoloredValue 
        { 
            get
            {
                if(BadlyDiscoloredPercent.HasValue)
             {
                 return BadlyDiscoloredPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        [Display(Name = "Foreign Material")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? ForeignMaterialPercent { get; set; }
        public decimal ForeignMaterialValue 
        { 
            get
            {
                if(ForeignMaterialPercent.HasValue)
             {
                 return ForeignMaterialPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        public string ForeignMaterialsComments { get; set; }
        [Display(Name = "Splits&Cracks")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? SplitsAndCracksPercent { get; set; }
        public decimal SplitsAndCracksValue 
        { 
            get
            {
                if(SplitsAndCracksPercent.HasValue)
             {
                 return SplitsAndCracksPercent.Value * 100;
             } 
             return 0;
            }           
        }  
        [Display(Name = "Chewing Insect Damage")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? ChewingInsectDamagePercent { get; set; }
        public decimal ChewingInsectDamageValue 
        { 
            get
            {
                if(ChewingInsectDamagePercent.HasValue)
             {
                 return ChewingInsectDamagePercent.Value * 100;
             } 
             return 0;
            }           
        } 

        [Display(Name = "Total Germination")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal TotalGermination { 
            get
            {
                if(GermHardSeed.HasValue && GermPercent.HasValue)
                {
                    return GermHardSeed.Value + GermPercent.Value;
                }
                if(GermPercent.HasValue)
                {
                    return GermPercent.Value;
                }
                if(GermHardSeed.HasValue)
                {
                    return GermHardSeed.Value;
                }
                return 0;
            } 
        }


    }
}
