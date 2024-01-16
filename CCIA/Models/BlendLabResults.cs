﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BlendLabResults
    {
        public int BlendId { get; set; }
        

        [Display(Name = "Germination")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? GermPercent { get; set; }
        public string GermValue 
        { 
            get
            {
                if(GermPercent.HasValue)
             {
                 return (GermPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }        
        }         
       
        public string GermResults { get; set; }
       
        [Display(Name = "Inert")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? InertPercent { get; set; }
        public string InertValue 
        { 
            get
            {
                if(InertPercent.HasValue)
             {
                 return (InertPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        public string InertComments { get; set; }
        [Display(Name = "Other Variety")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? OtherVarietyPercent { get; set; }
        public string OtherVarietyValue 
        { 
            get
            {
                if(OtherVarietyPercent.HasValue)
             {
                 return (OtherVarietyPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        public string OtherVarietyComments { get; set; }
        public int? OtherVarietyCount { get; set; }
        [Display(Name = "Other Kind")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? OtherKindPercent { get; set; }
        public string OtherKindComments { get; set; }
        public string OtherKindValue 
        { 
            get
            {
                if(OtherKindPercent.HasValue)
             {
                 return (OtherKindPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        [Display(Name = "Other Crop")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? OtherCropPercent { get; set; }
        public string OtherCropValue 
        { 
            get
            {
                if(OtherCropPercent.HasValue)
             {
                 return (OtherCropPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        public string OtherCropComments { get; set; }
        public int? OtherCropCount { get; set; }
        [Display(Name = "Purity")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? PurityPercent { get; set; }
        public string PurityValue 
        { 
            get
            {
                if(PurityPercent.HasValue)
             {
                 return (PurityPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }         
        public string PurityComments { get; set; }
        public string PurityResults { get; set; }
        [Display(Name = "Comments")]
        public string Comments { get; set; }
        [Display(Name = "Weed Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? WeedSeedPercent { get; set; }
        public string WeedSeedValue 
        { 
            get
            {
                if(WeedSeedPercent.HasValue)
             {
                 return (WeedSeedPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        public string WeedSeedComments { get; set; }
        [DisplayFormat(DataFormatString = "{0} seed(s)")]
        public int? WeedSeedCount { get; set; }
        [Display(Name = "Noxious Grams")]
        [Range(0.0, 100000)]
        public decimal? NoxiousGrams { get; set; }
        public string NoxiousComments { get; set; }
        [Display(Name = "Noxious Weeds")]        
        [DisplayFormat(DataFormatString = "{0} seed(s)")]
        public int? NoxiousCount { get; set; }
       
        [Display(Name = "Purity Grams")]
        [Range(0.0, 100000)]
        public decimal? PurityGrams { get; set; }
        
        [Display(Name = "Hard Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        [Range(0.0, 100)]
        public decimal? HardSeedPercent { get; set; }
        public string GermHardSeedValue 
        { 
            get
            {
                if(HardSeedPercent.HasValue)
             {
                 return (HardSeedPercent.Value * 100).ToString("0.####");
             } 
             return "";
            }           
        }  
        public string AssayResults { get; set; }
        public bool? AssayTest { get; set; }
        public bool? AssayTest2 { get; set; }
        public string AssayResults2 { get; set; }
        [Display(Name = "Dodder Grams")]
        [Range(0.0, 100000)]
        public decimal? DodderGrams { get; set; }
        
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
        [Display(Name ="CCIA Confirmed")]
        public bool CciaConfirmed { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string ConfirmUser { get; set; }
        

        [ForeignKey("SID")]
        public ICollection<SampleLabResultsChanges> Changes { get; set; }

        

        [Display(Name = "Total Germination")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal TotalGermination { 
            get
            {
                if(HardSeedPercent.HasValue && GermPercent.HasValue)
                {
                    return HardSeedPercent.Value + GermPercent.Value;
                }
                if(GermPercent.HasValue)
                {
                    return GermPercent.Value;
                }
                if(HardSeedPercent.HasValue)
                {
                    return HardSeedPercent.Value;
                }
                return 0;
            } 
        }

        public BlendRequests Blend { get; set; }


    }
}