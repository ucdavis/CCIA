using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCIA.Models
{
    public partial class Organizations
    {
        public int OrgId { get; set; }
        [DisplayName("Org Name")] 
        public string OrgName { get; set; }
        
        public Address Address { get; set; }
        public int? AddressId { get; set; }

        public string Phone { get; set; }

        public string Email { get; set; }

        public string NameAndId => OrgId + " " + OrgName;

        public int? CountyId { get; set; }

        public bool GermLab { get; set; }



       
        public ICollection<Applications> AppliedApplications { get; set; }

    }
}
