using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;

namespace CCIA.Models
{   
     
    public class AdminSeedSearchViewModel
    {
        public List<Seeds> seeds { get; set; }

        public bool Search { get; set; }
        
        [DisplayName("SID")]
        public int? sid { get; set; }

        [DisplayName("Submitted Year")]
        public List<int> submittedYearsToSearch { get; set; }

        [DisplayName("Cert Year")]
        public List<int> certYearsToSearch { get; set; }

        public List<int> certYearsToSelectFrom { get; set; }
       
        
        public List<Crops> crops { get; set; }

        [DisplayName("Crop(s)")]
        public List<int> searchCrops { get; set; }
        

        public string lotNumber { get; set; }

        public int AppId { get; set; }              

        public List<string> statusOptions { get; set; } 

        [DisplayName("Status")]
        public List<string> searchStatus { get; set; }       

        [DisplayName("Applicant")]
        public string applicantName { get; set; }

        [DisplayName("Conditioner")]
        public string conditionerName { get; set; }

        
        [DisplayName("Variety")]
        public string variety { get; set; }

        [DisplayName("Cert#")]
        public int? certNumber { get; set; }       
        

        public AdminSeedSearchViewModel() {
            Search = false;
        }
        
        public static async Task<AdminSeedSearchViewModel> Create(CCIAContext _dbContext, AdminSeedSearchViewModel vm)
        {   
            // var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeId).ToListAsync();
            // appTypes.Add(new AbbrevAppType { AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "Any"});
                              
            if(vm != null)
            {
                var seedToFind = _dbContext.Seeds
                    .Include(c => c.ConditionerOrganization)
                    .Include(c => c.AppTypeTrans)
                    .Include(v => v.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(c => c.ClassProduced)
                    .Include(l => l.LabResults) 
                    .ThenInclude(l => l.LabOrganization)                   
                    .Include(s => s.Application)
                    .ThenInclude(a => a.Crop)
                    .Include(s => s.OECDForm)
                    .AsQueryable(); 
                if(vm.sid.HasValue)
                {
                    seedToFind = seedToFind.Where(s => s.Id == vm.sid);
                }
                // if(vm.submittedYearsToSearch.Any())
                // {
                //     seedToFind = seedToFind.Where(s => vm.submittedYearsToSearch.Contains(s.YearConfirmed));
                // }

                // if(!string.IsNullOrWhiteSpace(vm.appType))
                // {
                //     seedToFind = seedToFind.Where(a => a.AppType == vm.appType || vm.appType == "Any");
                // }
                // if(!string.IsNullOrWhiteSpace(vm.applicantName))
                // {
                //     seedToFind = seedToFind.Where(a => EF.Functions.Like(a.ApplicantOrganization.NameAndId, "%" + vm.applicantName + "%"));
                // }
                // if(!string.IsNullOrWhiteSpace(vm.growerName))
                // {
                //     seedToFind = seedToFind.Where(a => EF.Functions.Like(a.GrowerOrganization.NameAndId, "%" + vm.growerName + "%"));
                // }
                // if(vm.searchCrops != null && vm.searchCrops.Count > 0)
                // {
                //     seedToFind = seedToFind.Where(a => vm.searchCrops.Contains(a.CropId.Value));
                // }
                // if(vm.searchCounties != null && vm.searchCounties.Count > 0)
                // {
                //     seedToFind = seedToFind.Where(a => vm.searchCounties.Contains(a.FarmCounty));
                // }
                // if(!string.IsNullOrWhiteSpace(vm.variety))
                // {
                //     seedToFind = seedToFind.Where(a => EF.Functions.Like(a.Variety.Name, "%" + vm.variety + "%"));
                // }
                // if(vm.certNumber.HasValue)
                // {
                //     seedToFind = seedToFind.Where(a => a.CertNum == vm.certNumber);
                // }
                // if(vm.searchStatus != null && vm.searchStatus.Count > 0)
                // {                    
                //     seedToFind = seedToFind.Where(a => vm.searchStatus.Contains(a.Status));
                // }
                // if(vm.accepted != 2)
                // {
                //     seedToFind = seedToFind.Where(a => (a.Approved && vm.accepted == 1) || (!a.Approved && vm.accepted == 0));
                // }
                // if(vm.cancelled != 2)
                // {
                //     seedToFind = seedToFind.Where(a => (a.Cancelled && vm.cancelled == 1) || (!a.Cancelled && vm.cancelled == 0));
                // }
                // if(vm.veMap != 2)
                // {
                //     seedToFind = seedToFind.Where(a => (a.MapVe && vm.veMap == 1) || (!a.MapVe && vm.veMap == 0));
                // }
                // if(vm.plantedAfter != null)
                // {
                //     seedToFind = seedToFind.Where(a => a.DatePlanted >= vm.plantedAfter);
                // }
                // if(vm.plantedBefore != null)
                // {
                //     seedToFind = seedToFind.Where(a => a.DatePlanted <= vm.plantedBefore);
                // }
                
                var viewModel = new AdminSeedSearchViewModel
                {
                    seeds = await seedToFind.ToListAsync(),                   
                   
                    //crops = await _dbContext.Crops.OrderBy(c => c.Name).ToListAsync(),
                    //statusOptions = EnumHelper.GetListOfDisplayNames<ApplicationStatus>(),
                    //counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(), 
                    
                };  
                return viewModel;


            }
            var freshModel = new AdminSeedSearchViewModel
            {
                seeds = new List<Seeds>(),               
            };           

            return freshModel;
        }

        
    } 
    
}
