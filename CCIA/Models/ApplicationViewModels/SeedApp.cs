using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class SeedApp : Applications 
    {
        public List<SeedPlantingStocks> PlantingStocks { get; set; }
    }

    public class SeedPlantingStocks : PlantingStocks {
        [Required]
        public override int? PsClass { 
            get { return base.PsClass; }
            set { base.PsClass = value; } 
        }
    }
}