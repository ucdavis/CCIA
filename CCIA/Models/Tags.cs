using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCIA.Helpers;

namespace CCIA.Models
{
    public enum TagStages
    { 
        [Display(Name="Requested")]
        Requested,
        [Display(Name="Printing")]
        Printing,
        [Display(Name="Pending file")]
        PendingFile,
        [Display(Name="Complete")]
        Complete,
        [Display(Name="Follow-Up")]
        followup,
        [Display(Name="Cancelled")]
        Cancelled,
        [Display(Name ="Returned to Client")]
        ReturnedToClient
    } 

    public enum TagHowPickUp
    { 
        [Display(Name="UPS Ground")]
        UPSGround,
        [Display(Name="UPS Overnight")]
        UPSOvernight,
        [Display(Name="Pick-up at Parsons Building")]
        PickUp,
        [Display(Name="Other (see comments)")]
        Other,
    } 
    public partial class Tags
    {

        public Tags()
        {
            Bulk = false;
            Pretagging = false;
            //SeriesNumbered = false;
            AnalysisRequested = false;
            OECD = false;
            SeriesRequest = false;
            ExtrasOverrun = 0;
        }
        [Display(Name = "TagID")]
        public int Id { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }
        public int? AppId { get; set; }
        [ForeignKey("AppId")]
        public Applications Application {get; set;}

        public bool FollowUp { get; set; }
        [Display(Name ="Return Reason")]
        public string ReturnReason { get; set; }

        
        public TagBagging TagBagging { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagSeries> TagSeries { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagChanges> Changes { get; set; }
        
        [Display(Name = "ID Type")] 
        public string IdType
        {
            get
            {
                if (SeedsID.HasValue)
                {
                    return "SID";
                }
                else if (BlendId.HasValue)
                {
                    return "BID";
                }
                else if (AppId.HasValue)
                {
                    return "AppID";
                }
                else if (Bulk)
                {
                    return "Bulk";
                }
                else
                {
                    return "";
                }
            }
        }

        [Display(Name = "Cert Year")] 
        public int CertYear
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.CertYear.Value;
                }
                if(BlendId.HasValue && Blend != null)
                {
                    return Blend.CertYear;
                }
                if(AppId.HasValue && Application != null)
                {
                    return Application.CertYear;
                }
                else 
                {
                    return 0;
                }
            }
        }

        public int ConditionerYearEntered 
        { get
            {
                if(!DateEntered.HasValue)
                {
                    return 0;
                }
                if(DateEntered.Value.Month >= 1 && DateEntered.Value.Month <= 6)
                    {
                        return DateEntered.Value.Year - 1;
                    } else
                    {
                        return DateEntered.Value.Year;
                    }                
            }        
        }

        [Display(Name = "Tagged Class")] 
        public string TaggedClass
        {
            get
            {
                if(TagAbbrevClass != null)
                {
                    return TagAbbrevClass.CertClass;
                }
                return "";
            }
        }

