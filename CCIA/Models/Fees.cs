using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Fees
    {
        public int FeeId { get; set; }
        public int? ItemId { get; set; }
        public string Item { get; set; }
        public decimal FeeAmount { get; set; }
        public decimal? Unit { get; set; }
        public string LinkType { get; set; }
        public string FeeCategory { get; set; }
        public bool? Active { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
