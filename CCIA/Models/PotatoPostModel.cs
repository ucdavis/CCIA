using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class PotatoPostModel
    {
        public PotatoPostModel()
        {
        }

    [Required]
    public int? AcresApplied { get; set; }

    public string AdditionalInfo { get; set; }

    [Required]
    public string CertLotNum { get; set; }

    [Required]
    public int? ClassPlanted { get; set; }

    [Required]
    public int? ClassProduced { get; set; }

    [Required]
    public int? County { get; set; }

    [Required]
    public int? Crop { get; set; }

    [Required]
    public int CropYear { get; set; }

    [Required]
    public DateTime? DatePlanted { get; set; }
    
    public string EnteredVarietyName {get; set; }

    public ICollection<FieldHistory> FieldHistories{ get; set; }

    public int GrowerId { get; set; }

    public string HistoryApplicationNum1 { get; set; }
    public string HistoryApplicationNum2 { get; set; }
    public string HistoryApplicationNum3 { get; set; }
    public int? HistoryCrop1 { get; set; }
    public int? HistoryCrop2 { get; set; }
    public int? HistoryCrop3 { get; set; }
    public string HistoryVarietyCrop1 { get; set; }
    public string HistoryVarietyCrop2 { get; set; }
    public string HistoryVarietyCrop3 { get; set; }
    public int? HistoryCropYear1 { get; set; }
    public int? HistoryCropYear2 { get; set; }
    public int? HistoryCropYear3 { get; set; }

    [Required]
    public string NameOrNum { get; set; }

    public string SeedFrom { get; set; }
    [Required]
    public int? StateCountryStockGrown { get; set; }

    [Required]
    public int? StateCountryTagIssued { get; set; }

    [Required]
    public string Variety { get; set; }

    public int? VarietyId { get; set; }

    [Required]
    public bool WinterTestResultsRequired {get; set; }

    [Required]
    public bool PVXTested {get; set; }
    }
}