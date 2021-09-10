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
                
        [Display(Name="SID")]
        public int? sid { get; set; }
        
        [Display(Name="Submitted Year")]
        public List<int> submittedYearsToSearch { get; set; }
        
        [Display(Name="Cert Year")]
        public List<int> certYearsToSearch { get; set; }

        public List<int> yearsToSelectFrom { get; set; }
        
        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> searchCrops { get; set; }
        
        [Display(Name="Lot Number")]
        public string lotNumber { get; set; }

        public string appId { get; set; }              

        public List<string> statusOptions { get; set; } 
        
        [Display(Name="Status")]
        public List<string> searchStatus { get; set; }       

        [Display(Name="Applicant")]
        public string applicantName { get; set; }
        
        [Display(Name="Conditioner")]
        public string conditionerName { get; set; }

        [Display(Name="Variety")]
        public string variety { get; set; }

      
        public List<AbbrevAppType> AppTypes { get; set; }

        [Display(Name="Program")]
        public string ProgramToSearch { get; set; }



           
        

        public AdminSeedSearchViewModel() {
            Search = false;
        }
        
        public static async Task<AdminSeedSearchViewModel> Create(CCIAContext _dbContext, AdminSeedSearchViewModel vm)
        {   
            // var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeId).ToListAsync();
            // appTypes.Add(new AbbrevAppType { AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "Any"});

            var appTypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync();
            appTypes.Insert(0, new AbbrevAppType{AppTypeId = 0, AppTypeTrans="Any", Abbreviation = "%"});
                              
            if(vm != null)
            {
                var seedToFind = _dbContext.Seeds
                    .Include(s => s.ConditionerOrganization)
                    .Include(s => s.ApplicantOrganization)
                    .Include(s => s.AppTypeTrans)
                    .Include(s => s.Variety)
                    .ThenInclude(v => v.Crop)
                    .Include(s => s.ClassProduced)
                    .Include(s => s.LabResults) 
                    .ThenInclude(l => l.LabOrganization)                   
                    .Include(s => s.Application)
                    .ThenInclude(a => a.Crop)
                    .AsQueryable(); 
                if(vm.sid.HasValue)
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.Id.ToString(), "%" +  vm.sid.ToString() + "%"));
                }
                if(vm.submittedYearsToSearch != null && vm.submittedYearsToSearch.Any())
                {
                    seedToFind = seedToFind.Where(s => vm.submittedYearsToSearch.Contains(s.YearConfirmed));
                }
                if(vm.certYearsToSearch != null && vm.certYearsToSearch.Any())
                {
                    seedToFind = seedToFind.Where(s => vm.certYearsToSearch.Contains(s.CertYear.Value));
                }
                if(!string.IsNullOrWhiteSpace(vm.conditionerName))
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.ConditionerOrganization.Name, "%" + vm.conditionerName + "%") || s.ConditionerId.ToString() == vm.conditionerName);
                }
                if(!string.IsNullOrWhiteSpace(vm.applicantName))
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.ApplicantOrganization.Name, "%" + vm.applicantName + "%") || s.ApplicantId.ToString() == vm.applicantName);
                }
                if(vm.searchCrops != null && vm.searchCrops.Any())
                {
                    seedToFind = seedToFind.Where(s => vm.searchCrops.Contains(s.Variety.CropId) || vm.searchCrops.Contains(s.Application.CropId.Value));
                }
                if(!string.IsNullOrWhiteSpace(vm.variety))
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.Variety.Name, "%" + vm.variety + "%"));
                }
                if(!string.IsNullOrWhiteSpace(vm.lotNumber))
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.LotNumber, "%" + vm.lotNumber.Trim() + "%"));
                }
                if(!string.IsNullOrWhiteSpace(vm.appId))
                {
                    seedToFind = seedToFind.Where(s => EF.Functions.Like(s.AppId.Value.ToString(), "%" + vm.appId.Trim() + "%"));
                }
                if(vm.ProgramToSearch != "%")
                {
                    seedToFind = seedToFind.Where(s => s.CertProgram == vm.ProgramToSearch);
                }
                
                if(vm.searchStatus != null && vm.searchStatus.Any())              
                {
                    seedToFind = seedToFind.Where(s => vm.searchStatus.Contains(s.Status));
                }
                
                var viewModel = new AdminSeedSearchViewModel
                {
                    seeds = await seedToFind.ToListAsync(),  
                    yearsToSelectFrom = CertYearFinder.certYearListReverse, 
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                    statusOptions = EnumHelper.GetListOfDisplayNames<SeedsStatus>(),
                    AppTypes = appTypes,
                    
                };  
                return viewModel;


            }
            var freshModel = new AdminSeedSearchViewModel
            {
                seeds = new List<Seeds>(), 
                yearsToSelectFrom = CertYearFinder.certYearListReverse,              
                crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                statusOptions = EnumHelper.GetListOfDisplayNames<SeedsStatus>(),
                AppTypes = appTypes,
            };           

            return freshModel;
        }

        
    } 
    
}
