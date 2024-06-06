using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedsReport
    {
       public string CountyName { get; set; }

       public string crop { get; set; }
       public string crop_kind { get; set; }

       public string report_group { get; set; }
       [Column(TypeName ="numeric(16,2)")]
       public decimal pounds_passed { get; set; }
       
       public int number_certified { get; set; }
        
    }

    public partial class RebateReport
    {
        public int ConditionerId { get; set; }

        public string ConditionerName { get; set; }

        public string crop { get; set; }
        public string cropKind { get; set; }
       
        [Column(TypeName = "numeric(16,2)")]
        public decimal totalPounds { get; set; }

        public decimal totalFee { get; set; }

        public int numberCertified { get; set; }
        public bool NFC { get; set; }

    }
}
