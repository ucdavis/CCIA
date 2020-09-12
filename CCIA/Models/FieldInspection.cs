using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class FieldInspection
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public DateTime? DateInspected { get; set; }

        public string InspectorId { get; set; }
        public bool ApplicantContacted { get; set; }
    }
}
