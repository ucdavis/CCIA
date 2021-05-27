using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BulkSalesCertificateChanges
    { 
        public int Id { get; set; }       
        public int BSCId { get; set; }
        public string ColumnChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserChange { get; set; }
        public DateTime DateChanged { get; set; }

        [ForeignKey("UserChange")]
        public CCIAEmployees Employee { get; set; }
    }
}