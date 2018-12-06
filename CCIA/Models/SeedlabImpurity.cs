using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabImpurity
    {
        public int ImpurityListId { get; set; }
        public int LabId { get; set; }
        public int ListId { get; set; }
        public string ImpurityType { get; set; }
        public short NumberFound { get; set; }
        public double? ReportRate { get; set; }
        public string Fraction { get; set; }
    }
}
