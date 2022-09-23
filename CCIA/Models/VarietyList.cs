using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{   

    public class VarietyList
    {
        public int Id { get; set; }        
        [Display(Name="Variety")]
        [Required]
        public string Name { get; set; }

         public string NameAndId => Id + " " + Name;

        public int CropId { get; set; }
        
    }
}
