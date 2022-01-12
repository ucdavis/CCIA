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
        [Display(Name="Pretag?")]
        public bool AllowPretag { get; set; }
        [Display(Name="Print Series?")]
        public bool PrintSeries { get; set; }
        [Display(Name="Request CCIA Print Series?")]
        public bool RequestCciaPrintSeries { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name="Pretag Approved")]
        public DateTime? DatePretagApproved { get; set; }
    }
}
