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
        public string BusPhone { get; set; }
        public string BusPhoneExt { get; set; }
        public string MobilePhone { get; set; }
        public string FaxNo { get; set; }
        public string HomePhone { get; set; }
        public string PagerNo { get; set; }
        public string EmailAddr { get; set; }
        public string Password { get; set; }

        public Byte[] PasswordHash { get; set; }
        
        public Byte[] Salt { get; set; }
        public string ContactType { get; set; }
        public bool CciaMember { get; set; }
        public int? CciaMemberYear { get; set; }
        public bool? BoardMember { get; set; }
        public string BoardTitle { get; set; }
        public string BoardRepresent { get; set; }
        public bool BoardActive { get; set; }
        public bool AgCommissioner { get; set; }
        public bool DeputyCommissioner { get; set; }
        public bool FarmAdvisor { get; set; }
        public bool LabContact { get; set; }
        public bool CertifiedSeedSx { get; set; }
        public string CertifiedSeedSxNo { get; set; }
        public bool MailListGrBook { get; set; }
        public bool MailListSeednotes { get; set; }
        public bool Active { get; set; }
        public DateTime? MemberSince { get; set; }
        public bool CreateApps { get; set; }
        public DateTime? DateAdded { get; set; }
        public int? UserAdding { get; set; }
        public int? UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public bool CurrentYearReview { get; set; }
        public string UserEmpModified { get; set; }
        public DateTime? UserEmpModDt { get; set; }
        public string Comments { get; set; }
        public bool AllowPinning { get; set; }
        public bool AllowApps { get; set; }
        public bool AllowSeeds { get; set; }
        public bool AuditNotify { get; set; }
        public int? AlfalfaLastYearAgreement { get; set; }
        public int? SweetCornLastYearAgreement { get; set; }
        public int? IdahoVegetableLastYearAgreement { get; set; }

        

       
        public string FullName() {
            return $"{FirstName} {LastName}";
        }
    }
}
