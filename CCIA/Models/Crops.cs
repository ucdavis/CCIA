using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class Crops
    {
        public Crops()
        {
            Applications = new HashSet<Applications>();
            VarOfficial = new HashSet<VarOfficial>();
        }

        public int CropId { get; set; }
        public string Genus { get; set; }
        public string Species { get; set; }
        public string Crop { get; set; }
        public string CropKind { get; set; }
        public int? CropRenewYears { get; set; }
        public int? AppDue { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public string Annual { get; set; }
        public string QbClass { get; set; }
        public string QbInvitem { get; set; }
        public string ReportGroup { get; set; }
        public bool? CertifiedCrop { get; set; }
        public bool? Heritage { get; set; }
        public bool? PreVarietyGermplasm { get; set; }
        public bool? Fov4Map { get; set; }
        public bool? IsolationCrop { get; set; }
        public bool? IdahoVegetable { get; set; }
        public string IdahoFieldType { get; set; }
        public string IdahoCropName { get; set; }
        public decimal? IsolationFoundation { get; set; }
        public decimal? IsolationRegistered { get; set; }
        public decimal? IsolationCertified { get; set; }
        public decimal? IsolationHybrid { get; set; }
        public decimal? IsolationParentA { get; set; }
        public decimal? IsolationParentB { get; set; }
        public decimal? IsolationParentR { get; set; }

        public string GenusSpecies 
        { 
            get
            {
                return $"{Genus} {Species}";

            }
        }

        [StringLength(256)]
        [Display(Name = "Crop")]
        public string Name => CropKind == null ? Crop : CropKind + " " + Crop;


        public ICollection<Applications> Applications { get; set; }
        public ICollection<VarOfficial> VarOfficial { get; set; }

        public ICollection<CropStandards> CropStandards { get; set; }
        
    }
}
