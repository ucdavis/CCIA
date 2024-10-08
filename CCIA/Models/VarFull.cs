﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public enum VarietyTypes
    { 
        
        [Display(Name ="Official")]
        official,        
        [Display(Name ="OECD")]
        OECD,
        [Display(Name ="Experimental")]
        Experimental,
        [Display(Name ="Alias")]
        Alias
    } 

    public class VarFull
    {
        public int Id { get; set; }        
        [Display(Name="Variety")]
        [Required]
        public string Name { get; set; }

         public string NameAndId => Id + " " + Name;

        public string Type { get; set; }

        [ForeignKey("CropId")]
        public Crops Crop { get; set; }
        public int CropId { get; set; }
        [Display(Name ="Category")]
        public string Category { get; set; }
        public string Status { get; set; }
        public bool Certified { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Certified")]
        public DateTime? DateCertified { get; set; }

        public string TableName { get; set; }
       
       
        [Display(Name ="Rice QA")]
        public bool RiceQa { get; set; }
        [Display(Name ="Rice Color")]
        public string RiceColor { get; set; }
        public int ParentId { get; set; }

        [ForeignKey("ParentId")]
        public VarOfficial VarietyOfficial { get; set; }
        [ForeignKey("Id")]
        public VarFamily VarietyFamily { get; set; }
        public bool Turfgrass { get; set; }
        public bool Blend { get; set; }

        public int EcoregionId { get; set; }
        [ForeignKey("EcoregionId")]
        public Ecoregions Ecoregion { get; set; }

        public int CountyHarvestId { get; set; }
        [ForeignKey("CountyHarvestId")]
        public County CountyHarvested { get; set; }

        public string Elevation { get; set; }

        [Display(Name="Permitted number of generations")]
        public int? NumberOfGenerationsPermitted { get; set; }

        [Display(Name ="G0 Collection Info")]
        public string G0CollectionInfo
        {
            get
            {
                if(EcoregionId != 0 && Ecoregion != null && CountyHarvestId != 0 && CountyHarvested != null)
                {
                    return $"{EcoregionId.ToString()}-{Ecoregion.Name}, {Elevation} {CountyHarvested.Name}";
                }
                return "";

            }
        }

        [ForeignKey("VarId")]
        public ICollection<VarCountries> Countries { get; set; }

        public int subspeciesId { get; set; }

        [ForeignKey("SubspeciesId")]
        public Subspecies Subspecies { get; set; }








    }
}
