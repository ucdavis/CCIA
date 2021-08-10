using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class PotatoHealthCertificateHistory
    {

        public int Id { get; set; }       

        public int AppId { get; set; }

        public int ProductionYear { get; set; }

        public string Greenhouse { get; set; }

        public string Field { get; set; }

        public string CertNumber { get; set; }

        public string CertifyingState { get; set; }

    }
}
