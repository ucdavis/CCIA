﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class FieldInspection
    {
        public FieldInspection() {}
        public FieldInspection(int appId)
        {
            AppId = appId;
        }

        public int Id { get; set; }
        public int AppId { get; set; }
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name="Date")]
        public DateTime? DateInspected { get; set; }

        [Display(Name="Inspector")]
        public string InspectorId { get; set; }
        [Display(Name="Applicant Contacted?")]
        public bool ApplicantContacted { get; set; }

        [Display(Name="Applicant Present")]
        public bool ApplicantPresent { get; set; }

        public string Weeds { get; set; }
        public string WeedsComments { get; set; }

        public string Comments { get; set; }

        // PO Fields        
        [Display(Name="Total Plants Inspected")]
        public int? TotalPlantsInspected { get; set; }
        [Display(Name="Other Varieties")]
        public int? OtherVarieties { get; set; }
        public int? Mosaic { get; set; }
        public int? Leafroll { get; set; }
        public int? Blackleg { get; set; }
        public int? Calico { get; set; }
        [Display(Name="Other Diseases")]
        public int? OtherDiseases { get; set; }
        public string Insects { get; set; }

        //Seeds
        public string Maturity { get; set; }
        public string Isolation { get; set; }
        [Display(Name="Estimated yield")]
        public string EstimatedYield { get; set; }
        [Display(Name="Other Varieties Comments")]
        public string OtherVarietiesComment { get; set; }
        [Display(Name="Other crop")]
        public string OtherCrop { get; set; }
        public string Disease { get; set; }
        public string Appearance { get; set; }

       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double OtherVarietiesPercent { 
            get {
                if(OtherVarieties.HasValue)
                {
                    return ((double)OtherVarieties.Value / TotalPlantsInspected.Value) *100;
                }
                return 0;                
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double MosaicPercent { 
            get {
                if(Mosaic.HasValue)
                {
                    return ((double)Mosaic.Value / TotalPlantsInspected.Value) *100;
                }
                return 0; 
                
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double LeafrollPercent { 
            get {
                if(Leafroll.HasValue)
                {
                    return ((double)Leafroll.Value / TotalPlantsInspected.Value) *100;
                }
                return 0; 
               
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double BlacklegPercent { 
            get {
                if(Blackleg.HasValue)
                {
                    return ((double)Blackleg.Value / TotalPlantsInspected.Value) *100;
                }
                return 0; 
                
            }
        }
       
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double CalicoPercent { 
            get {
                if(Calico.HasValue)
                {
                    return ((double)Calico.Value / (double)TotalPlantsInspected.Value) *100;
                }
                return 0; 
                
            }
        }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:0.00;0.00;0}")]
        public double OtherDiseasesPercent { 
            get {
                if(OtherDiseases.HasValue)
                {
                    return ((double)OtherDiseases.Value / TotalPlantsInspected.Value) *100;
                }
                return 0; 
                
            }
        }


        [ForeignKey("InspectorId")]
        public CCIAEmployees InspectorEmployee { get; set; }
    }
}
