using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedDocuments
    {
        [Key]
        public int Id { get; set; }
        public int SeedsId { get; set; } 

        public string Name { get; set; }

        public string Link { get; set; }

        public int DocType { get; set; }

        [ForeignKey("DocType")]
        public SeedsDocumentTypes DocumentType { get; set; }
      

    }
}