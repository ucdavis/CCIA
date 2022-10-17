using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel;

namespace CCIA.Models
{
    public partial class ContactMaps
    {
        public int Id { get; set; }
        public int ContactId { get; set; }

        public string Map { get; set; }

        public bool Allow { get; set; }
    }
}
