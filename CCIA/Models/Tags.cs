using System;
using System.Collections.Generic;
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
        public DateTime? DateNeeded { get; set; }
        public DateTime? DateRun { get; set; }
        public decimal? LotWeightBagged { get; set; }
        public decimal? CoatingPercent { get; set; }        
        public string WeightUnit { get; set; }
        public int? CountRequested { get; set; }
        public int? ExtrasOverrun { get; set; }
        public numeric? BagSize { get; set; }
        [NotMapped]
        public int BagSizePounds { 
            get{
                switch(WeightUnit) {
                    case "L":
                        BagSize;
                        break;
                    default:
                        BagSize * 2.20462262;
                        break;
                }

            }
        }
        [NotMapped]
        public int LotWeightRequested { 
            get {
                switch(WeightUnit) {
                    case "L":
                        (CountRequested - ExtrasOverrun) * BagSize;
                        break;
                    default:
                        (CountRequested - ExtrasOverrun) * BagSize * 2.20462262;
                        break;
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
    }
}
