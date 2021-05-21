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
     
    public class AdminBulkSalesCertificateSearchViewModel
    {
        public List<BulkSalesCertificates> bsc { get; set; } 

        public int searchId { get; set; } 

        public List<int> searchProgramId { get; set; }                   
       
        public List<AbbrevAppType> programTypes {get; set;} 

        public List<int> creationYears { get; set; }

        public List<int> searchYearsCreated { get; set; }

        public string searchConditioner { get; set; }

        public int searchSid { get; set; }

        public int searchBid { get; set; }


               
        public static async Task<AdminBulkSalesCertificateSearchViewModel> Create(CCIAContext _dbContext, AdminBulkSalesCertificateSearchViewModel vm, IFullCallService helper)
        {               
            

            if(vm != null)
            {
                var bscToFind = helper.OverviewBulkSalesCertificates().AsQueryable();

                if(vm.searchSid != 0)
                {
                    bscToFind = bscToFind.Where(b => b.SeedsID == vm.searchSid);
                }
                if(vm.searchBid !=0)
                {
                    bscToFind = bscToFind.Where(b => b.BlendId == vm.searchBid);
                }
                if(!string.IsNullOrWhiteSpace(vm.searchConditioner))
                {
                    bscToFind = bscToFind.Where(b => EF.Functions.Like(b.ConditionerOrganization.Name, "%" + vm.searchConditioner + "%") || b.ConditionerOrganizationId.ToString() == vm.searchConditioner);
                }
                if(vm.searchYearsCreated != null && vm.searchYearsCreated.Count > 0)
                {
                    bscToFind = bscToFind.Where(b => vm.searchYearsCreated.Contains(b.Date.Year));
                }

                
                var viewModel = new AdminBulkSalesCertificateSearchViewModel
                {
                    bsc = await bscToFind.ToListAsync(),  
                    programTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync(),
                    creationYears =  CertYearFinder.certYearList,
                };  
                return viewModel;


            }
            var freshModel = new AdminBulkSalesCertificateSearchViewModel
            {
                bsc = new List<BulkSalesCertificates>(),
                programTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync(),
                creationYears =  CertYearFinder.certYearList,
            };           

            return freshModel;
        }

        
    } 

     
    
}
