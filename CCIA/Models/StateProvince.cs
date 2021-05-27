using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{
    public partial class StateProvince
    {
        public int StateProvinceId { get; set; }
        public string StateProvinceCode { get; set; }
        public int CountryId { get; set; }
        [Display(Name="State")]        
        public string Name { get; set; }
        public DateTime DateModified { get; set; }

        public string StateWithCountry 
        { 
            get
            {
                if(CountryId==58)
                {
                    return $"{Name}, USA";            
                }
                return Name;
            }
        
        }


       
    }
}
