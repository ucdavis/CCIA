using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class SeedsDocumentTypes
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; } 

        public string Folder { get; set; }            

    }
}