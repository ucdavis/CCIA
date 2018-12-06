using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Rates
    {
        public long RateId { get; set; }
        public string Type { get; set; }
        public string Item { get; set; }
        public decimal? Cost { get; set; }
        public string Unit { get; set; }
        public bool? Active { get; set; }
        public DateTime? Modified { get; set; }
        public string Comments { get; set; }
    }
}
