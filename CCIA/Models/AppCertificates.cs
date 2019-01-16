using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Models
{
    public partial class AppCertificates
    {
        [Key]
        public int CertId { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        
        public Applications Application { get; set; }

    }
}