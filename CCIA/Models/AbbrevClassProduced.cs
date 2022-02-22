using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class AbbrevClassProduced
    {
       
        [Key]
        public int ClassProducedId { get; set; }
        public string ClassAbbrv { get; set; }
        public string ClassProducedTrans { get; set; }
        public int? SortOrder { get; set; }
        public int? AppTypeId { get; set; }

        [ForeignKey("AppTypeId")]
        public AbbrevAppType AppType { get; set; }

        public string HempProduction { get; set; }

        public bool HempPlanted { get; set; }

        public bool HempProduced { get; set; }

        public string NameAndAppType 
        { 
            get 
            {
                if(AppType != null)
                {
                    return $"{AppType.Abbreviation}-{ClassProducedTrans}";
                }
                return "";
            }
        }

       
       
    }
}
