using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class AbbrevTagType
    {
        public int Id { get; set; }
        
        public string TagTypeTrans { get; set; }
        public int SortOrder { get; set; }

        public bool StandardTagForm { get; set; }

        public bool OECD { get; set; }

        public bool PotatoTag { get; set; }
        
    }
}
