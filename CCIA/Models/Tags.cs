using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
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

        public string IdType { 
            get{
                if(SeedsID.HasValue){
                    return "SID";
                } else if (BlendId.HasValue) {
                    return "BID";
                } else if (PotatoAppId.HasValue) {
                    return "AppID";
                } else if (Bulk) {
                    return "Bulk";
                } else {
                    return "";
                }
            } 
        }

        public string CropName { 
            get{
                if(SeedsID.HasValue) { 
                    if(Seeds.OfficialVarietyId != null){
                        return Seeds.Variety.Crop.Name;
                    } else {
                        return "";
                    }                    
                } else if(BlendId.HasValue){
                    if(Blend != null && Blend.LotBlend != null && Blend.LotBlend.Seeds != null && Blend.LotBlend.Seeds.Variety != null && Blend.LotBlend.Seeds.Variety.Crop != null) {
                        return Blend.LotBlend.Seeds.Variety.Crop.Name; 
                    } else { return "";}
                }
                else {
                    return "";
                }

            } 
        }

        public int? LinkId { 
            get {
                if(SeedsID.HasValue){
                    return SeedsID;
                 } else if (BlendId.HasValue) {
                    return BlendId;
                } else if (PotatoAppId.HasValue) {
                    return PotatoAppId;
                } else if (Bulk) {
                    return null;
                } else {
                    return null;
                }
            }
        }
        public int? OECDId { get; set; }
        public int? TagClass { get; set; }
        public DateTime? DateRequested { get; set; }

        public int YearRequested { get { return DateRequested.HasValue ? DateRequested.Value.Year : 0; } }


        public DateTime? DateNeeded { get; set; }
        public DateTime? DateRun { get; set; }
        public decimal? LotWeightBagged { get; set; }
        public decimal? CoatingPercent { get; set; }        
        public string WeightUnit { get; set; }
        public int? CountRequested { get; set; }
        public int? ExtrasOverrun { get; set; }
        public decimal? BagSize { get; set; }
        [NotMapped]
        public decimal? BagSizePounds { 
            get{
                decimal k =  2.20462262M;
                if(!BagSize.HasValue) {
                    return null;
                } else {
                    switch(WeightUnit) {
                        case "L":
                            return BagSize.Value;
                        default:
                            return BagSize.Value * k;
                    }
                }
            }
        }
        [NotMapped]
        public decimal? LotWeightRequested { 
            get {
                decimal k =  2.20462262M;
                if(!CountRequested.HasValue || !ExtrasOverrun.HasValue || !BagSize.HasValue){
                    return null;
                } else {
                    switch(WeightUnit) {
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
        public string Statement { get; set; }
        public string Comments { get; set; }
        public int? Contact { get; set; }
        public string UserPrinted { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateEntered { get; set; }

        public string UserModified { get; set; }

        public DateTime? DateModified { get; set; }

        public int TaggingOrg { get; set; }

        public bool Bulk { get; set; }

        public bool Pretagging { get; set; }

        public bool SeriesNumbered { get; set; }

        public bool AnalysisRequested { get; set; }

        public string HowDeliver { get; set; }

        public string TrackingNumber { get; set; }

        public string Stage { get; set; }

        public string UserApproved { get; set; }

        public DateTime? ApprovedDate { get; set; }

        public DateTime? PrintedDate { get; set; }

        public string Alias { get; set; }

        public bool OECD { get; set; }

        public string PlantingStockNumber { get; set; }

        public int? OECDTagType { get; set; }

        public DateTime? DateSealed { get; set; }

        public int? OECDCountry { get; set; }

        public string AdminComments { get; set; }

        public bool SeriesRequest { get; set; }

        public int? BulkCropId { get; set; }

        public int? BulkVarietyId { get; set; }



    }
}
