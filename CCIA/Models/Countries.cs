using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Countries
    {
        public short CountryId { get; set; }
        public string CountryCode { get; set; }
        public string CountryName { get; set; }
        public bool? OecdMember { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
    }
}
