using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    [Keyless]
    public partial class InDirtBlendSummary
    {
        public int Id { get; set; }        

        public string AppId { get; set; }

        [Column(TypeName ="numeric(12,2)")]
        public decimal Weight { get; set; }

        public string Variety { get; set; }

        public string ClassName { get; set; }

        
        public string CertNumber { get; set; }

        [Column(TypeName ="decimal(18,1)")]
        public decimal Percent { get; set; }

        
       
        
    }
}
