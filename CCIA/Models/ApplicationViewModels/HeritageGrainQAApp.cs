using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class HeritageGrainQAApp : MasterApplicationViewModel 
    {
        public override ApplicationViewModel AppViewModel { get; set; }
        public List<HeritageGrainQAPlantingStocks> PlantingStocks { get; set; }
    }

    public class HeritageGrainQAPlantingStocks : PlantingStocks {
        
    }
}