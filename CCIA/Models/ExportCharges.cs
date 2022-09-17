using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class ExportCharges
    {
        public string Type { get; set; }
        public int Link_Id { get; set; }
        public string TRNS { get; set; }
        public string TrnsType { get; set; }
        public DateTime Date { get; set; }
        public string Accnt { get; set; }
        public string Name { get; set; }
        public string QBClass { get; set; }
        public decimal? Amount { get; set; }
        public string DocNum { get; set; }
        public string Memo { get; set; }
        public string InvItem { get; set; }

    }
}
