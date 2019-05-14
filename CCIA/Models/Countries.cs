using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Countries
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public bool? OecdMember { get; set; }
       
    }
}
