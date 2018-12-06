using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabGermination
    {
        public int LabId { get; set; }
        public DateTime? DatePlanted { get; set; }
        public bool? PreChill { get; set; }
        public int? PreChillDays { get; set; }
        public int? NumSeedsPlanted { get; set; }
        public string Substrate { get; set; }
        public int? Replicates { get; set; }
        public string Temperature { get; set; }
        public string Comments { get; set; }
        public string StartedBy { get; set; }
        public int? ReportGerm { get; set; }
        public int? ReportHard { get; set; }
        public bool? InsufficientSizeGerm { get; set; }
        public int? CalcGerm { get; set; }
        public int? CalcAbnormal { get; set; }
        public int? CalcHard { get; set; }
        public int? CalcDormant { get; set; }
        public int? CalcDead { get; set; }
    }
}
