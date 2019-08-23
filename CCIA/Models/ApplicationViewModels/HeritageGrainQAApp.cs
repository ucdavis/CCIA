using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class HeritageGrainQAApp : MasterApplicationViewModel
    {
        public override ApplicationViewModel AppViewModel { get; set; }
        public List<HeritageGrainQAPlantingStocks> AppSpecificPlantingStocks { get; set; }
    }

    public class HeritageGrainQAPlantingStocks : PlantingStocks
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