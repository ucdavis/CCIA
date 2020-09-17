using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;


namespace CCIA.Models.DetailsViewModels
{   
    public class AdminSearchViewModel
    {
        public List<Applications> apps { get; set; }

        public bool Search { get; set; }
        
        [DisplayName("App ID")]
        public int? appId { get; set; }

        [DisplayName("Cert Year")]
        public int? CertYear { get; set; }

        public List<AbbrevAppType> appTypes {get; set; }

        [DisplayName("App Type")]
        public string appType { get; set; }

        public List<Crops> crops { get; set; }

        [DisplayName("Crop(s)")]
        public List<int> searchCrops { get; set; }

        public List<string> statusOptions { get; set; } 

        public List<string> searchStatus { get; set; }       

        [DisplayName("Applicant")]
        public string applicantName { get; set; }

        [DisplayName("Grower")]
        public string growerName { get; set; }

        
        [DisplayName("Variety (official)")]
        public string variety { get; set; }

        [DisplayName("Cert Number")]
        public int? certNumber { get; set; }

        public AdminSearchViewModel() {
            Search = false;
        }
        
        public static async Task<AdminSearchViewModel> Create(CCIAContext _dbContext, AdminSearchViewModel vm)
        {   
            var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeId).ToListAsync();
            appTypes.Add(new AbbrevAppType { AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "Any"});
                              
            if(vm != null)
            {
                var appsToFind = _dbContext.Applications
                    .Include(a => a.ApplicantOrganization)
                    .Include(a => a.GrowerOrganization)
                    .Include(a => a.Variety)
                    .AsQueryable(); 
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
                    appsToFind = appsToFind.Where(a => EF.Functions.Like(a.ApplicantOrganization.NameAndId, "%" + vm.applicantName + "%"));
                }
                if(!string.IsNullOrWhiteSpace(vm.growerName))
                {
                    appsToFind = appsToFind.Where(a => EF.Functions.Like(a.GrowerOrganization.NameAndId, "%" + vm.growerName + "%"));
                }
                if(vm.searchCrops != null && vm.searchCrops.Count > 0)
                {
                    appsToFind = appsToFind.Where(a => vm.searchCrops.Contains(a.CropId.Value));
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
                var viewModel = new AdminSearchViewModel
                {
                    apps = await appsToFind.ToListAsync(),                   
                    appTypes = appTypes,
                    crops = await _dbContext.Crops.OrderBy(c => c.Name).ToListAsync(),
                    statusOptions = EnumHelper.GetListOfDisplayNames<ApplicationStatus>(),                    
                };  
                return viewModel;


            }
            var freshModel = new AdminSearchViewModel
            {
                apps = new List<Applications>(),
                appTypes = appTypes,
                crops = await _dbContext.Crops.OrderBy(c => c.Name).ToListAsync(),
                statusOptions = EnumHelper.GetListOfDisplayNames<ApplicationStatus>(),
            };           

            return freshModel;
        }

        
    } 
    
}
