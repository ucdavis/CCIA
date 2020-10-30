using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;

namespace CCIA.Models.ViewModels
{   
    public class AdminSeedsViewModel 
    {
        public Seeds seed { get; set; }

        public LabsAndStandards LabsAndStandards { get; set; }

        public static async Task<AdminSeedsViewModel> CreateDetails(CCIAContext _dbContext, int sid)
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

           var seed = await _dbContext.Seeds.Where(s => s.Id == sid)
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(v => v.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(c => c.ClassProduced)
                .Include(l => l.LabResults)
                .Include(s => s.CountryOfOrigin)
                .Include(s => s.StateOfOrigin)
                .Include(s => s.CountySampleDrawn)
                .Include(s => s.ContactEntered)
                .Include(s => s.Documents)
                .ThenInclude(d => d.DocumentType)                
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.GrowerOrganization)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.ApplicantOrganization)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.Crop)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.Variety)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.ClassProduced)
                .FirstOrDefaultAsync();
            var viewModel = new AdminSeedsViewModel
            {
                seed = seed,  
                LabsAndStandards = labsAndStandards,        
            };           

            return viewModel;
        }

        


    } 
}
