using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;


namespace CCIA.Models
{
    public class VarCountries
    {
        public int Id { get; set; }
        public int VarId { get; set; }

        public int CountryId { get; set; }

        [ForeignKey("CountryId")]
        public Countries Country { get; set; }
       

    }
}