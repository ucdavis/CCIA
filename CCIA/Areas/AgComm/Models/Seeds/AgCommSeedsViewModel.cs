using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.Collections.Generic;
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Models.AgComm
{
    public class AgCommSeedsViewModel
    {
        public Seeds seed { get; set; }
        public LabsAndStandards LabsAndStandards { get; set; }       

        public List<SeedDocuments>  Documents { get; set;}
        public List<SeedsDocumentTypes> documentTypes { get; set; }

        public string StandardsMessage { get; set; }
        public string AssayMessage { get; set; }
       

        public static async Task<AgCommSeedsViewModel> Create(CCIAContext _dbContext, int sid)
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
            if(labsAndStandards.Labs != null)
            {
                labsAndStandards.Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid);
            }            

            return new AgCommSeedsViewModel
            {
                seed = await _dbContext.Seeds.Where(s => s.Id == sid)
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
                Documents = await _dbContext.SeedDocuments.Where(d => d.SeedsId == sid).ToListAsync(),
                documentTypes = await _dbContext.SeedsDocumentTypes.OrderBy(d => d.Order).ToListAsync(),                
            };
        }        
    }
}
