using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class TagDocuments
    {
        [Key]
        public int Id { get; set; }
        public int TagId { get; set; } 
       
        public string Link { get; set; }

    }
}