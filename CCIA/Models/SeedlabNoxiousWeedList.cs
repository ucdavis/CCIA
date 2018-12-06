using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabNoxiousWeedList
    {
        public int NoxiousListId { get; set; }
        public int LabId { get; set; }
        public int ListId { get; set; }
        public int NumberFound { get; set; }
        public double? ReportRate { get; set; }
    }
}
