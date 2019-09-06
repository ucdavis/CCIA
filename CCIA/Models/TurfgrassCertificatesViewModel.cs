using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    public class TurfgrassCertificatesViewModel {

        public Applications application { get; set; }
        public int Id { get; set; }
        public TurfgrassCertificates TurfgrassCertificates { get; set;}
 
        public static async Task<TurfgrassCertificatesViewModel> Create(CCIAContext _dbContext, int id) {
            
            var viewModel = new TurfgrassCertificatesViewModel()
            {
                Id = id,
                TurfgrassCertificates = new TurfgrassCertificates()
            };

            return viewModel;
        }

        public static async Task<TurfgrassCertificatesViewModel> Certificate(CCIAContext _dbContext, int id, int certId) {

            var viewModel = new TurfgrassCertificatesViewModel()
            {
                application = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.Id == id)                
                    .Include(a => a.GrowerOrganization)    
                    .ThenInclude(g => g.Address)  
                    .ThenInclude(a => a.StateProvince)          
                    .Include(a => a.County)
                    .Include(a => a.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(a => a.ClassProduced)
                    .Include(a => a.TurfgrassCertificates)
                    .FirstOrDefaultAsync(),
                TurfgrassCertificates = await _dbContext.TurfgrassCertificates.Where(t => t.Id == certId)
                    .FirstOrDefaultAsync()
                                };

            return viewModel;
        }

        public static async Task<TurfgrassCertificatesViewModel> Edit(CCIAContext _dbContext, int id) {

            var viewModel = new TurfgrassCertificatesViewModel()
            {
                application = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.Id == id)                
                    .Include(a => a.GrowerOrganization)                
                    .Include(a => a.County)
                    .Include(a => a.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(a => a.ClassProduced)
                    .Include(a => a.TurfgrassCertificates)
                    .FirstOrDefaultAsync()
            };

            return viewModel;
        }
    }
}