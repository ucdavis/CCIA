using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SxLabResults
    {
        public int SeedsId { get; set; }
        public DateTime? DateComplete { get; set; }
        public decimal? GermPercent { get; set; }
        public int? GermDays { get; set; }
        public string GermResults { get; set; }
        public string GermTemp { get; set; }
        public decimal? InertPercent { get; set; }
        public string InertComments { get; set; }
        public decimal? OtherVarietyPercent { get; set; }
        public string OtherVarietyComments { get; set; }
        public short? OtherVarietyCount { get; set; }
        public decimal? OtherCropPercent { get; set; }
        public string OtherCropComments { get; set; }
        public short? OtherCropCount { get; set; }
        public decimal? PurityPercent { get; set; }
        public string PurityComments { get; set; }
        public string PurityResults { get; set; }
        public string SampleComments { get; set; }
        public decimal? WeedSeedPercent { get; set; }
        public string WeedSeedComments { get; set; }
        public short? WeedSeedCount { get; set; }
        public decimal? NoxiousGrams { get; set; }
        public string NoxiousComments { get; set; }
        public decimal? NoxiousPercent { get; set; }
        public short? NoxiousCount { get; set; }
        public decimal? BushelWeight { get; set; }
        public decimal? PurityGrams { get; set; }
        public bool? CciaGerm { get; set; }
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
        public decimal? BadlyDiscoloredPercent { get; set; }
        public decimal? ForeignMaterialPercent { get; set; }
        public string ForeignMaterialsComments { get; set; }
        public decimal? SplitsAndCracksPercent { get; set; }
        public decimal? ChewingInsectDamagePercent { get; set; }
    }
}
