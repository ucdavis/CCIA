using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace CCIA.Models
{
    public partial class StateProvince
    {
        public int StateProvinceId { get; set; }
        public string StateProvinceCode { get; set; }
        public int CountryId { get; set; }
        [DisplayName("State")]        
        public string StateProvinceName { get; set; }
        public DateTime DateModified { get; set; }


       
    }
}
