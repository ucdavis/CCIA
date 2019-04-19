using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class AbbrevClassSeeds
    {
        
        [Key]
        public int Id { get; set; }
        public string Abbrv { get; set; }
        public string Class { get; set; }
        public int? SortOrder { get; set; }
       
        
    }
}
