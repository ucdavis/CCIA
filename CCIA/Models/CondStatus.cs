using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class CondStatus
    {
        public int Id { get; set; }
        public int OrgId { get; set; }
        public int Year { get; set; }
        public string Status { get; set; }
        public DateTime DateUpdated { get; set; }
        public bool AllowPretag { get; set; }
        public bool PrintSeries { get; set; }
        public bool RequestCciaPrintSeries { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? DatePretagApproved { get; set; }
    }
}
