using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabDodder
    {
        public int LabId { get; set; }
        public decimal? WeightWorkingSample { get; set; }
        public bool? Dodder { get; set; }
        public decimal? WeightDodder { get; set; }
        public string CompletedBy { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string Comments { get; set; }
    }
}
