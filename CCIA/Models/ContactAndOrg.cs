using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class ContactsAndOrg
    {

        public Contacts contact { get; set; }
        public Organizations Org { get; set; }

    }

}