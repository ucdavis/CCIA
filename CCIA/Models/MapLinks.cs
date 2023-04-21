using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class MapLinks
    {
        [Key]
        public int CropptId { get; set; }
        public string Type { get; set; }
        public DateTime DatePlanted { get; set; }
        public string Variety { get; set; }
        public string Status { get; set; }
        public decimal Acres { get; set; }
        public string Description { get; set; }

    }   
}