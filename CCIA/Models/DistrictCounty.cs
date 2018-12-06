using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class DistrictCounty
    {
        public short DistCountyId { get; set; }
        public short? DistId { get; set; }
        public short? CountyId { get; set; }
        public string Comments { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserModified { get; set; }
    }
}
