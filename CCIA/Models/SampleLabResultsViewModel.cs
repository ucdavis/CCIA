using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;

namespace CCIA.Models.SampleLabResultsViewModel
{
    public class SampleLabResultsViewModel
    {
        public SampleLabResults LabResults { get; set; }
        public CropStandardsList  StandardsList { get; set; }

        public static async Task<SampleLabResultsViewModel> Create(CCIAContext _dbContext, int sid)
        {               
            return new SampleLabResultsViewModel
            {
                LabResults = await _dbContext.SampleLabResults.Where(s => s.SeedsId == sid)
                    .Include(l => l.LabOrganization)
                    .FirstOrDefaultAsync(),
                StandardsList = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid),
            };
        }

        
    }

    
}
