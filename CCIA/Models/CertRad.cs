using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class CertRad
    {
        public int CertNum { get; set; }
        public int CertYear { get; set; }
        public int Rad { get; set; }

        public ICollection<Applications> Applications { get; set; }
    }
}
