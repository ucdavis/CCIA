using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Charges
    {
        public int ChgId { get; set; }
        public int LinkId { get; set; }
        public string LinkType { get; set; }
        public int OrgId { get; set; }
        public string Batchno { get; set; }
        public string ChgCategory { get; set; }
        public decimal? ChargeAmt { get; set; }
        public string Description { get; set; }
        public DateTime? DateEntered { get; set; }
        public DateTime? DateApplied { get; set; }
        public bool? Holdchk { get; set; }
        public string Holdinit { get; set; }
        public DateTime? Holddt { get; set; }
        public int Delcharge { get; set; }
        public bool? Correction { get; set; }
        public bool? Approval { get; set; }
        public string Approver { get; set; }
        public string Note { get; set; }
    }
}
