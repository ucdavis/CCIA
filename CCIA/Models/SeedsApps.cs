using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedsApplications
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
        public int SeedsId { get; set; } 

        [ForeignKey("SeedsId")]
        public Seeds Seeds { get; set; } 

        [ForeignKey("AppId")]
        public Applications Application { get; set; }      

    }
}