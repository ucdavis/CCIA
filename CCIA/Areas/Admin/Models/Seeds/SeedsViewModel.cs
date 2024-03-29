using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using CCIA.Services;
using Microsoft.Data.SqlClient;

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

        public List<AbbrevAppType> certPrograms { get; set; }

        public List<County> counties { get; set; }
        public List<SeedsPreviousTagBag> seedsPreviousTagBag { get; set; }

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
            var p0 = new SqlParameter("@sid", sid); 
          
            var viewModel = new AdminSeedsViewModel
            {
                seed =  await _helper.FullSeeds().Where(s => s.Id == sid).FirstOrDefaultAsync(),  
                LabsAndStandards = labsAndStandards, 
                Documents =  await _dbContext.SeedDocuments.Where(d => d.SeedsId == sid).Include(d => d.DocumentType).ToListAsync(),
                documentTypes = await _dbContext.SeedsDocumentTypes.OrderBy(d => d.Order).ToListAsync(),
                seedsPreviousTagBag =  await _dbContext.SeedPreviousTagBag.FromSqlRaw($"EXEC mvc_SeedsPreviousTagBag @sid", p0).ToListAsync(),
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

        public static async Task<AdminSeedsViewModel> ClientEditDetails(CCIAContext _dbContext, int sid, IFullCallService _helper)
        {
            var seedToEdit = await _helper.FullSeeds().Where(s => s.Id == sid).FirstOrDefaultAsync();

            var viewModel = new AdminSeedsViewModel
            {
                seed = seedToEdit, 
                classes = await _dbContext.AbbrevClassSeeds.Include(c => c.AppType).Where(c => c.Id >= seedToEdit.Class && c.Program == seedToEdit.AppTypeTrans.AppTypeId).OrderBy(c => c.SortOrder).ToListAsync(),
            }; 
            return viewModel;

        }

         public static async Task<AdminSeedsViewModel> CreateNew(CCIAContext _dbContext)
        {
            var countryList = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();            
            countryList.Insert(0, new Countries{ Id = 0, Name = ""});
            var stateList = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();
            stateList.Insert(0, new StateProvince{ StateProvinceId = 0, Name = ""});


            var viewModel = new AdminSeedsViewModel
            {
                seed =  new Seeds(),        
                certYears = Helpers.CertYearFinder.certYearList,  
                countries = countryList,  
                states = stateList,    
                classes = await _dbContext.AbbrevClassSeeds.Include(c => c.AppType).OrderBy(c => c.AppType).ThenBy(c => c.SortOrder).ToListAsync(),
                certPrograms = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync(),
                counties = await _dbContext.County.OrderBy(c => c.Name).Where(c => c.StateProvinceId == 102).ToListAsync(),
            }; 
            viewModel.seed.CertYear = Helpers.CertYearFinder.CertYear - 1;
            viewModel.seed.CertProgram = "SD";
            viewModel.counties.Insert(0, new County{CountyId = 0, Name = ""});
            return viewModel;

        }

        


    } 
}
