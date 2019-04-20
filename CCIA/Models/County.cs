using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class County
    {
        public short CountyId { get; set; }
        public string CountyName { get; set; }
        public string District { get; set; }
        public short StateProvinceId { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
        public string Fips { get; set; }
        public int? AgCommOrg { get; set; }

        
    }
}
