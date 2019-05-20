// using System;
// using System.Collections.Generic;

// namespace CCIA.Models
// {
//     public partial class SeedPostModel
//     {
//         public SeedPostModel(){}

//         public Applications Application { get; set; }

//         [Required]
//         public short? County { get; set; }

//         [Required]
//         public DateTime? DatePlanted { get; set; }

//         public ICollection<FieldHistory> FieldHistories{ get; set; }

//         public int GrowerId { get; set; }

//         [Required]
//         public string NameOrNum { get; set; }

//         [Required]
//         public PlantingStocks PlantingStock1 { get; set; }
//         public PlantingStocks PlantingStock2 { get; set; }

//         [Required]
//         public int? PoundsPlanted { get; set; }

//         public int? PoundsPlanted2 { get; set; }
//     }
// }
