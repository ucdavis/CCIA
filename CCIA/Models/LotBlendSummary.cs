using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    [Keyless]
    public partial class LotBlendSummary
    {
        public int CompId { get; set; }
        public string Sid { get; set; }
        [Column(TypeName ="numeric(16,2)")]
        public decimal Weight { get; set; }

        public string LotNumber { get; set; }

        public string VarietyName { get; set; }

        public string ClassName { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? GermValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? HardValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? TotalGermValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? PurityValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? OtherCropValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? InertValue { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal? WeedValue { get; set; }

        public string  LabDate { get; set; }

        public bool DifferentConditioner { get; set; }

        public string CertificationResults { get; set; }  

        [Column(TypeName ="decimal(18,1)")]
        public decimal ComponentPercent { get; set; }     
       
        
    }
}
