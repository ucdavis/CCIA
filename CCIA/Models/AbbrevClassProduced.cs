using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class AbbrevClassProduced
    {
        public AbbrevClassProduced()
        {            
            PlantingStocks = new HashSet<PlantingStocks>();
        }
        [Key]
        public int ClassProducedId { get; set; }
        public string ClassAbbrv { get; set; }
        public string ClassProducedTrans { get; set; }
        public int? SortOrder { get; set; }
        public int? AppType { get; set; }

       
        public ICollection<PlantingStocks> PlantingStocks { get; set; }
    }
}
