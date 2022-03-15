using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class CropAssignments
    {
        public int Id { get; set; }
        public int CropId { get; set; }
        public string EmployeeId { get; set; }
        public int Level { get; set; }  

        [ForeignKey("CropId")]
        public Crops AssignedCrop { get; set; }      

    }
}