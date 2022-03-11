using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class StatesAndCountries
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public int Ord { get; set; }
    }
}