using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabGerminationReplicate
    {
        public int RepId { get; set; }
        public int ReadId { get; set; }
        public int RepNum { get; set; }
        public int? GermCount { get; set; }
        public int? HardCount { get; set; }
        public int? DormantFresh { get; set; }
        public int? DeadSeed { get; set; }
        public int? AbnormalSeed { get; set; }
        public int? Remainder { get; set; }
    }
}
