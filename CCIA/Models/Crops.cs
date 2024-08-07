﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Crops
    {
        
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
        public bool CertifiedCrop { get; set; }
        public bool Heritage { get; set; }
        public bool PreVarietyGermplasm { get; set; }
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
        public bool LacTracker { get; set; }

        public int maxOECDLotWeight { get; set; }

        public string GenusSpecies 
        { 
            get
            {
                return $"{Genus} {Species}";

            }
        }

        public string NativeSeedName
        {
            get
            {
                return $"{Genus} {Species} ({Crop})";

            }
        }

        [StringLength(256)]
        [Display(Name = "Crop")]
        public string Name
        {
            get
            {
                if(PreVarietyGermplasm)
                {
                    return $"{Genus} {Species} ({Crop})";
                } else
                {
                    if(CropKind == null)
                    {
                        return Crop;
                    } else
                    {
                        return $"{CropKind} {Crop}";
                    }
                }
            }
        } 

        
        [Display(Name = "Crop and Kind")]
        public string CropsAndKind => CropKind == null ? Crop : Crop + ", " + CropKind;


        
        public ICollection<VarOfficial> VarOfficial { get; set; }

        public ICollection<CropStandards> CropStandards { get; set; }

        [ForeignKey("CropId")]
        public ICollection<Subspecies> Subspecies { get; set; }

    }
}
