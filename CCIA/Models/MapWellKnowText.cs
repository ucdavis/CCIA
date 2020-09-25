using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CCIA.Models
{
    public class MapWKT
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string GeoField { get; set; }
        public string Icon { get; set; }
        public string Link { get; set; }

        public string GeoType { get; set; }
        public int AppId { get; set; }
        
    }
}
