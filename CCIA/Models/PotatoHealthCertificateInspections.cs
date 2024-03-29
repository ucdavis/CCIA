using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class PotatoHealthCertificateInspections
    {

        public string InspectionNumberText { get; set; }       

        [Column(TypeName ="decimal(18,1)")]
        public decimal RowNumber { get; set; }

        public string LeafrollPercent { get; set; }

        public string MosaicPercent { get; set; }

        public string OtherVarietyPercent { get; set; }

        public string BlacklegPercent { get; set; }

        public string OtherDiseasePercent { get; set; }

    }
}
