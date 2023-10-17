using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class OECDReport
    {
       public string CountyName { get; set; }

       public string crop { get; set; }
       public string crop_kind { get; set; }

       public string OECDClass { get; set; }

       public string report_group { get; set; }
       public decimal pounds_oecd { get; set; }
       
       public int number_certificates{ get; set; }
        
    }
}
