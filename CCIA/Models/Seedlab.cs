using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Seedlab
    {
        public int LabId { get; set; }
        public DateTime DateReceived { get; set; }
        public double? Weight { get; set; }
        public bool? InsufficientSizeSample { get; set; }
        public int? SeedsId { get; set; }
        public string SampleType { get; set; }
        public string Condition { get; set; }
        public string Comments { get; set; }
        public string RecordedBy { get; set; }
        public string AppEnteredBy { get; set; }
        public DateTime AppEnteredDate { get; set; }
        public string AppUpdatedBy { get; set; }
        public DateTime? AppUpdatedDate { get; set; }
        public short LabYear { get; set; }
        public bool? HasSampleForm { get; set; }
        public bool? ClearlyMarkedForCert { get; set; }
        public DateTime? DateDivided { get; set; }
        public string DividedBy { get; set; }
    }
}
