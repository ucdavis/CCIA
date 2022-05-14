using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public enum SeedTransferTypes
    { 
        [Display(Name="Intra-county")]
        IntraCounty,
        [Display(Name="Inter-county")]
        InterCounty,
        [Display(Name="Inter-agency (state)")]
        InterAgency,        
    } 

    public partial class SeedTransfers
    {
        public SeedTransfers() {
            AgricultureCommissionerAccurate = false;
            AgricultureCommissionerApprove = false;
            AgricultureCommissionerInaccurate = false;
            AdminUpdated = false;
        }

        [Display(Name="STId")]
        public int Id { get; set; }

        public string Type { get; set; }
        public int OriginatingOrganizationId { get; set; }

        [ForeignKey("OriginatingOrganizationId")]
        public Organizations OriginatingOrganization { get; set; }

        [Display(Name="County")]
        public int? OriginatingCountyId { get; set; }

        [ForeignKey("OriginatingCountyId")]
        public County OriginatingCounty { get; set; }
                
        [Display(Name="Certificate Date")]
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

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
        public Decimal Pounds { get; set; }
        
         public int? TransferClassId { get; set; }

        [ForeignKey("TransferClassId")]
        public AbbrevClassSeeds SeedClass { get; set; }

        [ForeignKey("TransferClassId")]
        public AbbrevClassProduced AppClass { get; set; }

        [Display(Name="Planting Stock Lot No.")]
        public string SeedstockLotNumbers { get; set; }

        public bool SubmittedForAnalysis { get; set; }

        public int? DestinationOrganizationId { get; set; }

        public Organizations DestinationOrganization { get; set; }       
        
        
        [Display(Name="Name")]
        public string PurchaserName { get; set; }

        public string PurchaserAddressLine1 { get; set; }

        public string PurchaserAddressLine2 { get; set; }

        [Display(Name="City")]
        public string PurchaserCity { get; set; }

        [Display(Name="State")]
        public int? PurchaserStateId { get; set; }

        [ForeignKey("PurchaserStateId")]
        public StateProvince PurchaserState { get; set; }

        [Display(Name="County")]
        public int? PurchaserCountyId { get; set; }

        [ForeignKey("PurchaserCountyId")]
        public County PurchaserCounty { get; set; }

        [Display(Name="Country")]
        public int? PurchaserCountryId { get; set; }

        public Countries PurchaserCountry { get; set; }

        [Display(Name="Zip")]
        public string PurchaserZip { get; set; }

        [Display(Name="Phone")]
        public string PurchaserPhone { get; set; }

        [Display(Name="Email")]
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

        public string GetVariety()
        {
            if (BlendId.HasValue && Blend != null)
            {
                return Blend.GetVarietyName();
            }
            if (SeedsID.HasValue && Seeds != null)
            {
                return Seeds.GetVarietyName();
            }
            if (ApplicationId.HasValue && Application != null)
            {
                return Application.Variety.Name;
            }            
            return "unknown";
        }

        public string GetCrop()
        {
            if (BlendId.HasValue && Blend != null)
            {
                return Blend.GetCrop();
            }
            if (SeedsID.HasValue && Seeds != null)
            {
                return Seeds.GetCropName();
            }
            if (ApplicationId.HasValue && Application != null)
            {
                return Application.Variety.Crop.Name;
            }            
            return "unknown";
        }

        public string GetClass()
        {
            if (BlendId.HasValue)
            {
                return "Blend";
            }
            if (SeedsID.HasValue && Seeds != null && Seeds.ClassProduced != null)
            {
                return Seeds.ClassProduced.AppType != null ? Seeds.ClassProduced.NameAndAppType : Seeds.ClassProduced.CertClass;
            }
            if (ApplicationId.HasValue && Application != null && Application.ClassProduced != null)
            {
                return Application.ClassProduced.AppType != null ? Application.ClassProduced.NameAndAppType : Application.ClassProduced.ClassProducedTrans;
            }            
            return "unknown";
        }

        public int GetYearOfProduction()
        {
            if (BlendId.HasValue && Blend != null)
            {
                return Blend.RequestStarted.Year;
            }
            if (SeedsID.HasValue && Seeds != null)
            {
                return Seeds.CertYear.Value;
            }
            if (ApplicationId.HasValue && Application != null)
            {
                return Application.CertYear;
            }            
            return 1900;
        }

        public string ClassTransferred()
        {
            if(BlendId.HasValue)
            {
                return "Blend";
            }
            if(TransferClassId.HasValue && SeedClass != null )
            {
                return SeedClass.CertClass;

            }
            if(TransferClassId.HasValue && AppClass != null)
            {
                return AppClass.ClassProducedTrans;
            }
            return "";
        }

        public string GetCertNumber()
        {
            if(BlendId.HasValue && Blend != null)
            {
                return Blend.CertNumber;
            }
           if(SeedsID.HasValue && Seeds != null)
           {
               return Seeds.CertNumber;
           }
           if(ApplicationId.HasValue && Application != null)
           {
               return Application.FullCert;
           }
            return "";
        }


        public string GetStateOfOrigin()
        {
            if(ApplicationId.HasValue || BlendId.HasValue)
            {
                return "California";
            }
            if(SeedsID.HasValue && Seeds != null && Seeds.StateOfOrigin != null)
            {
                return Seeds.StateOfOrigin.Name;
            }
            return "Unknown";
        }

        [Display(Name="Address Lines")]
        public string PurchaserAddressLines
        {
            get
            {
               return !string.IsNullOrWhiteSpace(PurchaserAddressLine2) ? $"{PurchaserAddressLine1}<br>{PurchaserAddressLine2}" : PurchaserAddressLine1;
            }
        }

        

        [Display(Name="Item")]
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
                if (SeedsID.HasValue && Seeds != null && Seeds.ClassProduced != null)
                {
                    return Seeds.ClassProduced.CertClass;
                }
                if (BlendId.HasValue)
                {
                    return "Certified Blend";
                }
                if(ApplicationId.HasValue && Application != null && Application.ClassProduced != null)
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
