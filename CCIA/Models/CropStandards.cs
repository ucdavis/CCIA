using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class CropStandards
    {
        public int CropId { get; set; }
        public Crops Crops { get; set; }
        public int StdId { get; set; }
        public Standards Standards { get; set; }
    }
}
