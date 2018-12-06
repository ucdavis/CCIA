using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class Certs
    {
        public int CertNum { get; set; }
        public int OfficialVarietyId { get; set; }
        public int OrgId { get; set; }
        public int ClassProduced { get; set; }
        public string ClassProducedChar { get; set; }
        public short? CertYear { get; set; }
        public DateTime? DateEntered { get; set; }
        public DateTime? DateModified { get; set; }
    }
}
