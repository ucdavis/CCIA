using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public class cropSummaryCount
    {
        public string Crop { get; set; }
        public int PendingAppCount { get; set; }
        public int TotalCount { get; set; }
    }
   
}