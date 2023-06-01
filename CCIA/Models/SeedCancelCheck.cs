using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedCancelCheck
    {
        [Key]
        public int Id { get; set; }        

        public string Source { get; set; }

      

    }
}