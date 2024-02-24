using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    //public partial class BlendLabResults : SampleLabResults
    //{
    //    public int BlendId { get; set; }
        

       
    //    [ForeignKey("BlendId")]
    //    public ICollection<BlendLabResultsChanges> BlendChanges { get; set; }
        

    //    public BlendRequests Blend { get; set; }


    //}

    public partial class BlendLabResultsChanges : SampleLabResultsChanges
    {
        public int BlendId { get; set; }
    }
}
