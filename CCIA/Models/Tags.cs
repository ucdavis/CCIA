using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "TagID")]
        public int Id { get; set; }
        public int? SeedsID { get; set; }
        [ForeignKey("SeedsID")]
        public Seeds Seeds { get; set; }
        public int? BlendId { get; set; }
        [ForeignKey("BlendId")]
        public BlendRequests Blend { get; set; }
        public int? PotatoAppId { get; set; }
        [ForeignKey("PotatoAppId")]
        public Applications Application {get; set;}

        
        public TagBagging TagBagging { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagSeries> TagSeries { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagChanges> Changes { get; set; }
        
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
                else if (PotatoAppId.HasValue)
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
                if(PotatoAppId.HasValue && Application != null)
                {
                    return Application.CertYear;
                }
                else 
                {
                    return 0;
                }
            }
        }

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
                if(PotatoAppId.HasValue && Application != null)
                {
                    return Application.CropName;
                }
                return "Unknown";
            }
        }

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
                if(PotatoAppId.HasValue && Application != null)
                {
                    return Application.VarietyName;
                }
                return "Unknown";
            }
        }

        public int VarietyId
        {
            get
            {
                if(SeedsID.HasValue && Seeds != null && Seeds.OfficialVarietyId.HasValue)
                {
                    return Seeds.OfficialVarietyId.Value;
                }
                if(BlendId.HasValue && Blend != null && Blend.VarietyId.HasValue)
                {
                    return Blend.VarietyId.Value;
                }
                if(BulkVariety != null)
                {
                    return BulkVariety.Id;
                }
                if(PotatoAppId.HasValue && Application != null && Application.SelectedVarietyId.HasValue)
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
                return $"{VarietyId.ToString()} {VarietyName}";
            }
        }

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
                return "Unknown";
            }
        }

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
                else if(PotatoAppId.HasValue && Application != null)
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
                else if (PotatoAppId.HasValue)
                {
                    return PotatoAppId;
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

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
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
                return 0;
            }
        }

        
        public int? OECDId { get; set; }
       
        public int? TagClass { get; set; }

        [ForeignKey("TagClass")]
        public AbbrevClassSeeds TagAbbrevClass { get; set; }

       
        public DateTime? DateRequested { get; set; }

        public int YearRequested { get { return DateRequested.HasValue ? DateRequested.Value.Year : 0; } }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DateNeeded { get; set; }
        public DateTime? DateRun { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
        public decimal? LotWeightBagged { get; set; }
        public decimal? CoatingPercent { get; set; }
        public string WeightUnit { get; set; }
        public int? CountRequested { get; set; }
        public int? ExtrasOverrun { get; set; }
        public decimal? BagSize { get; set; }
        [NotMapped]
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
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
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
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

        public bool SeriesNumbered { get; set; }

        public bool AnalysisRequested { get; set; }

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

        public string Stage { get; set; }

        public string UserApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? PrintedDate { get; set; }

        public string Alias { get; set; }

        public bool OECD { get; set; }

        public string PlantingStockNumber { get; set; }

        public int? OECDTagType { get; set; }
        [ForeignKey("OECDTagType")]
        public AbbrevOECDClass OECDClass { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DateSealed { get; set; }

        public int? OECDCountryId { get; set; }
        [ForeignKey("OECDCountryId")]
        public Countries OECDCountry {get; set;}

        public string AdminComments { get; set; }

        public bool SeriesRequest { get; set; }

        public int? BulkCropId { get; set; }

        [ForeignKey("BulkCropId")]
        public Crops BulkCrop { get; set; }

        public int? BulkVarietyId { get; set; }

        [ForeignKey("BulkVarietyId")]
        public VarFull BulkVariety { get; set; }

        [ForeignKey("TagId")]
        public ICollection<TagDocuments> Documents { get; set; }



    }
}
