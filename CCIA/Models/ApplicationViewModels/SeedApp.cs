using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class SeedApp : MasterApplicationViewModel
    {
        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<SeedPlantingStocks> PlantingStocks;

        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }
    }

    public class SeedPlantingStocks : PlantingStocks {
        [Required]
        public override int? PsClass { 
            get { return base.PsClass; }
            set { base.PsClass = value; } 
        }
    }
}