using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class NativeSeedSites
    {
        [Key]
        public int Id { get; set; }
        public int appID { get; set; }

        public string SiteName { get; set; }
        public string OwnershipOrSize { get; set; }
        public DateTime HarvestDate { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }
        public string Comments { get; set; }
        public DateTime DateInspected { get; set; }
        public bool Approved { get; set; }
        public string ApprovedBy { get; set; }
        public string InspectionComment { get; set; }
        public decimal? InspectionFee { get; set; }

    }
}