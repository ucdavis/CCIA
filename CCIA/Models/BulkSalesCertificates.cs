using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BulkSalesCertificates
    {
        public BulkSalesCertificates() {
            Date = DateTime.Now.Date;
            PurchaserStateId = 102;
        }
        public int Id { get; set; }
        public int ConditionerOrganizationId { get; set; }

        [ForeignKey("ConditionerOrganizationId")]
        public Organizations ConditionerOrganization { get; set; }

        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString = "{0:d}")]
        
        [Display(Name="Date Sold")]
        public DateTime Date { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }

        public string CertProgramAbbreviation { get; set; }
        [ForeignKey("CertProgramAbbreviation")]
        public AbbrevAppType CertProgram { get; set; }

        
        [Display(Name="Purchaser Name")]
        public string PurchaserName { get; set; }

        
        [Display(Name="Address Line1")]
        public string PurchaserAddressLine1 { get; set; }

        
        [Display(Name="Address Line2")]
        public string PurchaserAddressLine2 { get; set; }

        [Display(Name="Address Lines")]
        public string PurchaserAddressLines
        {
            get
            {
               return !string.IsNullOrWhiteSpace(PurchaserAddressLine2) ? $"{PurchaserAddressLine1}<br>{PurchaserAddressLine2}" : PurchaserAddressLine1;
            }
        }


        
        [Display(Name="City")]
        public string PurchaserCity { get; set; }

        
        [Display(Name="State")]
        public int? PurchaserStateId { get; set; }

        [ForeignKey("PurchaserStateId")]
        public StateProvince PurchaserState { get; set; }

        
        [Display(Name="County")]
        public int? PurchaserCountryId { get; set; }

        public Countries PurchaserCountry { get; set; }

         
         [Display(Name="Zip")]
        public string PurchaserZip { get; set; }

         
         [Display(Name="Phone")]
        public string PurchaserPhone { get; set; }

         
         [Display(Name="Email")]
        public string PurchaserEmail { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00}")]
        public int Pounds { get; set; }

        
        [Display(Name="Class Sold")]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevClassSeeds Class { get; set; }

        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public Contacts CreatedByContact { get; set; }

        
        [Display(Name="Date Created")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime CreatedOn { get; set; }

        public string AdminUpdatedId { get; set; }

        [ForeignKey("AdminUpdatedId")]
        public CCIAEmployees AdminEmployee { get; set; }

        public DateTime? AdminUpdatedDate { get; set; }

        public bool NotificationSent { get; set; }

         public int? LinkId
        {
            get
            {
                if (SeedsID.HasValue)
                {
                    return SeedsID;
                }
                else if (BlendId.HasValue)
                {
                    return BlendId;
                }               
                else
                {
                    return null;
                }
            }
        }

        
        public string IdType
        {
            get
            {
                if (SeedsID.HasValue)
                {
                    return "SID";
                }
                if (BlendId.HasValue)
                {
                    return "BID";
                }
                return "";
            }
        }       

        [Display(Name ="Crop")]
        public string CropName
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null && Seeds.Variety != null && Seeds.Variety.Crop != null)
                {
                    return Seeds.Variety.Crop.Name;
                }
                if (BlendId.HasValue && Blend != null)
                {
                    return Blend.GetCrop();
                }                
                return "Unknown";
            }
        }

        [Display(Name ="Variety")]
        public string VarietyName
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null && Seeds.Variety != null)
                {
                    return Seeds.Variety.Name;
                }
                if (BlendId.HasValue && Blend != null)
                {
                    return Blend.GetVarietyName();
                }               
                return "Unknown";
            }
        }

        [Display(Name ="Cert #")]
        public string CertNumber
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.CertNumber;
                }
                if (BlendId.HasValue && Blend != null)
                {
                    return Blend.CertNumber;
                }                
                return "Unknown";
            }
        }

        public string LotNumber 
        { 
            get
            {
                if(SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.LotNumber;
                }

                return "";
            }
        }

        public string Program 
        { 
            get
            {
                if (SeedsID.HasValue && Seeds != null && Seeds.AppTypeTrans != null)
                {
                    return Seeds.AppTypeTrans.AppTypeTrans;
                }
                if (BlendId.HasValue)
                {
                    return "Blend";
                }
                return "";
            }
        }

        public string CertResults
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null && Seeds.LabResults != null)
                {
                    return Seeds.CertResults();
                }
                if(BlendId.HasValue)
                {
                    return "Passed";
                }
                return "ReJECTED";
            }
        }

        public ICollection<BulkSalesCertificatesShares> BulkSalesCertificatesShares { get; set; }

        [ForeignKey("BSCId")]
        public ICollection<BulkSalesCertificateChanges> Changes {get; set;}

              


    }
}
