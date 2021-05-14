using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedTransfers
    {
        public int Id { get; set; }

        public string Type { get; set; }
        public int OriginatingOrganizationId { get; set; }

        [ForeignKey("OriginatingOrganizationId")]
        public Organizations OriginatingOrganization { get; set; }

        public int? OriginatingCountyId { get; set; }

        [ForeignKey("OriginatingCountyId")]
        public County OriginatingCounty { get; set; }
        
        [DisplayName("Certificate Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime CertificateDate { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }

        public int? ApplicationId { get; set; }
        [ForeignKey("ApplicationId")]
        public Applications Application { get; set; }

        public int CreatedById { get; set; }

        [ForeignKey("CreatedById")]
        public Contacts CreatedByContact { get; set; }

        public DateTime? CreatedOn { get; set; }

        public Decimal Pounds { get; set; }
        
         public int? ClassId { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevClassSeeds SeedClass { get; set; }

        [ForeignKey("ClassId")]
        public AbbrevClassProduced AppClass { get; set; }

        public string SeedstockLotNumbers { get; set; }

        public bool SubmittedForAnalysis { get; set; }

        public int? DestinationOrganizationId { get; set; }

        public Organizations DestinationOrganization { get; set; }       
        
        
        public string PurchaserName { get; set; }

        public string PurchaserAddressLine1 { get; set; }

        public string PurchaserAddressLine2 { get; set; }

        public string PurchaserCity { get; set; }

        public int? PurchaserStateId { get; set; }

        [ForeignKey("PurchaserStateId")]
        public StateProvince PurchaserState { get; set; }

        public int? PurchaserCountyId { get; set; }

        [ForeignKey("PurchaserCountyId")]
        public County PurchaserCounty { get; set; }

        public int? PurchaserCountryId { get; set; }

        public Countries PurchaserCountry { get; set; }

        public string PurchaserZip { get; set; }

        public string PurchaserPhone { get; set; }

        public string PurchaserEmail { get; set; }

        public bool StageInDirt { get; set; }

        public bool StageFromField { get; set; }

        public decimal? StageFromFieldNumberOfAcres  { get; set; }

        public bool StageFromStorage { get; set; }

        public bool StageConditioned { get; set; }

        public bool StageNotFinallyCertified { get; set; }

        public bool StageCertifiedSeed { get; set; }

        public bool StageTreatment { get; set; }

        public bool StageBagging { get; set; }

        public bool StageTagging { get; set; }

        public bool StageBlending { get; set; }

        public bool StageStorage { get; set; }

        public bool StageOther { get; set; }

        public string StageOtherValue { get; set; }

        public bool TypeRetail { get; set; }

        public bool TypeTote { get; set; }

        public bool TypeBulk { get; set; }

        public int? NumberOfTrucks { get; set; }

        public bool AgricultureCommissionerAccurate { get; set; }

        public bool AgricultureCommissionerInaccurate { get; set; }

        public bool AgricultureCommissionerApprove { get; set; }

        public DateTime? AgricultureCommissionerDateRespond { get; set; }

        public int? AgricultureCommissionerContactRespondId { get; set; }

        [ForeignKey("AgricultureCommissionerContactRespondId")]
        public Contacts AgricultureCommissionerContactRespond { get; set; }
        
        public string AdminUpdatedId { get; set; }

        [ForeignKey("AdminUpdatedId")]
        public CCIAEmployees AdminEmployee { get; set; }

        public DateTime? AdminUpdatedDate { get; set; }

        public bool AdminUpdated { get; set; }

       
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
                } if (ApplicationId.HasValue) 
                {
                    return ApplicationId;
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
                if(ApplicationId.HasValue)
                {
                    return "AppID";
                }
                return "";
            }
        }

        public string TransferClass
        {
            get
            {
                if (SeedsID.HasValue && Seeds.ClassProduced != null)
                {
                    return Seeds.ClassProduced.CertClass;
                }
                if (BlendId.HasValue)
                {
                    return "Certified Blend";
                }
                if(ApplicationId.HasValue && Application.ClassProduced != null)
                {
                    return Application.ClassProduced.ClassProducedTrans;
                }
                return "Unknown";
            }
        }

        [ForeignKey("STId")]
        public ICollection<SeedTransferChanges> Changes {get; set;}        

       

    }
}
