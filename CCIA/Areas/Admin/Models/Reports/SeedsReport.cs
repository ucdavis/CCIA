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
}
