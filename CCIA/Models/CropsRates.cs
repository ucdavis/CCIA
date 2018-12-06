using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class CropsRates
    {
        public int CropRateId { get; set; }
        public int CropId { get; set; }
        public string SubType { get; set; }
        public int RateId { get; set; }
        public DateTime? DateSet { get; set; }
    }
}
