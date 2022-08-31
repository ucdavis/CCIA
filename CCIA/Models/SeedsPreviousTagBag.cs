using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedsPreviousTagBag
    {
        public int Id { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,0}")]
        public decimal Weight { get; set; }
        public string Source { get; set; }  
    }
}