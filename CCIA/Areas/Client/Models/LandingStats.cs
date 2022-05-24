using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
     public partial class LandingStats
    {
        
        public string Type { get; set; }

        public string Status { get; set; }

        public int Year { get; set; }

        public int Count { get; set; }
    }
}