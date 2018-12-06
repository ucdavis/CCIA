using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class ChangeRequests
    {
        public int ChangeId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int? Priority { get; set; }
        public DateTime? SubmitDate { get; set; }
        public bool? Completed { get; set; }
        public DateTime? CompletedDate { get; set; }
    }
}
