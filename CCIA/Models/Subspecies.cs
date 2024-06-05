using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class Subspecies
    {
        public int Id { get; set; }
        public int CropId { get; set; }
        public string CommonName { get; set; }
        public string Name { get; set; }


    }
}
