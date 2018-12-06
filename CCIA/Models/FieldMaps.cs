using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class FieldMaps
    {
        public int MapptId { get; set; }
        public int AppId { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
    }
}
