using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;

namespace CCIA.Models
{   
     
    public class AdminSeedTransferSearchViewModel
    {
        public List<SeedTransfers> stc { get; set; } 

        public int searchId { get; set; } 

        public List<int> searchProgramId { get; set; }                   
       
        public List<AbbrevAppType> programTypes {get; set;} 

        public List<int> creationYears { get; set; }

        public List<int> searchYearsCreated { get; set; }

        public string searchConditioner { get; set; }

        public int searchSid { get; set; }

        public int searchBid { get; set; }


               
        public static async Task<AdminSeedTransferSearchViewModel> Create(CCIAContext _dbContext, AdminSeedTransferSearchViewModel vm, IFullCallService helper)
        {               
            

            if(vm != null)
            {
                var stcToFind = helper.OverviewSeedTransfers().AsQueryable();

                if(vm.searchSid != 0)
                {
                    stcToFind = stcToFind.Where(b => b.SeedsID == vm.searchSid);
                }
                if(vm.searchBid !=0)
                {
                    stcToFind = stcToFind.Where(b => b.BlendId == vm.searchBid);
                }
                // if(!string.IsNullOrWhiteSpace(vm.searchConditioner))
                // {
                //     stcToFind = stcToFind.Where(b => EF.Functions.Like(b.ConditionerOrganization.Name, "%" + vm.searchConditioner + "%") || b.ConditionerOrganizationId.ToString() == vm.searchConditioner);
                // }
                // if(vm.searchYearsCreated != null && vm.searchYearsCreated.Count > 0)
                // {
                //     stcToFind = stcToFind.Where(b => vm.searchYearsCreated.Contains(b.Date.Year));
                // }
                
                
                var viewModel = new AdminSeedTransferSearchViewModel
                {
                    stc = await stcToFind.ToListAsync(),  
                    programTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync(),
                    creationYears =  CertYearFinder.certYearList,
                };  
                return viewModel;


            }
            var freshModel = new AdminSeedTransferSearchViewModel
            {
                stc = new List<SeedTransfers>(),
                programTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync(),
                creationYears =  CertYearFinder.certYearList,
            };           

            return freshModel;
        }

        
    } 

     
    
}
