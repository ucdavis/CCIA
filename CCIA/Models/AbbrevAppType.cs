using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class AbbrevAppType
    {
        public int AppTypeId { get; set; }
        [Display(Name="App Type")]
        public string AppTypeTrans { get; set; }
        public string Abbreviation { get; set; }
        public bool GrowerSameAsApplicant { get; set; }
        public string CertificateTitle { get; set; }
        public string NumberTitle { get; set; }
        public string SirTitle { get; set; }
        public string ProcessTitle { get; set; }
        public string SpeciesOrCrop { get; set; }
        public string ProducedTitle { get; set; }
        public string VarietyTitle { get; set; }
        public bool? ShowType { get; set; }

        public bool QAProgram { get; set; }
        public int FieldHistoryCount { get; set; }

        public bool showSecondPlantingStock { get; set; }
        public string VarietyLabel { get; set; }
        public string PlantingStockCertNumberLabel { get; set; }

        public bool ShowPSTagIssued { get; set; }

        public ICollection<BulkSalesCertificates> BulkSalesCertificate { get; set; }
        public ICollection<Applications> Application { get; set; }
        public ICollection<Seeds> Seed { get; set; }

       
        
       
        
    }
}
