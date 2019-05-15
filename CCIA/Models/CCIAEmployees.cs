using System;
using System.Collections.Generic;

namespace CCIA.Models
{
    public partial class CCIAEmployees
    {
        public string Id { get; set; }
       
        public string FirstName { get; set; }
       
        public string LastName { get; set; }
        public string UCDMaildID { get; set; }

        public string CampusRoom { get; set; }
        public string CampusBuilding { get; set; }
        public string CampusPhone { get; set; }
        public string CellPhone { get; set; }
        public string KerberosId { get; set; }
        public bool Current { get; set; }

        public bool CCIAAccess { get; set; }

        public bool CoreStaff { get; set; }

        public bool FieldInspector { get; set; }

        public bool SeedLotInform { get; set; }

        public bool EditVarieties { get; set; }

        public bool BillingAccess { get; set; }

        public bool SeedLab { get; set; }

        public bool SeasonalEmployee { get; set; }

        public bool NewTag { get; set; }

        public bool TagPrint { get; set; }

        public bool HeritageGrainQA { get; set; }

        public bool PrevarietyGermplasm { get; set; }

        public bool OECDInvoicePrinter { get; set; }
        
    }
}
