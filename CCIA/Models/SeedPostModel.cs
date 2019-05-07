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
    public int? AcresApplied { get; set; }

    public string AdditionalInfo { get; set; }

    [Required]
    public string CertLotNum { get; set; }
    public string CertLotNum2 { get; set; }

    [Required]
    public int? ClassPlanted { get; set; }

    public int? ClassPlanted2 { get; set; }

    [Required]
    public int? ClassProduced { get; set; }

    [Required]
    public short? County { get; set; }

    [Required]
    public int? Crop { get; set; }

    [Required]
    public short? CropYear { get; set; }

    [Required]
    public DateTime? DatePlanted { get; set; }
    
    public string EnteredVarietyName {get; set; }

    public List<FieldHistory> FieldHistories{ get; set; }

    public int GrowerId { get; set; }

    [Required]
    public string NameOrNum { get; set; }

    public PlantingStocks PlantingStock1 { get; set; }
    public PlantingStocks PlantingStock2 { get; set; }

    [Required]
    public int? PoundsPlanted { get; set; }

    public int? PoundsPlanted2 { get; set; }
    public string SeedFrom { get; set; }
    [Required]
    public int? StateCountryStockGrown { get; set; }

    public int? StateCountryStockGrown2 { get; set; }
    
    [Required]
    public int? StateCountryTagIssued { get; set; }

    public int? StateCountryTagIssued2 { get; set; }

    [Required]
    public string Variety { get; set; }

    public string Variety2 { get; set; }

    public int? VarietyId { get; set; }
    }
}
