using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class VarOfficial
    {
        public VarOfficial()
        {
            VarFamily = new HashSet<VarFamily>();
        }

        public int VarOffId { get; set; }
        public string VarOffName { get; set; }
        [Display(Name ="Category")]
        public string VarCategory { get; set; }
        [Display(Name="Status")]
        public string VarStatus { get; set; }
        public bool Experimental { get; set; }
        [Display(Name="Private Code")]
        public bool PrivateCode { get; set; }
        public bool OECD { get; set; }
        [Display(Name ="CCIA Certified?")]
        public bool CCIACertified { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name="CCIA Certified")]
        public DateTime? CCIACertifiedDate { get; set; }
        public string CCIACertifier { get; set; }
        [Display(Name ="Pending Certification?")]
        public bool PendingCertification { get; set; }
        [Display(Name ="Germplasm Entity")]
        public bool GermplasmEntity { get; set; }
        [Display(Name ="Desc. On File?")]
        public bool DescriptionOnFile { get; set; }
        [Display(Name ="Description Link")]
        public string DescHyperlink { get; set; }
        public int? CropId { get; set; }
        public bool Active { get; set; }
        
        [Display(Name ="Reason Inactive")]
        public string ReasonForInactive { get; set; }
        [Display(Name ="Historical Name")]
        public string HistoricalName { get; set; }
        public string Comments { get; set; }
        [Display(Name ="Brief Desc.")]
        public string BriefDescription { get; set; }
        public bool Confidential { get; set; }
        [Display(Name ="CTC Approved?")]
        public bool CtcApproved { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="CTC Approved")]
        public DateTime? CtcDateApproved { get; set; }
        
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Organizations OwnerOrganization { get; set; }
        public int? ProducerId { get; set; }
        [Display(Name ="Plant Patent?")]
        public bool PlantPatent { get; set; }
        [Display(Name ="Plant Patent #")]
        public int? PlantPatentNum { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Plant Patent Date")]
        public DateTime? PlantPatentDate { get; set; }
        [Display(Name ="PVP?")]
        public bool Pvp { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="PVP Date")]
        public DateTime? PvpDate { get; set; }
        [Display(Name ="PVP #")]
        public int? PvpNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="PVP Expiration")]
        public DateTime? PvpExpDate { get; set; }
        [Display(Name ="PVP Years")]
        public int? PvpYears { get; set; }
        [Display(Name ="Title V?")]
        public bool TitleV { get; set; }
        [Display(Name ="Variety Review Board?")]
        public bool VarReviewBoard { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        [Display(Name ="Var. Review Board Date")]
        public DateTime? VarReviewBoardDate { get; set; }
        [Display(Name ="Other State Cert?")]
        public string OtherStateCert { get; set; }
       
        [Display(Name ="Entered")]
        public DateTime? DateEntered { get; set; }
        [Display(Name ="Entered By")]
        public string UserEntered { get; set; }
        [Display(Name ="Updated")]
        public DateTime? DateUpdated { get; set; }
        [Display(Name ="Updated By")]
        public string UserUpdated { get; set; }
        [Display(Name ="Rice QA?")]
        public bool RiceQa { get; set; }
        [Display(Name ="Rice QA Color")]
        public string RiceQaColor { get; set; }
        [Display(Name ="Turfgrass")]
        public bool Turfgrass { get; set; }

        public Crops Crop { get; set; }
        public ICollection<VarFamily> VarFamily { get; set; }
        public int? Ecoregion { get; set; }
        [ForeignKey("Ecoregion")]
        public Ecoregions EcoRegionTranslate { get; set; }
        public string Elevation { get; set; }
        public int? HarvestCountyId { get; set; }
        [ForeignKey("HarvestCountyId")]
        public County HarvestCounty { get; set; }
        public int? StateHarvestedId { get; set; }
        [ForeignKey("StateHarvestedId")]
        public StateProvince StateHarvested { get; set; }


    }
}
