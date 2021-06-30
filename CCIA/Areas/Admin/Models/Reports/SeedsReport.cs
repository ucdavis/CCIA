using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedsReport
    {
       public string CountyName { get; set; }

       public string crop { get; set; }
       public string crop_kind { get; set; }

       public string group { get; set; }
       public int cert_year { get; set; }
       public decimal pounds_passed { get; set; }
       
       public int number_certified { get; set; }
        
    }
}
