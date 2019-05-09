using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BlendInDirtComponents
    {
        public int Id { get; set; }
        public int BlendId { get; set; }

        public int? AppId { get; set; }

        public decimal Weight { get; set; }

        public int? ApplicantId { get; set; }

        public int? CropId { get; set; }

        public int? OfficialVarietyId { get; set; }

        public int? CertYear { get; set; }

        public int? CountryOfOrigin { get; set; }

        public int? StateOfOrigin { get; set; }

        public string CertNumber { get; set; }

        public string LotNumber { get; set; }

        public int? Class { get; set; }

        public string LastEditBy { get; set; }

        

        
    }
}
