using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class HempFromSeedApp : MasterApplicationViewModel
    {
        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }
        
        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<SeedPlantingStocks> PlantingStocks;
    }

    public class HempFromSeedPlantingStocks : PlantingStocks {
        
    }
}