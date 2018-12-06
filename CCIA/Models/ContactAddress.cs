using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class ContactAddress
    {
        public int ContaddId { get; set; }
        public int ContactId { get; set; }
        public int AddressId { get; set; }
        public bool? Mailing { get; set; }
        public bool? Billing { get; set; }
        public bool? Delivery { get; set; }
        public bool? PhysicalLoc { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public bool? Active { get; set; }
    }
}
