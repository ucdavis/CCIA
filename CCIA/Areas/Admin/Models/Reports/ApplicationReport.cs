using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class ApplicationReport
    {
       public string CountyName { get; set; }

       public string crop { get; set; }
       public string crop_kind { get; set; }
       public string var_name { get; set; }
       public int cert_year { get; set; }
       public decimal acres_applied { get; set; }

       public decimal acres_approved { get; set; }
       public decimal acres_cancelled { get; set; }
       public decimal acres_rejected { get; set; }
       public decimal acres_no_crop { get; set; }
       public int number_apps { get; set; }
        
    }
}
