using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Address
    {
        public int Id { get; set; }
        public int? ReferenceId { get; set; }
        public int? OcId { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        public string AddressLines
        {
            get
            {
                if(!string.IsNullOrWhiteSpace(Address3))
                {
                    return $"{Address1}<br>{Address2}<br>{Address3}";
                }
                if(!string.IsNullOrWhiteSpace(Address2))
                {
                    return $"{Address1}<br>{Address2}";
                }
                return Address1;
            }
        }
        public string City { get; set; }
        public int? CountyId { get; set; }
        [ForeignKey("CountyId")]
        public County County { get; set; }
        
        public int? StateProvinceId { get; set; }
        [ForeignKey("StateProvinceId")]
        public StateProvince StateProvince { get; set; }
        [Display(Name="Postal Code")]
        public string PostalCode { get; set; }
        public int? CountryId { get; set; }
        [ForeignKey("CountryId")]
        public Countries Countries { get; set; }
        public DateTime? DateModified { get; set; }
        public string UserModified { get; set; }

        public string CountyName 
        { 
            get
            {
                if (CountyId == null)
                {
                    return "";
                }                
                if(County != null) 
                {
                    return County.Name;

                }                
                return "";
            } 
        }

        public string StateName 
        { 
            get
            {
                if (StateProvinceId == null)
                {
                    return "";
                }                
                if(StateProvince != null) 
                {
                    return StateProvince.Name;

                }                
                return "";
            } 
        }

        public string CountryName 
        { 
            get
            {
                if (CountryId == null)
                {
                    return "";
                }                
                if(Countries != null) 
                {
                    return Countries.Name;

                }                
                return "";
            } 
        }
    }
}
