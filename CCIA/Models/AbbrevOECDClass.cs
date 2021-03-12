using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class AbbrevOECDClass
    {
        
        [Key]
        public int Id { get; set; }       
        [Display(Name="OECD Class")]
        public string Class { get; set; }
        public int? SortOrder { get; set; }
       
        
    }
}
