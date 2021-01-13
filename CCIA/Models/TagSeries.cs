using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class TagSeries
    {
        public int Id { get; set; }

        public int TagId { get; set; }

        public string Letter { get; set; }

        public int Start { get; set; }

        public int End { get; set; }

        public bool Void { get; set; }

        [DatabaseGenerated(DatabaseGeneratedOption.Computed)]
        public int Count { get; set; }  

        
        public Tags Tag { get; set; }  


    }
}