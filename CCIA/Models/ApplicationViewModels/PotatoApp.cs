using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class PotatoApp : MasterApplicationViewModel 
    {
        public override ApplicationViewModel AppViewModel { get; set; }
        public ICollection<PotatoPlantingStocks> PlantingStocks { get; set; }
    }

    public class PotatoPlantingStocks : PlantingStocks {
        [Required]
        public override int? PsClass { 
            get { return base.PsClass; }
            set { base.PsClass = value; } 
        }
        [Required]
        public override bool? WinterTest { 
            get { return base.WinterTest; }
            set { base.WinterTest = value; } 
        }
        [Required]
        public override bool? PvxTest { 
            get { return base.PvxTest; }
            set { base.PvxTest = value; } 
        }
    }
}