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
        public string VarCategory { get; set; }
        public string VarStatus { get; set; }
        public bool? Experimental { get; set; }
        public bool? PrivateCode { get; set; }
        public bool? OECD { get; set; }
        public bool? CCIACertified { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? CCIACertifiedDate { get; set; }
        public string CCIACertifier { get; set; }
        public bool? PendingCertification { get; set; }
        public bool? GermplasmEntity { get; set; }
        public bool? DescriptionOnFile { get; set; }
        public string DescHyperlink { get; set; }
        public int? CropId { get; set; }
        public bool? Active { get; set; }
        // TODO: delete this property?
        public string ReasonForInactive { get; set; }
        public string HistoricalName { get; set; }
        public string Comments { get; set; }
        public string BriefDescription { get; set; }
        public bool? Confidential { get; set; }
        public bool? CtcApproved { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? CtcDateApproved { get; set; }
        
        public int? OwnerId { get; set; }
        [ForeignKey("OwnerId")]
        public Organizations OwnerOrganization { get; set; }
        public int? ProducerId { get; set; }
        public bool? PlantPatent { get; set; }
        public int? PlantPatentNum { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? PlantPatentDate { get; set; }
        public bool? Pvp { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? PvpDate { get; set; }
        public int? PvpNumber { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? PvpExpDate { get; set; }
        public int? PvpYears { get; set; }
        public bool? TitleV { get; set; }
        public bool? VarReviewBoard { get; set; }
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? VarReviewBoardDate { get; set; }
        public string OtherStateCert { get; set; }
       
        public DateTime? DateEntered { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserUpdated { get; set; }
        public bool? RiceQa { get; set; }
        public string RiceQaColor { get; set; }
        public bool? Turfgrass { get; set; }

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
