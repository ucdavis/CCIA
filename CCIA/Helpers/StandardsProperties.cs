using CCIA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Helpers
{
    public class StandardsProperties
    {
        public int CropId { get; set; }
        public string CertProgram { get; set; }
        public string ClassAbbreviation { get; set; }

        public int ClassId { get; set; }

    }
        
}