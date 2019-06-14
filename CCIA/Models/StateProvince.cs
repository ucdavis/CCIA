using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class StateProvince
    {
        public int StateProvinceId { get; set; }
        public string StateProvinceCode { get; set; }
        public int CountryId { get; set; }
        public string StateProvinceName { get; set; }
        public DateTime DateModified { get; set; }


       
    }
}
