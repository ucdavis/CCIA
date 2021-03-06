﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class County
    {
        public int CountyId { get; set; }
        
        [Display(Name="County")]
        public string Name { get; set; }
        public string District { get; set; }
        public int StateProvinceId { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }
        public string Fips { get; set; }
        public int? AgCommOrg { get; set; }

        
    }
}
