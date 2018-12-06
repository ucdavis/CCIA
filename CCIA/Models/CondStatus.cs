using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class CondStatus
    {
        public int CondStatusId { get; set; }
        public int OrgId { get; set; }
        public short CondYear { get; set; }
        public string CondStatus1 { get; set; }
        public DateTime CondUpdate { get; set; }
        public bool? AllowPretag { get; set; }
        public bool? PrintSeries { get; set; }
        public bool? RequestCciaPrintSeries { get; set; }
        public DateTime? DatePretagApproved { get; set; }
    }
}
