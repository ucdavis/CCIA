using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;

namespace CCIA.Models.SeedsViewModels
{
    public class ClientSeedsViewModel
    {
        public Seeds seed { get; set; }
        public LabsAndStandards LabsAndStandards { get; set; }
       

        public static async Task<ClientSeedsViewModel> Create(CCIAContext _dbContext, int orgId, int sid)
        {
            if (!await _dbContext.SampleLabResults.AnyAsync(s => s.SeedsId == sid))
            {
                var labresults = new SampleLabResults();
                labresults.SeedsId = sid;
                await _dbContext.SampleLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();
            }
            
            var labsAndStandards = new LabsAndStandards();
            labsAndStandards.Labs = await _dbContext.SampleLabResults.Where(l => l.SeedsId == sid)
                    .Include(r => r.LabOrganization)
                    .FirstOrDefaultAsync();
            labsAndStandards.Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid);

            return new ClientSeedsViewModel
            {
                seed = await _dbContext.Seeds.Where(s => s.Id == sid && s.ConditionerId == orgId)
                    .Include(a => a.ApplicantOrganization)
                    .Include(c => c.ConditionerOrganization)
                    .Include(c => c.AppTypeTrans)
                    .Include(v => v.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(c => c.ClassProduced)
                    .Include(l => l.LabResults)
                    .Include(s => s.SeedsApplications)
                    .ThenInclude(sa => sa.Application)
                    .ThenInclude(a => a.GrowerOrganization)
                    .Include(s => s.Application)
                    .ThenInclude(a => a.Crop)
                    .FirstOrDefaultAsync(),
                LabsAndStandards = labsAndStandards,
                
            };
        }
    }
}
