using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using CCIA.Models;

namespace CCIA.Models
{
    public partial class CCIAEmployees
    {
        public string Id { get; set; }
       
        [Required]
        [StringLength(50)]
        [Display(Name="First Name")]
        public string FirstName { get; set; }
       
        [Required]
        [StringLength(50)]
        [Display(Name="Last Name")]
        public string LastName { get; set; }

        [StringLength(256)]
        [Display(Name = "Name")]        
        public string Name { 
            get {
                return FirstName + " " + LastName;
            }
        }

        public string Initials { 
            get {
                return $"{FirstName.Substring(0,1)}{LastName.Substring(0,1)}";
            }
        }

        [Display(Name="Email ID")]
        public string UCDMailID { get; set; }

        public string Email { 
            get{
                return UCDMailID + "@ucdavis.edu";
            } 
        }

      
        [Display(Name="Office")]
        public string CampusPhone { get; set; }
        [Display(Name="Cell")]
        public string CellPhone { get; set; }
        [Display(Name="Kerberos")]
        public string KerberosId { get; set; }
        public bool Current { get; set; }

        [Display(Name="Admin Access?")]
        public bool CCIAAccess { get; set; }

        [Display(Name="Core Staff?")]
        public bool CoreStaff { get; set; }

        [Display(Name="Field Inspector?")]
        public bool FieldInspector { get; set; }

        
        [Display(Name="Seed Lot Inform?")]
        public bool SeedLotInform { get; set; }

        [Display(Name="Edit Varieties?")]
        public bool EditVarieties { get; set; }

        [Display(Name="Billing?")]
        public bool BillingAccess { get; set; }        

        [Display(Name="Seasonal Employee?")]
        public bool SeasonalEmployee { get; set; }

        [Display(Name="New Tag Inform?")]
        public bool NewTag { get; set; }

        [Display(Name="Tag Print Inform?")]
        public bool TagPrint { get; set; }

        [Display(Name="Heritage QA Inform?")]
        public bool HeritageGrainQA { get; set; }

        [Display(Name="NS/PVG Inform?")]
        public bool PrevarietyGermplasm { get; set; }

        [Display(Name = "LacTracker Inform?")]
        public bool Lactracker { get; set; }

        [Display(Name="OECD Invoice Printer?")]
        public bool OECDInvoicePrinter { get; set; }

        public bool Admin { get; set; }

        [Display(Name="Cond. Status Update?")]
        public bool ConditionerStatusUpdate { get; set; }

        [Display(Name="Map Permissions Update?")]
        public bool UpdateMapPermissions { get; set; }

        [Display(Name="New Blend Inform")]
        public bool NewBlend { get; set; }
        [Display(Name="Admin Email Summary")]
        public bool AdminEmailSummary { get; set; }

        [ForeignKey("EmployeeId")]
        public ICollection<CropAssignments> AssignedCrops { get; set; }
        
    }
}
