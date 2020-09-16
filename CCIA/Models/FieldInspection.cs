using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class FieldInspection
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public DateTime? DateInspected { get; set; }

        public string InspectorId { get; set; }
        public bool ApplicantContacted { get; set; }

        public bool ApplicantPresent { get; set; }

        public string Weeds { get; set; }

        public string Comments { get; set; }

        // PO Fields        
        public int? TotalPlantsInspected { get; set; }
        public int? OtherVarieties { get; set; }
        public int? Mosaic { get; set; }
        public int? Leafroll { get; set; }
        public int? Blackleg { get; set; }
        public int? Calico { get; set; }
        public int? OtherDiseases { get; set; }
        public string Insects { get; set; }

        //Seeds
        public string Maturity { get; set; }
        public string Isolation { get; set; }
        public string EstimatedYield { get; set; }
        public string OtherVarietiesComment { get; set; }
        public string OtherCrop { get; set; }
        public string Disease { get; set; }
        public string Appearance { get; set; }


        [ForeignKey("InspectorId")]
        public CCIAEmployees InspectorEmployee { get; set; }
    }
}
