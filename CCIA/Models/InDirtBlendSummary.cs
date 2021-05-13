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

        public decimal Weight { get; set; }

        public string Variety { get; set; }

        public string ClassName { get; set; }

        
        public string CertNumber { get; set; }

        public decimal Percent { get; set; }

        
       
        
    }
}
