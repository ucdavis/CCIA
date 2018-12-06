using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabPurityLists
    {
        public int PurityListId { get; set; }
        public int LabId { get; set; }
        public string Type { get; set; }
        public int ListId { get; set; }
        public short? Count { get; set; }
        public double? Grams { get; set; }
    }
}
