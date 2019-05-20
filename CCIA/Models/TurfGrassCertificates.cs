using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public class TurfgrassCertificates
    {
        public int Id { get; set; }

        public int AppId { get; set; }

       

        public int? Sprigs { get; set; }

        public int? Sod { get; set; }

        public string BillingInvoice { get; set; }

        public DateTime HarvestDate { get; set; }

        public int HarvestNumber { get; set; }

       
    }
}