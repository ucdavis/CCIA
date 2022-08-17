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
    public class AgCommSearchViewModel
    {
        public List<Applications> apps { get; set; }

        public bool Search { get; set; }
        
        [Display(Name="App ID")]
        public int? appId { get; set; }

        [Display(Name="Cert Year")]
        public int? CertYear { get; set; }

        public List<Crops> crops { get; set; }

        [Display(Name="Crop(s)")]
        public List<int> searchCrops { get; set; }

        [Display(Name="Applicant")]
        public string applicantName { get; set; }

        [Display(Name="Grower")]
        public string growerName { get; set; }

                
        public AgCommSearchViewModel() {
            Search = false;
        }
        
        public static async Task<AgCommSearchViewModel> Create(CCIAContext _dbContext, AgCommSearchViewModel vm, IFullCallService _helper, int countyId)
        {                                             
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
                appsToFind = appsToFind.Where(a => a.FarmCounty == countyId && a.Approved && a.Status != ApplicationStatus.ApplicationCancelled.GetDisplayName());
                
                
                var viewModel = new AgCommSearchViewModel
                {
                    apps = await appsToFind.ToListAsync(),   
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                };  
                return viewModel;


            }
            var freshModel = new AgCommSearchViewModel
            {
                apps = new List<Applications>(),
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),                
                CertYear = CertYearFinder.CertYear,
            };           

            return freshModel;
        }

        
    } 
    
}
