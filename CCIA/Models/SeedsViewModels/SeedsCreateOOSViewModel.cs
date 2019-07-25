using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CCIA.Helpers;

namespace CCIA.Models.SeedsCreateOOSViewModel
{
    public class SeedsCreateOOSViewModel
    {
        public List<Crops> Crops { get; set; }

        public List<AbbrevClassProduced> ClassProducable { get; set; }

        public List<int> CertYears { get; set; }

        public NewOOSSeeds Seed { get; set; }

        public List<County> Counties { get; set; }

        public List<Countries> Countries { get; set; }

        public List<StateProvince> States { get; set; }



        public static async Task<SeedsCreateOOSViewModel> Create(CCIAContext _dbContext)
        {   
            // TODO : get real org ID!
            var countyId = await _dbContext.Organizations.Where(o => o.OrgId == 168).Select(o => o.CountyId).FirstAsync();            
            var seed = new NewOOSSeeds();
            seed.CountyDrawn = countyId; 
            seed.CertYear = CertYearFinder.CertYear;
            var cal = await _dbContext.StateProvince.Where(s => s.StateProvinceName == "California").Select(s => s.StateProvinceId).FirstAsync();           
           

            var viewModel = new SeedsCreateOOSViewModel
            {
                States =  await _dbContext.StateProvince.Where(s => s.StateProvinceName != "California").Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, StateProvinceName = s.StateProvinceName}).ToListAsync(),
                ClassProducable = await _dbContext.AbbrevClassProduced.Where(c => c.AppType == 1 && c.ClassProducedTrans != "Inspection Only" && c.ClassProducedTrans != "Breeder").
                    Select(m => new AbbrevClassProduced { ClassProducedId = m.ClassProducedId, ClassProducedTrans = m.ClassProducedTrans })
                    .ToListAsync(),
                Seed = seed,
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == cal)
                    .Select(c => new County { CountyId = c.CountyId, CountyName = c.CountyName }).ToListAsync(),
            };

            return viewModel;
        }
    }

    public partial class NewOOSSeeds
    {   
        public int? ApplicantId { get; set; }     
        [Display(Name = "Lot Number")]
        [Required]
        public string LotNumber { get; set; }
        [Display(Name = "Lot Weight (pounds)")]
        [Required]
        [Range(1, 40000000, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public decimal PoundsLot { get; set; }

        public int? Class { get; set; }

        [Display(Name = "County Drawn")]
        public int? CountyDrawn { get; set; }
        [Display(Name = "Select Type")]
        [Required]
        public string Type { get; set; }
        public string Remarks { get; set; }
        public string SampleDrawnBy { get; set; }
        [Display(Name = "Name of Sampler")]
        [Required]
        public string SamplerName { get; set; }
        [Display(Name = "OECD Lot?")]
        public bool OECDLot { get; set; }

        public int? CertYear { get; set; }
        public int SampleFormCertNumber { get; set; }
        public int? SampleFormRad { get; set; }
    }
}
