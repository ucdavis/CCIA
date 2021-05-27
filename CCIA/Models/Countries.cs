using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class Countries
    {
        public int Id { get; set; }
        public string Code { get; set; }
        
        [Display(Name="Country")]
        public string Name { get; set; }
        public bool? OecdMember { get; set; }

        public bool US { get; set; }
       
    }
}
