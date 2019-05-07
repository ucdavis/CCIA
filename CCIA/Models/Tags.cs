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
        public int? BlendId { get; set; }
        public int? PotatoAppId { get; set; }
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

        public int TaggingOrg { get; set; }

    }
}
