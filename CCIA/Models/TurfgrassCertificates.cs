using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class TurfgrassCertificates
    {
        public int Id { get; set; }

        public int AppId { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00}")]
        public int? Sprigs { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = false, DataFormatString = "{0:#,00}")]
        public int? Sod { get; set; }

        public string BillingInvoice { get; set; }

        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime HarvestDate { get; set; }

        public int HarvestNumber { get; set; }
       
        public Applications Application { get; set; }

       
    }
}
