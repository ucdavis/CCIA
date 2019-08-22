using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class RiceQAApp : MasterApplicationViewModel
    {
        // Overrides MasterApplicationViewModel's AppViewModel since it is always of the same type.
        public override ApplicationViewModel AppViewModel { get; set; }

        // Hidden to account for different variations of PlantingStocks on each application type.
        public ICollection<RiceQAPlantingStocks> AppSpecificPlantingStocks { get; set; }
    }

    public class RiceQAPlantingStocks : PlantingStocks
    {
        [System.ComponentModel.DisplayName("Pounds Planted")]
        [Required(AllowEmptyStrings = false)]
        [DisplayFormat(ConvertEmptyStringToNull = false)]
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