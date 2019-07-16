using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.SeedsCreateViewModel
{
    public class SeedsCreateViewModel
    {
        public Applications Application { get; set; }
        
        public List<AbbrevClassProduced> ClassProducable { get; set; }

        public Seeds Seed { get; set; }

        public List<County> Counties { get; set; }

        public int[] AppId { get; set; }


        public static async Task<SeedsCreateViewModel> Create(CCIAContext _dbContext, int[] appId)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == appId.First())
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .FirstAsync();
            var viewModel = new SeedsCreateViewModel
            {
                Application = app,
                ClassProducable = await _dbContext.AbbrevClassProduced.Where(c => c.ClassAbbrv == app.AppType && c.ClassProducedId >= app.ClassProducedId).ToListAsync(),
                Seed = new Seeds(),
                AppId = appId,
            };

            return viewModel;
        }
    }
}
