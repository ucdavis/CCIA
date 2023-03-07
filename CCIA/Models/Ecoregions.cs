using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Ecoregions
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Display
        {
            get {
                return $"{Id} {Name}";
            }
        }
    }
}
