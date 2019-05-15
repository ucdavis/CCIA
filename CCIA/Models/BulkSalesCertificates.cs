using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BulkSalesCertificates
    {
        public int Id { get; set; }
        public int ConditionerOrganizationId { get; set; }

        [ForeignKey("ConditionerOrganizationId")]
        public Organizations ConditionerOrganization { get; set; }
        public DateTime Date { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }

        public string CertProgram { get; set; }

        public string PurchaserName { get; set; }

        public string PurchaserAddressLine1 { get; set; }

        public string PurchaserAddressLine2 { get; set; }

        public string PurchaserCity { get; set; }

        public int? PurchaserStateId { get; set; }

        [ForeignKey("PurchaserStateId")]
        public StateProvince PurchaserState { get; set; }

        public int? PurchaserCountryId { get; set; }

        public Countries PurchaserCountry { get; set; }

        public string PurchaserZip { get; set; }

        public string PurchaserPhone { get; set; }

        public string PurchaserEmail { get; set; }

        public Decimal Pounds { get; set; }

        public int? ClassId { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevClassSeeds Class { get; set; }

        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public Contacts CreatedByContact { get; set; }

        public DateTime CreatedOn { get; set; }

        public string AdminUpdatedId { get; set; }

        [ForeignKey("AdminUpdatedId")]
        public CCIAEmployees AdminEmployee { get; set; }

        public DateTime? AdminUpdatedDate { get; set; }

        public bool NotificationSent { get; set; }

        
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
                    return Blend.Crop;
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
                    return Blend.VarietyName;
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

              


    }
}
