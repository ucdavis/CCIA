using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;

namespace CCIA.Models.AgComm
{   
     
    public class AgCommSeedTransferSearchViewModel
    {
        public List<SeedTransfers> stc { get; set; } 

        public int searchId { get; set; } 

        public int approvedStatus { get; set; }

        public List<int> creationYears { get; set; }

        public List<int> searchYearsCreated { get; set; }

        public string searchConditioner { get; set; }

        public int searchSid { get; set; }

        public int searchBid { get; set; }

        public int searchAppId { get; set; }


               
        public static async Task<AgCommSeedTransferSearchViewModel> Create(CCIAContext _dbContext, AgCommSeedTransferSearchViewModel vm, IFullCallService helper, int countyId)
        {               
            

            if(vm != null)
            {
                var stcToFind = helper.FullSeedTransfers().AsQueryable();

                if(vm.searchSid != 0)
                {
                    stcToFind = stcToFind.Where(b => EF.Functions.Like(b.SeedsID.ToString(), "%" + vm.searchSid + '%'));
                }
                if(vm.searchBid !=0)
                {
                    stcToFind = stcToFind.Where(b => EF.Functions.Like(b.BlendId.ToString(), "%" + vm.searchBid + '%'));
                }
                if(vm.searchAppId !=0)
                {
                    stcToFind = stcToFind.Where(b => EF.Functions.Like(b.ApplicationId.ToString(), "%" + vm.searchAppId + '%'));
                }
                if(vm.searchId !=0)
                {
                    stcToFind = stcToFind.Where(b => EF.Functions.Like(b.Id.ToString(), "%" + vm.searchId + '%'));                    
                }
                if(vm.searchYearsCreated != null && vm.searchYearsCreated.Count > 0)
                {
                    stcToFind = stcToFind.Where(st => vm.searchYearsCreated.Contains(st.CertificateDate.Year));
                }
                if(vm.approvedStatus != 2)
                {
                    stcToFind = stcToFind.Where(st => (st.AgricultureCommissionerApprove && vm.approvedStatus == 1) || (!st.AgricultureCommissionerApprove && vm.approvedStatus == 0));
                }
                if(!string.IsNullOrWhiteSpace(vm.searchConditioner))
                {
                    stcToFind = stcToFind.Where(st => EF.Functions.Like(st.OriginatingOrganization.Name, "%" + vm.searchConditioner + "%") || EF.Functions.Like(st.OriginatingOrganizationId.ToString(), "%" + vm.searchConditioner + "%"));
                }
                
                stcToFind = stcToFind.Where(t => t.OriginatingCountyId == countyId || t.PurchaserCountyId == countyId);
               
                
                
                var viewModel = new AgCommSeedTransferSearchViewModel
                {
                    stc = await stcToFind.ToListAsync(),  
                    creationYears =  CertYearFinder.certYearListReverse,
                };  
                return viewModel;


            }
            var freshModel = new AgCommSeedTransferSearchViewModel
            {
                stc = new List<SeedTransfers>(),
                creationYears =  CertYearFinder.certYearListReverse,
            };           

            return freshModel;
        }

        
    } 

     
    
}
