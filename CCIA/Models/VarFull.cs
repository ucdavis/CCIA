using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;


namespace CCIA.Models
{
    public class VarFull
    {
        public int Id { get; set; }
        [DisplayName("Variety")] 
        public string Name { get; set; }
        public string Type { get; set; }

        [ForeignKey("CropId")]
        public Crops Crop { get; set; }
        public int CropId { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool Certified { get; set; }
       
       
        public bool RiceQa { get; set; }
        public string RiceColor { get; set; }
        public int ParentId { get; set; }
        public bool Turfgrass { get; set; }

       

        

    }
}
