using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class SeedPostModel
    {
        public SeedPostModel()
        {
        }

    [Required]
    public int AcresApplied { get; set; }

    public string AdditionalInfo { get; set; }
    public int CertLotNum { get; set; }
    public int? CertLotNum2 { get; set; }

    [Required]
    public string ClassPlanted { get; set; }

    public string ClassPlanted2 { get; set; }

    [Required]
    public string ClassProduced { get; set; }

    [Required]
    public string County { get; set; }

    [Required]
    public string Crop { get; set; }

    [Required]
    public int CropYear { get; set; }

    [Required]
    public DateTime DatePlanted { get; set; }

    public int? HistoryApplicationNum1 { get; set; }
    public int? HistoryApplicationNum2 { get; set; }
    public int? HistoryApplicationNum3 { get; set; }
    public string HistoryCrop1 { get; set; }
    public string HistoryCrop2 { get; set; }
    public string HistoryCrop3 { get; set; }
    public string HistoryVarietyCrop1 { get; set; }
    public string HistoryVarietyCrop2 { get; set; }
    public string HistoryVarietyCrop3 { get; set; }
    public int HistoryYear1 { get; set; }
    public int HistoryYear2 { get; set; }
    public int HistoryYear3 { get; set; }

    [Required]
    public string NameOrNum { get; set; }

    [Required]
    public int PoundsPlanted { get; set; }

    public int? PoundsPlanted2 { get; set; }
    public string SeedFrom { get; set; }
    public string StateCountyStockGrown { get; set; }
    public string StateCountyStockGrown2 { get; set; }
    public string StateCountyTagIssued { get; set; }
    public string StateCountyTagIssued2 { get; set; }

    [Required]
    public string Variety { get; set; }

    public string Variety2 { get; set; }
    }
}
