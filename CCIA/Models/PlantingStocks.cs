using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class PlantingStocks
    {
        public PlantingStocks()
        {
            WinterTest = false;
            PvxTest = false;
            StateCountryGrown = 102;
            StateCountryTagIssued = 102;
        }
        public int PsId { get; set; }
        public int? AppId { get; set; }

        [Required]
        [Display(Name="Planting Stock Cert Number")]
        public string PsCertNum { get; set; }
        public virtual string PsEnteredVariety { get; set; }
        public int? OfficialVarietyId { get; set; }
        
        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00.0}")]
        public virtual decimal? PoundsPlanted { get; set; }
        public virtual int? PsClass { get; set; }
        public int? PsAccession { get; set; }
        public int? StateCountryTagIssued { get; set; }
        public int? StateCountryGrown { get; set; }
        public virtual string SeedPurchasedFrom { get; set; }
        public virtual bool WinterTest { get; set; }
        public virtual bool PvxTest { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateEntered { get; set; }
        public int? UserCreator { get; set; }

        public string ThcPercent { get; set; }
        public decimal? PlantsPerAcre { get; set; }

        public string UserEmpModified { get; set; }

    
        
        [ForeignKey("PsClass")]
        public AbbrevClassProduced PsClassNavigation { get; set; }

       
       [ForeignKey("StateCountryGrown")]
       public StateProvince GrownStateProvince {get; set; }

       [ForeignKey("StateCountryGrown")]
       public Countries GrownCountry { get; set; }

       [ForeignKey("StateCountryTagIssued")]
       public StateProvince TaggedStateProvince { get; set; }

       [ForeignKey("StateCountryTagIssued")]
       public Countries TaggedCountry { get; set; }

       
    }
}
