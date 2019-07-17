using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace CCIA.Models
{
    public class ApplicationLabels
    {
        public string Ps1Variety { get; set; }
        public string PsCertNum { get; set; }

        public static ApplicationLabels Create(int appTypeId)
        {
            switch(appTypeId)
            {
                // Heritate Grain QA
                case 3:
                    return new ApplicationLabels
                    {
                        Ps1Variety = "Heritage Grain",
                        PsCertNum = "Accession/QA Program Number/Lot Number"
                    };
                // Pre-Variety Germplasm
                case 4:
                    return new ApplicationLabels
                    {
                        Ps1Variety = "Selection ID",
                        PsCertNum = "PV Program Number/Lot Number"
                    };
                // Rice
                case 5:
                    return new ApplicationLabels
                    {
                        Ps1Variety = "Rice QA Variety",
                        PsCertNum = "QA Program Number/Lot Number"
                    };
                // Hemp from Seed
                case 7:
                default:
                    return new ApplicationLabels
                    {
                        Ps1Variety = "Variety",
                        PsCertNum = "Certification and Lot Number"
                    };
            }
        }
    }
}