using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{   
    public partial class VarietyBlendComponents
    {
        public int Id { get; set; }

        public int BlendVarietyId { get; set; }

        [ForeignKey("BlendVarietyId")]
        public VarFull BlendVariety { get; set; }

        public int ComponentVarietyId { get; set; }

        [ForeignKey("ComponentVarietyId")]
        public VarFull ComponentVariety { get; set; }

        public decimal ComponentPercent { get; set; }
        
    }
}
