using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SampleLabResultsChanges : LabResultChanges
    {
        public int SID { get; set; }
        
        [ForeignKey("UserChange")]
        public CCIAEmployees Employee { get; set; }

    }

    public partial class BlendLabResultsChanges : LabResultChanges
    {
        public int BlendId { get; set; }

        public int? UpdateContactId { get; set; }
        public string UpdateAdminId { get; set; }
        public bool LastUpdateAdmin { get; set; }

        [ForeignKey("UpdateAdminId")]
        public CCIAEmployees Employee { get; set; }

        [ForeignKey("UpdateContactId")]
        public Contacts Contact { get; set; }
    }

    public partial class LabResultChanges
    { 
        public int Id { get; set; }       
        
        public string ColumnChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserChange { get; set; }
        public DateTime DateChanged { get; set; }

       
        
    }
}