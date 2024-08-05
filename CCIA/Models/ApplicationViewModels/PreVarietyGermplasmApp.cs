using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class PreVarietyGermplasmApp : MasterApplicationViewModel
    {
        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }
        
        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<PreVarietyGermplasmPlantingStocks> AppSpecificPlantingStocks { get; set; }
    }

    public class PreVarietyGermplasmPlantingStocks : PlantingStocks {
        
    }


    public class NativeSeedApp : MasterApplicationViewModel
    {
        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }

        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<NativeSeedPlantingStocks> AppSpecificPlantingStocks { get; set; }
    }

    public class NativeSeedPlantingStocks : PlantingStocks
    {

    }
}