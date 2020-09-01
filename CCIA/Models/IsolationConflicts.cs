using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class IsolationConflicts
    {
        public string Origin { get; set; }
        public int Id { get; set; }
        public int CertYear { get; set; }
        public string RenewedFrom { get; set; }
        public string Crop { get; set; }
        public string Variety { get; set; }
        public string Status { get; set; }
        public string Url { get; set; }
        
    }
}
