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
        public decimal Weight { get; set; }

        public string LotNumber { get; set; }

        public string VarietyName { get; set; }

        public string ClassName { get; set; }

        public decimal? GermValue { get; set; }

        public decimal? HardValue { get; set; }

        public decimal? TotalGermValue { get; set; }

        public decimal? PurityValue { get; set; }

        public decimal? OtherCropValue { get; set; }

        public decimal? InertValue { get; set; }

        public decimal? WeedValue { get; set; }

        public string  LabDate { get; set; }

        public bool DifferentConditioner { get; set; }

        public string CertificationResults { get; set; }       
       
        
    }
}
