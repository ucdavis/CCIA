using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class BulkSalesCertificatesShares
    {
        public int Id { get; set; }
        public int  BulkSalesCertificatesId { get; set; }
        public int ShareOrganizationId { get; set; }

        [ForeignKey("ShareOrganizationId")]
        public Organizations ShareOrganization { get; set; }
    }
}
