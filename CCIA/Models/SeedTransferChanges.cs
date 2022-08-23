using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedTransferChanges
    { 
        public int Id { get; set; }       
        public int STId { get; set; }
        public string ColumnChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserChange { get; set; }
        public DateTime DateChanged { get; set; }
        public bool userIsAdmin { get; set; }

        public int? ContactId { get; set; }

        [ForeignKey("UserChange")]
        public CCIAEmployees Employee { get; set; }

        [ForeignKey("ContactId")]
        public Contacts UpdateContact { get; set; }
        
    }
}