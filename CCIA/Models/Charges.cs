using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Charges
    {
        public Charges()
        {
           HoldCheck = false;
           Correction = false;
           Approval = false;
        }
        
        public int ChgId { get; set; }
        public int LinkId { get; set; }
        public string LinkType { get; set; }
        public int OrgId { get; set; }
        [ForeignKey("OrgId")]
        public Organizations Organization { get; set; }

        public string BatchNumber { get; set; }
        public string ChargeCategory { get; set; }
        public decimal? ChargeAmount { get; set; }
        public string Description { get; set; }
        public DateTime? DateEntered { get; set; }
        public DateTime? DateApplied { get; set; }
        public bool HoldCheck { get; set; }        
        public DateTime? HoldDate { get; set; }
        public int Deletecharge { get; set; }
        public bool Correction { get; set; }
        public bool Approval { get; set; }
        public string Approver { get; set; }
        public string Note { get; set; }
    }
}
