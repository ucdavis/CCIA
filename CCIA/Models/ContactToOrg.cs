using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class ContactToOrg
    {
        public int ContOrgId { get; set; }
        public int? ContactId { get; set; }
        public string OrgId { get; set; }
        public string Comments { get; set; }
    }
}
