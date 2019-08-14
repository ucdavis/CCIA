using System.Collections.Generic;

namespace CCIA.Models
{
    public class GrowerInfo
    {
        public List<Organizations> Orgs { get; set; }

        public string ActionType { get; set; }

        public int AppTypeId { get; set; }
    }
}