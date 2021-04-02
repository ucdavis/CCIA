using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Contacts
    {
        public int Id { get; set; }
        
        public int OrgId { get; set; }
        
        public string Title { get; set; }
        public string FormOfAddr { get; set; }
        public string FirstName { get; set; }
        public string Mi { get; set; }
        public string LastName { get; set; }

        [StringLength(256)]
        [Display(Name = "Name")]        
        public string Name { 
            get {
                return FirstName + " " + LastName;
            }
        }

        public string Suffix { get; set; }
        public string BusinessPhone { get; set; }
        public string BusinessPhoneExtension { get; set; }
        public string MobilePhone { get; set; }
        public string FaxNumber { get; set; }
        public string HomePhone { get; set; }
        public string PagerNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Byte[] PasswordHash { get; set; }
        
        public Byte[] Salt { get; set; }        
        
        public bool AllowPinning { get; set; }
        public bool AllowApps { get; set; }
        public bool AllowSeeds { get; set; }
        
        public int? AlfalfaLastYearAgreement { get; set; }
        public int? SweetCornLastYearAgreement { get; set; }
        public int? IdahoVegetableLastYearAgreement { get; set; }

        

       
        public string FullName() {
            return $"{FirstName} {LastName}";
        }
    }
}
