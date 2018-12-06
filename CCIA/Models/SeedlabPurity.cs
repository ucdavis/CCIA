using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class SeedlabPurity
    {
        public int LabId { get; set; }
        public double? WeightWorkingSample { get; set; }
        public double? OtherCropGrams { get; set; }
        public short? OtherCropCount { get; set; }
        public double? OtherVarietyGrams { get; set; }
        public short? OtherVarietyCount { get; set; }
        public double? InertGrams { get; set; }
        public string InertDescription { get; set; }
        public bool? InertSoil { get; set; }
        public bool? InertDirt { get; set; }
        public bool? InertSeedFragments { get; set; }
        public bool? InertChaff { get; set; }
        public bool? InertPlantFragments { get; set; }
        public bool? InertStems { get; set; }
        public bool? InertTreatmentColorant { get; set; }
        public double? WeedSeedGrams { get; set; }
        public short? WeedSeedCount { get; set; }
        public double? PureSeedGrams { get; set; }
        public decimal? ReportPurity { get; set; }
        public decimal? ReportOtherCrop { get; set; }
        public decimal? ReportInert { get; set; }
        public decimal? ReportWeed { get; set; }
        public decimal? ReportOtherVariety { get; set; }
        public double? BushelWeight { get; set; }
        public DateTime? DateCompleted { get; set; }
        public string CompletedBy { get; set; }
        public string Comments { get; set; }
        public bool? InsufficientSizePurity { get; set; }
        public decimal? CalcPurity { get; set; }
        public decimal? CalcOtherCrop { get; set; }
        public decimal? CalcInert { get; set; }
        public decimal? CalcWeed { get; set; }
        public double? ForeignMaterialGrams { get; set; }
        public double? SplitsCracksGrams { get; set; }
        public double? BadlyDiscoloredGrams { get; set; }
        public double? ChewingInsectDamageGrams { get; set; }
        public decimal? CalcForeignMaterial { get; set; }
        public decimal? CalcSplitsAndCracks { get; set; }
        public decimal? CalcBadlyDiscolored { get; set; }
        public decimal? CalcChewingInsectDamage { get; set; }
        public decimal? ReportForeignMaterial { get; set; }
        public decimal? ReportSplitsCracks { get; set; }
        public decimal? ReportBadlyDiscolored { get; set; }
        public decimal? ReportChewingInsectDamage { get; set; }
    }
}
