﻿using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class AbbrevAppType
    {
        public int AppTypeId { get; set; }
        public string AppTypeTrans { get; set; }
        public string Abbreviation { get; set; }
        public string CertificateTitle { get; set; }
        public string NumberTitle { get; set; }
        public string SirTitle { get; set; }
        public string ProcessTitle { get; set; }
        public string SpeciesOrCrop { get; set; }
        public string ProducedTitle { get; set; }
        public string VarietyTitle { get; set; }
        public bool? ShowType { get; set; }
    }
}
