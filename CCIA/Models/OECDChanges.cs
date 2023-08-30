using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.CodeAnalysis.Operations;

namespace CCIA.Models
{
    public partial class OECDChanges
    { 
        public int Id { get; set; }       
        public int OECDId { get; set; }
        public string ColumnChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserChange { get; set; }
        public int? ContactChange { get; set; }
        public DateTime DateChanged { get; set; }

        [ForeignKey("UserChange")]
        public CCIAEmployees Employee { get; set; }

		[ForeignKey("ContactChange")]
		public Contacts Contact { get; set; }
    }
}