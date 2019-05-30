using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class SeedApp : Applications {
        
    }

    public class SeedPlantingStocks : PlantingStocks {
        [Required]
        public int? PsClass { 
            get { return base.PsClass; }
            set { base.PsClass = value; } 
        }
    }
}