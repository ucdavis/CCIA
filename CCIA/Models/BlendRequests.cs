using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BlendRequests
    {
        public int BlendId { get; set; }
        public string BlendType { get; set; }
        public DateTime RequestStarted { get; set; }
        public int ConditionerId { get; set; }
        public int UserEntered { get; set; }
        public decimal? LbsLot { get; set; }
        public int? Class { get; set; }
        public string Status { get; set; }
        public int? TagCountRequested { get; set; }
        public int? TagType { get; set; }
        public int? Variety { get; set; }
        public DateTime? DateNeeded { get; set; }
        public string HowDeliver { get; set; }
        public string DeliveryAddress { get; set; }
        public string Comments { get; set; }
        public DateTime? DateSubmitted { get; set; }
        public bool? Submitted { get; set; }
        public bool? Approved { get; set; }
        public DateTime? ApproveDate { get; set; }
        public string ApprovedBy { get; set; }

        [ForeignKey("BlendId")]
        public LotBlends LotBlend { get; set; }
    }
}
