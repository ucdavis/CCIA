using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class FieldHistory
    {
        [Key]
        public int Id { get; set; }

        public int AppId { get; set; }
        public int? Year { get; set; }
        public int? Crop { get; set; }
        public string Variety { get; set; }
        public string AppNumber { get; set; }

       
        [ForeignKey("Crop")]
        public Crops FHCrops { get; set; }

    }
}