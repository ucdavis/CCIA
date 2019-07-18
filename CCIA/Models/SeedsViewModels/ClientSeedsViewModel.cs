using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.SeedsViewModels
{
    public class ClientSeedsViewModel
    {
        public Seeds seed { get; set; }
        public SxLabResults labResults { get; set; }


        public static async Task<ClientSeedsViewModel> Create(CCIAContext _dbContext, int orgId, int sid)
        {
            var viewModel = new ClientSeedsViewModel
            {
                seed = await _dbContext.Seeds.Where(s => s.Id == sid && s.ConditionerId == orgId).Include(a => a.ApplicantOrganization)
                    .Include(c => c.ConditionerOrganization)
                    .Include(c => c.AppTypeTrans)
                    .Include(v => v.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(c => c.ClassProduced)
                    .Include(l => l.LabResults)
                    .Include(s => s.SeedsApplications)
                    .ThenInclude(sa => sa.Application)
                    .ThenInclude(a => a.GrowerOrganization)
                    .FirstOrDefaultAsync(),
                labResults = await _dbContext.SxLabResults.Where(l => l.SeedsId == sid)                    
                    .Include(r => r.LabOrganization)
                    .FirstOrDefaultAsync(),
            };

            return viewModel;
        }
    }
}
