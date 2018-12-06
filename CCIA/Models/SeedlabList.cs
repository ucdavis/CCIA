using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabList
    {
        public int ListId { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string Subspecies { get; set; }
        public string CommonName { get; set; }
        public bool? Noxious { get; set; }
        public string NoxiousType { get; set; }
        public string ListName { get; set; }
        public string ScientificName { get; set; }
    }
}
