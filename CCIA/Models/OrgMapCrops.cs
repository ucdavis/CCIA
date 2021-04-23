using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CCIA.Models
{
    public partial class OrgMapCrops
    {
         public int Id { get; set; }
        public int OrgId { get; set; }
        public int CropId { get; set; }
        [ForeignKey("CropId")]
        public Crops Crop { get; set; }
        public bool Allow { get; set; }
    }
}
