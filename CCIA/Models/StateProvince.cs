using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class StateProvince
    {
        public int StateProvinceId { get; set; }
        public string StateProvinceCode { get; set; }
        public short CountryId { get; set; }
        public string StateProvinceName { get; set; }
        public DateTime DateModified { get; set; }


        public ICollection<PlantingStocks> GrownInPlantingStocks { get; set; }
        public ICollection<PlantingStocks> TaggedInPlantingStocks { get; set; }
    }
}
