using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class HempFromSeedApp : MasterApplicationViewModel
    {
        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }

        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<HempFromSeedPlantingStocks> AppSpecificPlantingStocks { get; set; }

        [Required]
        public override string CountyPermit
        {
            get { return base.CountyPermit; }
            set { base.CountyPermit = value; }
        }
    }

    public class HempFromSeedPlantingStocks : PlantingStocks
    {
        [Required]
        public override decimal? PoundsPlanted
        {
            get { return base.PoundsPlanted; }
            set { base.PoundsPlanted = value; }
        }
        [Required]
        public override int? PsClass
        {
            get { return base.PsClass; }
            set { base.PsClass = value; }
        }
        [Required]
        public override string PsEnteredVariety
        {
            get { return base.PsEnteredVariety; }
            set { base.PsEnteredVariety = value; }
        }
    }
}