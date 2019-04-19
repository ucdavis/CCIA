using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Models
{
    public class VarFull
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }

        public Crops Crop { get; set; }
        public int CropId { get; set; }
        public string Category { get; set; }
        public string Status { get; set; }
        public bool Certified { get; set; }
       
       
        public bool RiceQa { get; set; }
        public string RiceColor { get; set; }
        public int ParendId { get; set; }
        public bool Turfgrass { get; set; }

        public ICollection<Applications> Applications { get; set; }

        

    }
}
