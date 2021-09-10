using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Jobs
    {

        public int Id { get; set; }
        public string JobTitle { get; set; }

        public int JobInterval { get; set; }

        public TimeSpan JobTime { get; set; }

        public DateTime DateLastJobRan { get; set; }
        
        public DateTime DateNextJobStart { get; set; }

        public string Section { get; set; }


        
    }
}
