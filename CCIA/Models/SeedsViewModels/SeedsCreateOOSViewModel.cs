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

        public List<AbbrevClassProduced> ClassProducible { get; set; }

        public List<int> CertYears { get; set; }

        public NewOOSSeeds Seed { get; set; }

        public List<County> Counties { get; set; }

        public List<Countries> Countries { get; set; }

        public List<StateProvince> States { get; set; }



        public static async Task<SeedsCreateOOSViewModel> Create(CCIAContext _dbContext)
        {               
            var countyId = await _dbContext.Organizations.Where(o => o.Id == 168).Select(o => o.CountyId).FirstAsync();            
            var seed = new NewOOSSeeds();
            seed.CountyDrawn = countyId; 
            seed.CertYear = CertYearFinder.CertYear;
            var cal = await _dbContext.StateProvince.Where(s => s.Name == "California").Select(s => s.StateProvinceId).FirstAsync();           
           

            var viewModel = new SeedsCreateOOSViewModel
            {
                States =  await _dbContext.StateProvince.Where(s => s.Name != "California").Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),
                ClassProducible = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == 1 && c.ClassProducedTrans != "Inspection Only" && c.ClassProducedTrans != "Breeder").
                    Select(m => new AbbrevClassProduced { ClassProducedId = m.ClassProducedId, ClassProducedTrans = m.ClassProducedTrans })
                    .ToListAsync(),
                Seed = seed,
                Crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true).Select(c => new Crops { CropId = c.CropId, Crop = c.Crop, CropKind = c.CropKind}).ToListAsync(),
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == cal)
                    .Select(c => new County { CountyId = c.CountyId, Name = c.Name }).ToListAsync(),
                Countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                CertYears =  CertYearFinder.certYearListReverse.ToList(),
            };

            return viewModel;
        }

        public static async Task<SeedsCreateOOSViewModel> Return(CCIAContext _dbContext, NewOOSSeeds seed )
        {   
            // TODO : get real org ID!
            var countyId = await _dbContext.Organizations.Where(o => o.Id == 168).Select(o => o.CountyId).FirstAsync(); 
            var cal = await _dbContext.StateProvince.Where(s => s.Name == "California").Select(s => s.StateProvinceId).FirstAsync();           
            var currentCertYear =  CertYearFinder.CertYear;

            var viewModel = new SeedsCreateOOSViewModel
            {
                States =  await _dbContext.StateProvince.Where(s => s.Name != "California").Select(s => new StateProvince{ StateProvinceId = s.StateProvinceId, Name = s.Name, CountryId = s.CountryId}).OrderBy(s => s.CountryId).ThenBy(s => s.Name).ToListAsync(),
                ClassProducible = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == 1 && c.ClassProducedTrans != "Inspection Only" && c.ClassProducedTrans != "Breeder").
                    Select(m => new AbbrevClassProduced { ClassProducedId = m.ClassProducedId, ClassProducedTrans = m.ClassProducedTrans })
                    .ToListAsync(),
                Seed = seed,
                Crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true).Select(c => new Crops { CropId = c.CropId, Crop = c.Crop, CropKind = c.CropKind}).ToListAsync(),
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == cal)
                    .Select(c => new County { CountyId = c.CountyId, Name = c.Name }).ToListAsync(),
                Countries = await _dbContext.Countries.OrderByDescending(c => c.US).ThenBy(c => c.Name).Select(c => new Countries { Id = c.Id, Name = c.Name}).ToListAsync(),
                CertYears = Enumerable.Range(2007, currentCertYear - 2006).ToList(),
            };

            return viewModel;
        }
    }

    public partial class NewOOSSeeds
    {   
        public NewOOSSeeds() {
            Type = "Original Run";
        }


        public int? ApplicantId { get; set; }     
        [Display(Name = "Lot Number")]
        [Required]
        public string LotNumber { get; set; }
        [Display(Name = "Lot Weight (pounds)")]
        [Required]
        [Range(1, 40000000, ErrorMessage = "Value for {0} must be between {1} and {2}")]
        public decimal PoundsLot { get; set; }
        [Required]
        [Display(Name = "Certification Number")]
        public string SampleFormCertNumber { get; set; }

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
        [Display(Name = "OECD Lot?")]
        public bool OECDLot { get; set; }

        [Display(Name = "Finally Certified by Other Agency?")]
        [Required]
        public string CertByOtherAgency { get; set; }

        public int? CertYear { get; set; }
        
        public int? SampleFormRad { get; set; }
 
        [Display(Name = "Variety")]
         public int? SampleFormVarietyId { get; set; }
        
        [Required]
        [Display(Name = "State of Origin")]
        public int OriginState { get; set; }
        [Required]
        [Display(Name = "Country of Origin")]
        public int OriginCountry { get; set; }
    }
}
