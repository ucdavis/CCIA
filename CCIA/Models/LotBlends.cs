using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class LotBlends
    {
        public int CompId { get; set; }
        public int BlendId { get; set; }
        public int Sid { get; set; }
        public decimal Weight { get; set; }
    }
}
