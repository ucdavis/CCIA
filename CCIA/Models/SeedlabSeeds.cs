using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabSeeds
    {
        public int LabId { get; set; }
        public int? CondId { get; set; }
        public string CondText { get; set; }
        public int CropId { get; set; }
        public int? VarOffId { get; set; }
        public string VarietyName { get; set; }
        public string LotNumber { get; set; }
        public bool? Treated { get; set; }
        public decimal? LotSize { get; set; }
        public string SeedLab { get; set; }
        public string CertNumber { get; set; }
        public string SubmittingLabNumber { get; set; }
    }
}
