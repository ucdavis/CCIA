using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CCIA.Models
{
    public partial class Certs
    {
        public int CertNum { get; set; }
        public int OfficialVarietyId { get; set; }
        public int OrgId { get; set; }
        public int ClassProduced { get; set; }
        public string ClassProducedChar { get; set; }
        public int? CertYear { get; set; }
        public DateTime? DateEntered { get; set; }
        public DateTime? DateModified { get; set; }
        
        [ForeignKey("OfficialVarietyId")]
        public VarFull Variety { get; set; }

        [ForeignKey("OrgId")]
        public Organizations ApplicantOrganization { get; set; }

        [ForeignKey("ClassProduced")]
        public AbbrevClassProduced Class { get; set; }
    }
}
