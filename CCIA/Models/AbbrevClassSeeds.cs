using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class AbbrevClassSeeds
    {
        
        [Key]
        public int Id { get; set; }
        public string Abbrv { get; set; }
        public string CertClass { get; set; }
        public int? SortOrder { get; set; }
        
        public int  Program { get; set; }

        [ForeignKey("Program")]
        public AbbrevAppType AppType { get; set; }

        public string NameAndAppType 
        { 
            get 
            {
                if(AppType != null)
                {
                    return $"{AppType.Abbreviation}-{CertClass}";
                }
                return "";
            }
        }
        
    }
}
