using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class LotBlends
    {
        public int CompId { get; set; }
        public int BlendId { get; set; }
        public int Sid { get; set; }
        public decimal Weight { get; set; }

        [ForeignKey("Sid")]
        public Seeds Seeds { get; set; }

        public bool DifferentConditioner(int OrgID)
        { 
            if(Seeds != null)
            {
                return Seeds.ConditionerId != OrgID;
            }
            return false;            
        }

        
    }
}
