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
                // Potato
                case 2:
                // Heritate Grain QA
                case 3:
                    return new ApplicationLabels
                    {
                        Ps1Variety = "Germplasm Entity",
                        PsCertNum = "Accession/QA Program Number/Lot Number"
                    };
                case 4:
                case 5:
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