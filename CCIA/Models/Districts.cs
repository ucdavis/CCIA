using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Districts
    {
        public short DistId { get; set; }
        public string DistCode { get; set; }
        public string DistName { get; set; }
        public int? DistLeader { get; set; }
        public int? DistOffice { get; set; }
        public string Comments { get; set; }
    }
}
