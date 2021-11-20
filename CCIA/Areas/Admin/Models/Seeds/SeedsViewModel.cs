using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using CCIA.Services;

namespace CCIA.Models.ViewModels
{   
    public class AdminSeedsViewModel 
    {
        public Seeds seed { get; set; }

        public LabsAndStandards LabsAndStandards { get; set; }

        public List<SeedDocuments> Documents { get; set; }

        public List<SeedsDocumentTypes> documentTypes { get; set; }

        public List<int> certYears { get; set; }

        public List<Countries> countries { get; set; }
        public List<StateProvince> states { get; set; }
        public List<AbbrevClassSeeds> classes { get; set; }

        public static async Task<AdminSeedsViewModel> CreateDetails(CCIAContext _dbContext, int sid, IFullCallService _helper)
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
          
            var viewModel = new AdminSeedsViewModel
            {
                seed =  await _helper.FullSeeds().Where(s => s.Id == sid).FirstOrDefaultAsync(),  
                LabsAndStandards = labsAndStandards, 
                Documents =  await _dbContext.SeedDocuments.Where(d => d.SeedsId == sid).Include(d => d.DocumentType).ToListAsync(),
                documentTypes = await _dbContext.SeedsDocumentTypes.OrderBy(d => d.Order).ToListAsync(),
            };           

            return viewModel;
        }

        public static async Task<AdminSeedsViewModel> EditDetails(CCIAContext _dbContext, int sid, IFullCallService _helper)
        {
            var countryList = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();            
            countryList.Insert(0, new Countries{ Id = 0, Name = ""});
            var stateList = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();
            stateList.Insert(0, new StateProvince{ StateProvinceId = 0, Name = ""});


            var viewModel = new AdminSeedsViewModel
            {
                seed =  await _helper.FullSeeds().Where(s => s.Id == sid).FirstOrDefaultAsync(),        
                certYears = Helpers.CertYearFinder.certYearList,  
                countries = countryList,  
                states = stateList,    
                classes = await _dbContext.AbbrevClassSeeds.Include(c => c.AppType).OrderBy(c => c.AppType).ThenBy(c => c.SortOrder).ToListAsync(),
            }; 
            return viewModel;

        }

        


    } 
}
