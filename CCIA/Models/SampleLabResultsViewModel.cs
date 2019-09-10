using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;

namespace CCIA.Models.SampleLabResultsViewModel
{
    public class SampleLabResultsViewModel
    {
        public SampleLabResults Labs { get; set; }
        public CropStandardsList  Standards { get; set; }

        public static async Task<SampleLabResultsViewModel> Create(CCIAContext _dbContext, int sid)
        {               
            return new SampleLabResultsViewModel
            {
                Labs = await _dbContext.SampleLabResults.Where(s => s.SeedsId == sid)
                    .Include(l => l.LabOrganization)
                    .FirstOrDefaultAsync(),
                Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid),
            };
        }

        
    }

    
}
