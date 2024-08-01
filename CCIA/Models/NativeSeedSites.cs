using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class NativeSeedSites
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
        public string SiteName { get; set; }
        public int CollectionAreaSize { get; set; }
        [DataType(DataType.Date)]
        public DateTime HarvestDate { get; set; }
        public decimal? Lat { get; set; }
        public decimal? Long { get; set; }
        public int? FieldElevation { get; set; }
        public int SiteCounty { get; set; }
        [ForeignKey("SiteCounty")]
        public County County { get; set; }        
        public string Comments { get; set; }
        


        public NativeSeedSites()
        {
           SiteCounty = 102;
            HarvestDate = DateTime.Now.AddDays(14).Date;
        }

    }
}