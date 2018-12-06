using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class VarFamily
    {
        public int VarFamId { get; set; }
        public string VarFamName { get; set; }
        public int? VarOffId { get; set; }
        public int? OrigOfficialId { get; set; }
        public string VarietyType { get; set; }
        public bool? InUse { get; set; }
        public int? OecdCountry { get; set; }
        public bool? Confidential { get; set; }
        public string VarComments { get; set; }
        public string UpdateComments { get; set; }
        public bool? Experimental { get; set; }
        public bool? PrivateCode { get; set; }
        public bool? Oecd { get; set; }
        public bool? Alias { get; set; }
        public DateTime? DateEntered { get; set; }
        public string UserEntered { get; set; }
        public DateTime? DateUpdated { get; set; }
        public string UserUpdated { get; set; }
        public string DescHyperlink { get; set; }

        public VarOfficial VarOff { get; set; }
    }
}