        [Display(Name = "Crop")] 
        public string CropName
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.GetCropName();
                }
                if (BlendId.HasValue && Blend != null)
                {
                    return Blend.GetCrop();
                }
                if (BulkCrop != null)
                {
                    return BulkCrop.Name;
                }
                if(AppId.HasValue && Application != null)
                {
                    return Application.CropName;
                }
                return "Unknown";
            }
        }

        [Display(Name = "Variety")] 
        public string VarietyName
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.GetVarietyName();
                }
                if (BlendId.HasValue && Blend != null)
                {
                    return Blend.GetVarietyName();
                }
                if (BulkVariety != null)
                {
                    return BulkVariety.Name;
                }
                if(AppId.HasValue && Application != null)
                {
                    return Application.VarietyName;
                }
                return "Unknown";
            }
        }

        [Display(Name = "Variety ID")] 
        public int VarietyId
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null && Seeds.OfficialVarietyId.HasValue)
                {
                    return Seeds.OfficialVarietyId.Value;
                }
                if(BlendId.HasValue && Blend != null)
                {
                    return Blend.GetVarietyId();
                }
                if(BulkVariety != null)
                {
                    return BulkVariety.Id;
                }
                if(AppId.HasValue && Application != null && Application.SelectedVarietyId.HasValue)
                {
                    return Application.SelectedVarietyId.Value;
                }
                return -1;
            }
        }

        [Display(Name = "Variety")]
        public string VarietyIdandName
        {
            get
            {
                return $"{VarietyId.ToString()} <span class='border p-2'>{VarietyName}</span>";
            }
        }

        [Display(Name = "Cert Number")] 
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
                if (BulkVariety != null)
                {
                    return "";
                }
                if(AppId.HasValue && Application != null)
                {
                    return Application.FullCert;
                }
                return "Unknown";
            }
        }

        [Display(Name = "Lot Number")] 
        public string LotNumber
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.LotNumber;
                }
                return "";
            }
        }

        [Display(Name = "SID Class")] 
        public string SIDClass
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null && Seeds.ClassProduced != null)
                {
                    return Seeds.ClassProduced.CertClass;
                }
                else if (BlendId.HasValue)
                {
                    return "Certified Blend";
                }
                return "";
            }
        }

        [Display(Name = "Class Produced")] 
        public string ClassProduced
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null && Seeds.ClassProduced != null)
                {
                    return Seeds.ClassProduced.CertClass;
                }
                else if (BlendId.HasValue)
                {
                    return "Certified Blend";
                }
                else if(AppId.HasValue && Application != null)
                {
                    return Application.ClassProducedName;
                }
                return "";
            }
        }




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
                else if (AppId.HasValue)
                {
                    return AppId;
                }
                else if (Bulk)
                {
                    return null;
                }
                else
                {
                    return null;
                }
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.0}")]
        [Display(Name = "Source Lot Weight")] 
        public decimal SourceLotWeight
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null)
                {
                    return Seeds.PoundsLot;
                }
                if(BlendId.HasValue && Blend != null && Blend.LbsLot.HasValue)
                {
                    return Blend.LbsLot.Value;
                }
                if(BlendId.HasValue && Blend != null && (Blend.BlendType == BlendType.Varietal.GetDisplayName() || Blend.BlendType == BlendType.Lot.GetDisplayName()) && Blend.LotBlends != null)
                {
                    decimal varietalTotal = 0;
                    foreach(var row in Blend.LotBlends)
                    {
                        varietalTotal += row.Weight;                        
                    }
                    return varietalTotal;
                }
                if(BlendId.HasValue && Blend != null && Blend.BlendType == BlendType.InDirt.GetDisplayName() && Blend.InDirtBlends != null)
                {
                    decimal inDirtTotal = 0;
                    foreach(var row in Blend.InDirtBlends)
                    {
                        inDirtTotal += row.Weight;                        
                    }
                    return inDirtTotal;
                }
                if(AppId.HasValue && Application != null && Application.FieldInspectionReport != null)
                {
                    return Application.FieldInspectionReport.PotatoPoundsHarvested;
                }
                return 0;
            }
        }

        [Display(Name = "OECD Warning")]
        public string OECDWarning
        {
            get
            {
                if (SeedsID.HasValue && Seeds != null && Seeds.LabResults == null)
                {
                    return "SID has no lab results";
                }
                if (SeedsID.HasValue && Seeds != null && !Seeds.LabResults.CciaConfirmed)
                {
                    return "SID has lab results, but they have not been confirmed";
                }
                if (BlendId.HasValue && Blend != null && Blend.Labs == null)
                {
                    return "Blend has no lab results";
                }
                if (BlendId.HasValue && Blend != null && !Blend.Labs.CciaConfirmed)
                {
                    return "Blend has lab results, but they have not been confirmed";
                }
                if (AppId.HasValue)
                {
                    return "Tags with AppID should not go OECD";
                }
                return "";
            }
        }


        public int? OECDId { get; set; }
        
        [ForeignKey("OECDId")]
        public OECD OECDFile { get; set; }
       
        public int? TagClass { get; set; }

        [ForeignKey("TagClass")]
        public AbbrevClassSeeds TagAbbrevClass { get; set; }

       
        [Display(Name = "Date Requested")] 
        public DateTime? DateRequested { get; set; }

        public int YearRequested { get { return DateRequested.HasValue ? DateRequested.Value.Year : 0; } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Needed")]
        public DateTime? DateNeeded { get; set; }
        public DateTime? DateRun { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.0}")]
        public decimal? LotWeightBagged { get; set; }
        [Display(Name = "Coating %")] 
        public decimal? CoatingPercent { get; set; }
        public string WeightUnit { get; set; }
        [Display(Name = "Count Requested")] 
        public int? CountRequested { get; set; }
        [Display(Name ="Extras/Overrun")]
        public int? ExtrasOverrun { get; set; }
        public decimal? BagSize { get; set; }
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.00}")]
        [Display(Name = "Bag Size (Pounds)")] 
        public decimal? BagSizePounds
        {
            get
            {
                decimal k = 2.20462262M;
                if (!BagSize.HasValue)
                {
                    return null;
                }
                else
                {
                    switch (WeightUnit)
                    {
                        case "L":
                            return BagSize.Value;
                        default:
                            return BagSize.Value * k;
                    }
                }
            }
        }

        [NotMapped]                
        public string Warning
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null && Seeds.Status != "SIR ready")
                {
                    return "Attached SID status is not marked 'SIR Ready'!!";
                }
                return "";
            }
        }

        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.00}")]
        [Display(Name = "Bag Size (Kilograms)")] 
        public decimal? BagSizeKilos
        {
            get
            {
                decimal k = 2.20462262M;
                if (!BagSize.HasValue)
                {
                    return null;
                }
                else
                {
                    switch (WeightUnit)
                    {
                        case "L":
                            return BagSize.Value / k;
                        default:
                            return BagSize.Value;
                    }
                }
            }
        }
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,##0.0}")]
        public decimal? LotWeightRequested
        {
            get
            {
                decimal k = 2.20462262M;
                if (!CountRequested.HasValue || !ExtrasOverrun.HasValue || !BagSize.HasValue)
                {
                    return null;
                }
                else
                {
                    switch (WeightUnit)
                    {
                        case "L":
                            return (CountRequested.Value - ExtrasOverrun.Value) * BagSize.Value;
                        default:
                            return (CountRequested.Value - ExtrasOverrun.Value) * BagSize.Value * k;
                    }
                }
            }
        }
        [Display(Name ="Count Used")]
        public int? CountUsed { get; set; }
        public int? TagType { get; set; }

        [ForeignKey("TagType")]
        public AbbrevTagType AbbrevTagType { get; set; }
        public string Statement { get; set; }
        public string Comments { get; set; }
        public int? Contact { get; set; }
        public string UserPrinted { get; set; }
        [ForeignKey("UserPrinted")]
        public CCIAEmployees EmployeePrinted { get; set; }
        public int? UserEntered { get; set; }
        [ForeignKey("UserEntered")]
        public Contacts ContactEntered { get; set; }
        public DateTime? DateEntered { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateModified { get; set; }

        public int? TaggingOrg { get; set; }

        [ForeignKey("TaggingOrg")]
        public Organizations TaggingOrganization {get; set;}

        public bool Bulk { get; set; }

        public bool Pretagging { get; set; }

        // public bool SeriesNumbered { get; set; }

        [Display(Name ="Analysis Requested?")]
        public bool AnalysisRequested { get; set; }

        [Display(Name ="How Deliver")]
        public string HowDeliver { get; set; }

        public string TrackingNumber { get; set; }

        public string TrackingLink { 
            get
            {
                if(!string.IsNullOrWhiteSpace(TrackingNumber))
                {
                   return $"https://wwwapps.ups.com/WebTracking/processInputRequest?sort_by=status&error_carried=true&tracknums_displayed=1&TypeOfInquiryNumber=T&loc=en-us&InquiryNumber1={TrackingNumber}&AgreeToTermsAndConditions=yes";
                }
                return "";

            } 
        }

        [Display(Name = "Potato Tag Statement")]
        public string PotatoTagStatement
        {
            get
            {
                if(AppId.HasValue && Application != null && Application.AppType == "PO" && Country != null && State != null )
                {
                    if(Country.Name != "Canada" && Country.Name != "United States")
                    {
                        return $"For export to {Country.Name} only";
                    }
                   if(State.Name == "California")
                    {
                        return "For in-state use only";
                    }
                   if(State.ProducesCertifiedSeedPotatoes || Country.Name == "Canada")
                    {
                        return "Post-Harvest Test Required";
                    }
                   if(!State.ProducesCertifiedSeedPotatoes)
                    {
                        return $"Destination {State.Name}: Post Harvest Test not required";
                    }
                    return "Can not determine PHT statement!";
                }
                if (AppId.HasValue && Application != null && Application.AppType == "PO")
                {
                    return "Missing destination Country and/or State. Please update.";
                }
                return "";
            }
        }

        public string OECDCertNumber {
            get 
            {
                if(BlendId != null)
                {
                    return $"USA-CA-{CertYear % 100}{CertNumber}";
                }
                if(Seeds != null){
                    if(Seeds.OriginState == 102)
                    {
                        return $"USA-CA-{CertNumber}";
                    }
                    if(Seeds.OriginCountry == 58 && Seeds.StateOfOrigin != null)
                    {
                        return $"USA-{Seeds.StateOfOrigin.StateProvinceCode}/CA-{Seeds.CertNumber}-{Seeds.LotNumber}";
                    }
                    if(Seeds.CountryOfOrigin != null)
                    {
                        return $"USA-{Seeds.CountryOfOrigin.Code}/CA-{Seeds.CertNumber}-{Seeds.LotNumber}";
                    }
                }
                return "Cannot determine";
            }
        }

        public string Stage { get; set; }

        public string UserApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        [Display(Name ="Date Printed")]
        public DateTime? PrintedDate { get; set; }

        public string Alias { get; set; }

        public bool OECD { get; set; }

        [Display(Name ="Planting Stock #")]
        public string PlantingStockNumber { get; set; }

        public int? OECDTagType { get; set; }
        [ForeignKey("OECDTagType")]
        public AbbrevOECDClass OECDClass { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Date Sealed")]
        public DateTime? DateSealed { get; set; }

        public int? OECDCountryId { get; set; }
        [ForeignKey("OECDCountryId")]
        public Countries OECDCountry {get; set;}

        [Display(Name ="Admin Comments")]
        public string AdminComments { get; set; }

        [Display(Name = "Series Request")] 
        public bool SeriesRequest { get; set; }

        public int? BulkCropId { get; set; }

        [Display(Name = "Destination Country")]
        public int? DestinationCountry { get; set; }
        [Display(Name = "Destination State")]
        public int? DestinationState { get; set; }

        [ForeignKey("BulkCropId")]
        public Crops BulkCrop { get; set; }

        public int? BulkVarietyId { get; set; }

        [ForeignKey("BulkVarietyId")]
        public VarFull BulkVariety { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagDocuments> Documents { get; set; }

        [ForeignKey("DestinationCountry")]
        public Countries Country { get; set; }

        [ForeignKey("DestinationState")]
        public StateProvince State { get; set; }




    }
}
