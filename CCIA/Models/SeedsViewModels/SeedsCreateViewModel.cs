using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models.SeedsCreateViewModel
{
    public class SeedsCreateViewModel
    {
        public Applications Application { get; set; }

        public List<AbbrevClassProduced> ClassProducible { get; set; }

        public NewSeeds Seed { get; set; }

        public List<County> Counties { get; set; }




        public static async Task<SeedsCreateViewModel> Create(CCIAContext _dbContext, int[] appId, int certYear, int certNum, int certRad)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == appId.First())
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(a => a.AppTypeTrans)
                .FirstAsync();
            var state = await _dbContext.StateProvince.Where(s => s.Name == "California").Select(s => s.StateProvinceId).FirstAsync();
            // TODO : get real org ID!
            var countyId = await _dbContext.Organizations.Where(o => o.Id == 168).Select(o => o.CountyId).FirstAsync();            
            var seed = new NewSeeds();
            seed.CountyDrawn = countyId;
            seed.AppId = appId;
            seed.CertYear = certYear;
            seed.SampleFormCertNumber = certNum;
            if(certRad == 0)
            {
                seed.SampleFormRad = null;
            } else
            {
                seed.SampleFormRad = certRad;
            }           

            var viewModel = new SeedsCreateViewModel
            {
                Application = app,
                ClassProducible = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == app.AppTypeTrans.AppTypeId && c.ClassProducedId >= app.ClassProducedId && c.ClassProducedTrans != "Inspection Only").
                    Select(m => new AbbrevClassProduced { ClassProducedId = m.ClassProducedId, ClassProducedTrans = m.ClassProducedTrans })
                    .ToListAsync(),
                Seed = seed,
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                    .Select(c => new County { CountyId = c.CountyId, Name = c.Name }).ToListAsync(),
            };

            return viewModel;
        }

        public static async Task<SeedsCreateViewModel> Return(CCIAContext _dbContext, NewSeeds seed)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == seed.AppId.First())
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(a => a.AppTypeTrans)
                .FirstAsync();
            var state = await _dbContext.StateProvince.Where(s => s.Name == "California").Select(s => s.StateProvinceId).FirstAsync();
            // TODO : get real org ID!
            var countyId = await _dbContext.Organizations.Where(o => o.Id == 168).Select(o => o.CountyId).FirstAsync();            
            

            var viewModel = new SeedsCreateViewModel
            {
                Application = app,
                ClassProducible = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == app.AppTypeTrans.AppTypeId && c.ClassProducedId >= app.ClassProducedId && c.ClassProducedTrans != "Inspection Only").
                    Select(m => new AbbrevClassProduced { ClassProducedId = m.ClassProducedId, ClassProducedTrans = m.ClassProducedTrans })
                    .ToListAsync(),
                Seed = seed,
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                    .Select(c => new County { CountyId = c.CountyId, Name = c.Name }).ToListAsync(),
            };

            return viewModel;
        }
    }

    public partial class NewSeeds
    {
        public NewSeeds() {
            Type = "Original Run";
        }
        public int[] AppId { get; set; }

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
        [Display(Name = "OECD Lot?")]
        public bool OECDLot { get; set; }

        public int? CertYear { get; set; }
        public int SampleFormCertNumber { get; set; }
        public int? SampleFormRad { get; set; }


    }
}
