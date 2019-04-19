using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace CCIA.Models
{
    public partial class Organizations
    {
        public int OrgId { get; set; }
        public string OrgName { get; set; }
        
        public Address Address { get; set; }
        public int AddressId { get; set; }


       
        public ICollection<Applications> AppliedApplications { get; set; }

    }
}
