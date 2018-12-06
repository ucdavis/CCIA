using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabNoxiousWeed
    {
        public int LabId { get; set; }
        public double? WeightWorkingSample { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string CompletedBy { get; set; }
        public string Comments { get; set; }
        public bool? InsufficientSizeNoxious { get; set; }
        public double? WeightDodder { get; set; }
        public DateTime? DateDodder { get; set; }
        public string DodderCompletedBy { get; set; }
        public string DodderResult { get; set; }
        public string DodderComments { get; set; }
        public int? DodderCount { get; set; }
        public bool? InsufficientSizeDodder { get; set; }
        public double? RedriceWeight { get; set; }
        public DateTime? RedriceDate { get; set; }
        public string RedriceCompletedBy { get; set; }
        public string RedriceResult { get; set; }
        public string RedriceComments { get; set; }
        public bool? RedriceInsufficientSize { get; set; }
        public int? RedriceCount { get; set; }
    }
}
