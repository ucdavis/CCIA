using System;
using System.Collections.Generic;

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
        public bool? Oecd { get; set; }
        public bool? CciaCertified { get; set; }
        public DateTime? CciaCertifiedDate { get; set; }
        public string CciaCertifier { get; set; }
        public bool? PendingCertification { get; set; }
        public bool? GermplasmEntity { get; set; }
        public bool? DescriptionOnFile { get; set; }
        public string DescHyperlink { get; set; }
        public int? CropId { get; set; }
        public bool? Active { get; set; }
        public string ReasonForInactive { get; set; }
        public string HistoricalName { get; set; }
        public string Comments { get; set; }
        public string BriefDescription { get; set; }
        public bool? Confidential { get; set; }
        public bool? CtcApproved { get; set; }
        public DateTime? CtcDateApproved { get; set; }
        public string GenBreeder { get; set; }
        public string GenCertified { get; set; }
        public string GenFoundation { get; set; }
        public string GenRegistered { get; set; }
        public int? OwnerId { get; set; }
        public int? ProducerId { get; set; }
        public bool? PlantPatent { get; set; }
        public int? PlantPatentNum { get; set; }
        public DateTime? PlantPatentDate { get; set; }
        public bool? Pvp { get; set; }
        public DateTime? PvpDate { get; set; }
        public int? PvpNumber { get; set; }
        public DateTime? PvpExpDate { get; set; }
        public int? PvpYears { get; set; }
        public bool? TitleV { get; set; }
        public bool? VarReviewBoard { get; set; }
        public DateTime? VarReviewBoardDate { get; set; }
        public string OtherStateCert { get; set; }
        public string Generation { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserUpdated { get; set; }
        public bool? RiceQa { get; set; }
        public string RiceQaColor { get; set; }
        public bool? Turfgrass { get; set; }

        public Crops Crop { get; set; }
        public ICollection<VarFamily> VarFamily { get; set; }
    }
}
