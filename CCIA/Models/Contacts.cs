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
        [Display(Name="Form of Address")]
        public string FormOfAddr { get; set; }
        [Display(Name="First Name")]
        public string FirstName { get; set; }
        [Display(Name="Middle Initial")]
        public string MiddleInitial { get; set; }
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [StringLength(256)]
        [Display(Name = "Name")]        
        public string Name { 
            get {
                return FirstName + " " + LastName;
            }
        }

        public string Suffix { get; set; }
        [Display(Name="Business #")]
        public string BusinessPhone { get; set; }
        [Display(Name="Business # Ext.")]
        public string BusinessPhoneExtension { get; set; }
        [Display(Name="Mobile #")]
        public string MobilePhone { get; set; }
        [Display(Name="Fax #")]
        public string FaxNumber { get; set; }
        [Display(Name="Home #")]
        public string HomePhone { get; set; }
        [Display(Name="Pager #")]
        public string PagerNumber { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }

        public Byte[] PasswordHash { get; set; }
        
        public Byte[] Salt { get; set; }        
        
        [Display(Name="Can Pin?")]
        public bool AllowPinning { get; set; }
        [Display(Name="Allow Apps?")]
        public bool AllowApps { get; set; }
        [Display(Name="Allow Seeds")]
        public bool AllowSeeds { get; set; }
        [Display(Name="Allow Org Update")]
        public bool AllowOrgUpdate { get; set; }

        
        [Display(Name="Rec. App. Notices")]
        public bool ApplicationNotices { get; set; }
         [Display(Name="Rec. Seed Notices")]
        public bool SeedNotices { get; set; }
         [Display(Name="Rec. Tag Notices")]
        public bool TagNotices { get; set; }
        [Display(Name="Rec. OECD Notices")]
        public bool OECDNotices { get; set; }
        [Display(Name="Rec. Blend Notices")]
        public bool BlendNotices { get; set; }
        [Display(Name="Rec. Bulk Sales Notices")]
        public bool BulkSalesNotices { get; set; }
        [Display(Name="Rec. Transfer Notices")]
        public bool TransferNotices { get; set; }
        
        public int? AlfalfaLastYearAgreement { get; set; }
        public int? SweetCornLastYearAgreement { get; set; }
        public int? IdahoVegetableLastYearAgreement { get; set; }
        public int? LastApplicationAgreementYear { get; set; }

        [ForeignKey("ContactId")]
        public ICollection<ContactAddress> Addresses { get; set; }

        

       
        public string FullName() {
            return $"{FirstName} {LastName}";
        }
    }
}
