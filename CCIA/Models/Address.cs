using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int? ReferenceId { get; set; }
        public int? OcId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string City { get; set; }
        public int? CountyId { get; set; }
        public int StateProvinceId { get; set; }
        public string PostalCode { get; set; }
        public int CountryId { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
    }
}
