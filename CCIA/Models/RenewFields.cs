using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class RenewFields
    {
        public int Id { get; set; }
        public int AppId { get; set; }
        public int Year { get; set; }
        public int Action { get; set; }
        public DateTime? DateRenewed { get; set; }

        [ForeignKey("AppId")]
        public Applications Application { get; set; }

    }
}
