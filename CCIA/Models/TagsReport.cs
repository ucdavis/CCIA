using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class TagsReport
    {
        public string OrgName { get; set; }
        public int TagId { get; set; }
        public int SID { get; set; }
        public DateTime? DateRequested { get; set; }
        public DateTime? DateApproved { get; set; }
        public string TagStage { get; set; }
        public string SidStatus { get; set; }

    }
}
