using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BlendDocuments
    {
        public int Id { get; set; }
        public int BlendId { get; set; } 

        public string Name { get; set; }

        public string Link { get; set; }

    }
}