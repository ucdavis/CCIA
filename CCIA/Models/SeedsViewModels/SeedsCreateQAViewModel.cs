using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CCIA.Helpers;

namespace CCIA.Models.SeedsCreateQAViewModel
{
    public class SeedsCreateQAViewModel
    {
        public NewQASeeds Seed { get; set; }

        public List<County> Counties { get; set; }

        public List<AbbrevAppType> AppTypes { get; set; }

        public List<int> CertYears { get; set; }

        public string DisplayEntry { get; set; }

        public Applications Application { get; set; }




        public static async Task<SeedsCreateQAViewModel> Create(CCIAContext _dbContext)
        {           
            var state = await _dbContext.StateProvince.Where(s => s.StateProvinceName == "California").Select(s => s.StateProvinceId).FirstAsync();
            var countyId = await _dbContext.Organizations.Where(o => o.OrgId == 168).Select(o => o.CountyId).FirstAsync();            
            var seed = new NewQASeeds();
            seed.CountyDrawn = countyId;
            seed.CertYear = CertYearFinder.CertYear;

            var viewModel = new SeedsCreateQAViewModel
            {                
                Seed = seed,
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                    .Select(c => new County { CountyId = c.CountyId, CountyName = c.CountyName }).ToListAsync(),
                AppTypes = await _dbContext.AbbrevAppType.Where(a => a.QAProgram == true).Select(a => new AbbrevAppType{ AppTypeId =a.AppTypeId, CertificateTitle = a.CertificateTitle}).ToListAsync(),
                CertYears = Enumerable.Range(2016, seed.CertYear.Value - 2015).ToList(),
                DisplayEntry = "display: none !important;",
            };

            return viewModel;
        }

        public static async Task<SeedsCreateQAViewModel> Return(CCIAContext _dbContext, NewQASeeds seed)
        {            
            var state = await _dbContext.StateProvince.Where(s => s.StateProvinceName == "California").Select(s => s.StateProvinceId).FirstAsync();            
            var countyId = await _dbContext.Organizations.Where(o => o.OrgId == 168).Select(o => o.CountyId).FirstAsync();
            var appType = await _dbContext.AbbrevAppType.Where(a => a.AppTypeId == seed.AppType).Select(a => a.Abbreviation).FirstOrDefaultAsync();  
            var app = await _dbContext.Applications.Where(a => a.Id == seed.AppId && a.AppType == appType && a.CertYear == seed.CertYear)
                    .Include(a => a.ApplicantOrganization)
                    .Include(a => a.Variety)
                    .Include(a => a.ClassProduced)
                    .FirstOrDefaultAsync(); 

            var viewModel = new SeedsCreateQAViewModel
            {                
                Seed = seed,
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                    .Select(c => new County { CountyId = c.CountyId, CountyName = c.CountyName }).ToListAsync(),
                AppTypes = await _dbContext.AbbrevAppType.Where(a => a.QAProgram == true).Select(a => new AbbrevAppType{ AppTypeId =a.AppTypeId, CertificateTitle = a.CertificateTitle}).ToListAsync(),
                CertYears = Enumerable.Range(2016, seed.CertYear.Value - 2015).ToList(),
                DisplayEntry = app == null ? "display: none !important;" : "",
                Application = app,
            };

            return viewModel;
        }
    }

    public partial class NewQASeeds
    {
        
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
        [Display(Name = "AASCO Id")]
        public string SamplerId { get; set; }
        

        public int? CertYear { get; set; }
        [Display(Name = "App Id")]
        public int? AppId { get; set; }
        [Display(Name = "QA Program")]
        public int AppType { get; set; }

    }
}
