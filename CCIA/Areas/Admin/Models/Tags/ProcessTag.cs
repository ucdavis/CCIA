using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
     public partial class ProcessTag
    {
        public int Id { get; set; }

        public int? LinkId { get; set; }

        public string IDType { get; set; }

        public DateTime DateRequested { get; set; }

        public string Crop { get; set; }

        public string Variety { get; set; }

        public string SeedClass { get; set; }

        public string LotNumber { get; set; }

        public string TagClass { get; set; }

        [Column(TypeName ="numeric(34,12)")]
        public decimal? Pounds { get; set; }

        public string TagType { get; set; }

        public int OrgId { get; set; }

        public string OrgName { get; set; }

        public int FileCount { get; set; }
        public int SeriesRows { get; set; }

        public int SeriesCount { get; set; }
    }
}