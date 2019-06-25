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
        }
        public int Id { get; set; }
        public int ConditionerOrganizationId { get; set; }

        [ForeignKey("ConditionerOrganizationId")]
        public Organizations ConditionerOrganization { get; set; }

        [DisplayFormat(ApplyFormatInEditMode=true, DataFormatString = "{0:d}")]
        [DisplayName("Date Sold")]
        public DateTime Date { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }

        public string CertProgram { get; set; }

        [DisplayName("Purchaser Name")]
        public string PurchaserName { get; set; }

        [DisplayName("Address Line1")]
        public string PurchaserAddressLine1 { get; set; }

        [DisplayName("Address Line2")]
        public string PurchaserAddressLine2 { get; set; }

        [DisplayName("City")]
        public string PurchaserCity { get; set; }

        [DisplayName("State")]
        public int? PurchaserStateId { get; set; }

        [ForeignKey("PurchaserStateId")]
        public StateProvince PurchaserState { get; set; }

        [DisplayName("Country")]
        public int? PurchaserCountryId { get; set; }

        public Countries PurchaserCountry { get; set; }

         [DisplayName("Zip")]
        public string PurchaserZip { get; set; }

         [DisplayName("Phone")]
        public string PurchaserPhone { get; set; }

         [DisplayName("Email")]
        public string PurchaserEmail { get; set; }

        public int Pounds { get; set; }

        [DisplayName("Class Sold")]
        public int ClassId { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevClassSeeds Class { get; set; }

        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public Contacts CreatedByContact { get; set; }

        [DisplayName("Date Created")]
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

        public string CropName
        {
            get
            {
                if (SeedsID.HasValue && Seeds.Variety.Crop != null)
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

        public string VarietyName
        {
            get
            {
                if (SeedsID.HasValue && Seeds.Variety != null)
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

        public string CertNumber
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.CertNumber();
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
                if (SeedsID.HasValue)
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

        public ICollection<BulkSalesCertificatesShares> BulkSalesCertificatesShares { get; set; }

              


    }
}
