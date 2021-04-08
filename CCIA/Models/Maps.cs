using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CCIA.Models
{
    public partial class Maps
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
    }
}
