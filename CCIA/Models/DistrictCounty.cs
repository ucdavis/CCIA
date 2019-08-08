using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class DistrictCounty
    {
        public int DistCountyId { get; set; }
        public int? DistId { get; set; }
        public int? CountyId { get; set; }
        public string Comments { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserModified { get; set; }
    }
}
