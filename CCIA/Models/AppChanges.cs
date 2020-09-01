using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Models
{
    public partial class AppChanges
    { 
        public int Id { get; set; }       
        public int AppId { get; set; }
        public string ColumnChange { get; set; }
        public string OldValue { get; set; }
        public string NewValue { get; set; }
        public string UserChange { get; set; }
        public DateTime DateChanged { get; set; }

        //TODO add employees

    }
}