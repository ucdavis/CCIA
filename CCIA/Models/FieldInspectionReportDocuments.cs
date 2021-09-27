using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Models
{
    public partial class FIRDocuments
    {
        [Key]
        public int Id { get; set; }
        public int AppId { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        
        

    }
}