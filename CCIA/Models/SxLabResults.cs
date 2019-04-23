using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class SxLabResults
    {
        public int SeedsId { get; set; }
        public DateTime? DateComplete { get; set; }

        [Display(Name = "Germination")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? GermPercent { get; set; }
        public int? GermDays { get; set; }
        public string GermResults { get; set; }
        public string GermTemp { get; set; }
        [Display(Name = "Inert")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? InertPercent { get; set; }
        public string InertComments { get; set; }
        [Display(Name = "Other Variety")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? OtherVarietyPercent { get; set; }
        public string OtherVarietyComments { get; set; }
        public short? OtherVarietyCount { get; set; }
        [Display(Name = "Other Crop")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? OtherCropPercent { get; set; }
        public string OtherCropComments { get; set; }
        public short? OtherCropCount { get; set; }
        [Display(Name = "Purity")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? PurityPercent { get; set; }
        public string PurityComments { get; set; }
        public string PurityResults { get; set; }
        public string SampleComments { get; set; }
        [Display(Name = "Weed Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? WeedSeedPercent { get; set; }
        public string WeedSeedComments { get; set; }
        public short? WeedSeedCount { get; set; }
        public decimal? NoxiousGrams { get; set; }
        public string NoxiousComments { get; set; }
        [Display(Name = "Noxious")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? NoxiousPercent { get; set; }
        public short? NoxiousCount { get; set; }
        public decimal? BushelWeight { get; set; }
        public decimal? PurityGrams { get; set; }
        public bool? CciaGerm { get; set; }
        [Display(Name = "Hard Seed")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? GermHardSeed { get; set; }
        public string AssayResults { get; set; }
        public bool? AssayTest { get; set; }
        public bool? AssayTest2 { get; set; }
        public string AssayResults2 { get; set; }
        public decimal? DodderGrams { get; set; }
        public decimal? LbsCanceled { get; set; }
        public decimal? LbsPassed { get; set; }
        public decimal? LbsRejected { get; set; }
        public DateTime? DataEntryDate { get; set; }
        public string DataEntryUser { get; set; }
        public DateTime? UpdateDate { get; set; }
        public string UpdateUser { get; set; }
        public DateTime? PrivateLabDate { get; set; }
        public string PrivateLabName { get; set; }
        public int? PrivateLabId { get; set; }
        public string PrivateLabNumber { get; set; }
        public bool? CciaConfirmed { get; set; }
        public DateTime? ConfirmDate { get; set; }
        public string ConfirmUser { get; set; }
        [Display(Name = "Badly Discolored")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? BadlyDiscoloredPercent { get; set; }
        [Display(Name = "Foreign Material")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? ForeignMaterialPercent { get; set; }
        public string ForeignMaterialsComments { get; set; }
        [Display(Name = "Splits&Cracks")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? SplitsAndCracksPercent { get; set; }
        [Display(Name = "Chewing Insect Damage")]
        [DisplayFormat(DataFormatString = "{0:P2}")]
        public decimal? ChewingInsectDamagePercent { get; set; }
    }
}
