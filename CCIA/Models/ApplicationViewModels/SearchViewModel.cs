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
     public enum SearchOptions
    { 
        No,
        Yes,
        Both        
    }    
    public class AdminSearchViewModel
    {
        public List<Applications> apps { get; set; }

        public bool Search { get; set; }
        
        [Display(Name="App ID")]
        public int? appId { get; set; }

        [Display(Name="Cert Year")]
        public int? CertYear { get; set; }

        public List<AbbrevAppType> appTypes {get; set; }

        [Display(Name="App Type")]
        public string appType { get; set; }

        public List<Crops> crops { get; set; }

        [Display(Name="Crop(s)")]
        public List<int> searchCrops { get; set; }

        public List<County> counties { get; set; }

        [Display(Name="County(ies)")]
        public List<int> searchCounties { get; set; }

        public List<string> statusOptions { get; set; } 

        [Display(Name="Status")]
        public List<string> searchStatus { get; set; }       

        [Display(Name="Applicant")]
        public string applicantName { get; set; }

        [Display(Name="Grower")]
        public string growerName { get; set; }

        
        [Display(Name="Variety")]
        public string variety { get; set; }

        [Display(Name="Cert#")]
        public int? certNumber { get; set; }

        [Display(Name="Accepted?")]
        public int accepted { get; set; }

        [Display(Name="Follow-up?")]
        public int followUp { get; set; }

        [Display(Name="Cancelled?")]
        public int cancelled { get; set; }

        [Display(Name="CCIA Map?")]
        public int veMap { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Planted Before")]
        public DateTime? plantedBefore { get; set; }

        [DataType(DataType.Date)]
        [Display(Name="Planted After")]
        public DateTime? plantedAfter { get; set; }

        [Display(Name="Include Map Options?")]
        public bool includeMapOptions { get; set; }

        

        public AdminSearchViewModel() {
            Search = false;
        }
        
        public static async Task<AdminSearchViewModel> Create(CCIAContext _dbContext, AdminSearchViewModel vm, IFullCallService _helper)
        {   
            var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeId).ToListAsync();
            appTypes.Add(new AbbrevAppType { AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "Any"});
                              
            if(vm != null)
            {
                var appsToFind = _helper.OverviewApplications();
                if(vm.appId.HasValue)
                {
                    appsToFind = appsToFind.Where(a => a.Id == vm.appId);
                }
                if(vm.CertYear.HasValue)
                {
                    appsToFind = appsToFind.Where(a => a.CertYear == vm.CertYear);
                }
                if(!string.IsNullOrWhiteSpace(vm.appType))
                {
                    appsToFind = appsToFind.Where(a => a.AppType == vm.appType || vm.appType == "Any");
                }
                if(!string.IsNullOrWhiteSpace(vm.applicantName))
                {
                    appsToFind = appsToFind.Where(a => EF.Functions.Like(a.ApplicantOrganization.Name, "%" + vm.applicantName + "%") || a.ApplicantId.ToString() == vm.applicantName);
                }
                if(!string.IsNullOrWhiteSpace(vm.growerName))
                {
                    appsToFind = appsToFind.Where(a => EF.Functions.Like(a.GrowerOrganization.Name, "%" + vm.growerName + "%") || a.GrowerId.ToString() == vm.growerName);
                }
                if(vm.searchCrops != null && vm.searchCrops.Count > 0)
                {
                    appsToFind = appsToFind.Where(a => vm.searchCrops.Contains(a.CropId.Value));
                }
                if(vm.searchCounties != null && vm.searchCounties.Count > 0)
                {
                    appsToFind = appsToFind.Where(a => vm.searchCounties.Contains(a.FarmCounty));
                }
                if(!string.IsNullOrWhiteSpace(vm.variety))
                {
                    appsToFind = appsToFind.Where(a => EF.Functions.Like(a.Variety.Name, "%" + vm.variety + "%"));
                }
                if(vm.certNumber.HasValue)
                {
                    appsToFind = appsToFind.Where(a => a.CertNum == vm.certNumber);
                }
                if(vm.searchStatus != null && vm.searchStatus.Count > 0)
                {                    
                    appsToFind = appsToFind.Where(a => vm.searchStatus.Contains(a.Status));
                }
                if(vm.accepted != 2)
                {
                    appsToFind = appsToFind.Where(a => (a.Approved && vm.accepted == 1) || (!a.Approved && vm.accepted == 0));
                }
                if(vm.followUp != 2)
                {
                    appsToFind = appsToFind.Where(a => (a.FollowUp && vm.followUp == 1) || (!a.FollowUp && vm.followUp == 0));
                }
                if(vm.cancelled != 2)
                {
                    appsToFind = appsToFind.Where(a => (a.Cancelled && vm.cancelled == 1) || (!a.Cancelled && vm.cancelled == 0));
                }
                if(vm.veMap != 2)
                {
                    appsToFind = appsToFind.Where(a => (a.MapVe && vm.veMap == 1) || (!a.MapVe && vm.veMap == 0));
                }
                if(vm.plantedAfter != null)
                {
                    appsToFind = appsToFind.Where(a => a.DatePlanted >= vm.plantedAfter);
                }
                if(vm.plantedBefore != null)
                {
                    appsToFind = appsToFind.Where(a => a.DatePlanted <= vm.plantedBefore);
                }
                
                var viewModel = new AdminSearchViewModel
                {
                    apps = await appsToFind.ToListAsync(),                   
                    appTypes = appTypes,
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                    statusOptions = EnumHelper.GetListOfDisplayNames<ApplicationStatus>(),
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(), 
                    includeMapOptions = vm.includeMapOptions,                   
                };  
                return viewModel;


            }
            var freshModel = new AdminSearchViewModel
            {
                apps = new List<Applications>(),
                appTypes = appTypes,
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                statusOptions = EnumHelper.GetListOfDisplayNames<ApplicationStatus>(),
                CertYear = CertYearFinder.CertYear,
                accepted = 2,
                followUp = 2,
                cancelled = 0,
                veMap = 2, 
                includeMapOptions = false,               
                counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
            };           

            return freshModel;
        }

        
    } 
    
}
