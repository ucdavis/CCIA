using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;

namespace CCIA.Models
{
    public partial class Organizations
    {
        public int Id { get; set; }
        [DisplayName("Org Name")] 
        public string Name { get; set; }
        
        public Address Address { get; set; }
        public int? AddressId { get; set; }

        public string Phone { get; set; }
        public string Fax { get; set; }

        public string Email { get; set; }
        public string Website { get; set; }

        public string NameAndId => Id + " " + Name;

        public int? CountyId { get; set; }
        public County OrgCounty { get; set; }

        public bool GermLab { get; set; }
        //public bool FoundationSeedGrower { get; set; }
        public bool DiagnosticLab { get; set; }
        public bool AgCommissioner { get; set; }

        public string District { get; set; }
        public bool Member { get; set; }

        public int? MemberYear { get; set; }
        public string MemberType { get; set; }
        public DateTime? LastMemberAgreement { get; set; }
        public DateTime? MemberSince { get; set; }
        [ForeignKey("Id")]
        public int? RepresentativeContactId { get; set; }
        [ForeignKey("RepresentativeContactId")]
        public Contacts RepresentativeContact { get; set; }
        public bool Active { get; set; }
        public string UserModified { get; set; }
        public DateTime? DateModified { get; set; }
        public string Notes { get; set; }
        public int? AppYearAgree { get; set; }
        //public int? LacYearAgree { get; set; }
        public bool AlfalfaGMOPinning { get; set; }
        



        [ForeignKey("OrgId")]
        public ICollection<OrganizationAddress> Addresses { get; set; }
       
        public ICollection<Applications> AppliedApplications { get; set; }
        
        [ForeignKey("OrgId")]
        public ICollection<Contacts> Employees { get; set; }

        [ForeignKey("OrgId")]
        public ICollection<CondStatus> ConditionerStatus { get; set; }

        [ForeignKey("OrgId")]
        public ICollection<OrgMaps> MapPermissions { get; set; }

        [ForeignKey("OrgId")]
        public ICollection<OrgMapCrops> MapCropPermissions { get; set; }

    }
}
