using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class PlantingStocks
    {
        public int PsId { get; set; }
        public int? AppId { get; set; }
        public string PsCertNum { get; set; }
        public string PsEnteredVariety { get; set; }
        public int? OfficialVarietyId { get; set; }
        public decimal? PoundsPlanted { get; set; }
        public int? PsClass { get; set; }
        public int? PsAccession { get; set; }
        public int? StateCountryTagIssued { get; set; }
        public int? StateCountryGrown { get; set; }
        public string SeedPurchasedFrom { get; set; }
        public bool? WinterTest { get; set; }
        public bool? PvxTest { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateEntered { get; set; }
        public int? UserCreator { get; set; }

        public string ThcPercent { get; set; }
        public decimal? PlantsPerAcre { get; set; }

        public Applications Applications { get; set; }

        public int? AbbrevClassProducedClassProducedId {get; set; }

        public AbbrevClassProduced PsClassNavigation { get; set; }

       
       [ForeignKey("StateCountryGrown")]
       public StateProvince GrownStateProvince {get; set; }

       [ForeignKey("StateCountryTagIssued")]
       public StateProvince TaggedStateProvince { get; set; }

       
    }
}
