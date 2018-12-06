using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabGerminationRead
    {
        public int ReadId { get; set; }
        public int LabId { get; set; }
        public DateTime? DateRead { get; set; }
        public string ReadBy { get; set; }
        public string Comments { get; set; }
        public bool? Final { get; set; }
    }
}
