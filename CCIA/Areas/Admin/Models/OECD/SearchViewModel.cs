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
     
    public class AdminOECDSearchViewModel
    {
        public List<OECD> oecds { get; set; }             
                
        [Display(Name="SID")]
        public int? sid { get; set; }

        
        [Display(Name="OECD ID")]
        public int? oecdId { get; set; }
        
        [Display(Name="Year Printed")]
        public List<int> submittedYearsToSearch { get; set; }
        
        [Display(Name="Cert Year")]
        public List<int> certYearsToSearch { get; set; }

        public List<int> yearsToSelectFrom { get; set; }
        [Display(Name="Follow-up?")]
        public int followUp { get; set; }
       
        
        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> searchCrops { get; set; }
                
        [Display(Name="Conditioner")]
        public string conditionerName { get; set; }
        
        [Display(Name="Shipper")]
        public string shipperName { get; set; }
              

        public List<AbbrevOECDClass> classOptions { get; set; } 

        [Display(Name="Class")]
        public string searchClass { get; set; }       
        
        
        [Display(Name="Variety")]
        public string variety { get; set; }
        [Display(Name ="Printed?")]
        public string PrintedToSearch { get; set; }
       

               
        public static async Task<AdminOECDSearchViewModel> Create(CCIAContext _dbContext, AdminOECDSearchViewModel vm)
        {  
            var classList = await _dbContext.AbbrevOECDClass.OrderBy(c => c.SortOrder).ToListAsync();
            classList.Insert(0, new AbbrevOECDClass { Id = 0, Class = "Any" });             
            if(vm != null)
            {
                var oecdToFind = _dbContext.OECD
                    .Include(o => o.Seeds)
                    .ThenInclude(s => s.ClassProduced)                    
                    .Include(o => o.Class)
                    .Include(o => o.ShipperOrganization)                                        
                    .Include(o => o.ConditionerOrganization)
                    .Include(o => o.Variety)
                    .ThenInclude(v => v.Crop)
                    .AsQueryable(); 
                if(vm.oecdId.HasValue)
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.Id.ToString(), "%" +  vm.oecdId.ToString() + "%"));
                }
                if(vm.sid.HasValue)
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.SeedsId.ToString(), "%" +  vm.sid.ToString() + "%"));
                }

                if(vm.submittedYearsToSearch != null && vm.submittedYearsToSearch.Any())
                {
                    oecdToFind = oecdToFind.Where(s => vm.submittedYearsToSearch.Contains(s.DatePrinted.Value.Year));
                }
                if(vm.certYearsToSearch != null && vm.certYearsToSearch.Any())
                {
                    oecdToFind = oecdToFind.Where(s => vm.certYearsToSearch.Contains(s.Seeds.CertYear.Value));
                }
                if(vm.followUp != 2)
                {
                    oecdToFind = oecdToFind.Where(a => (a.FollowUp && vm.followUp == 1) || (!a.FollowUp && vm.followUp == 0));
                }
                if(!string.IsNullOrWhiteSpace(vm.conditionerName))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.ConditionerOrganization.Name, "%" + vm.conditionerName.Trim() + "%") || s.ConditionerId.ToString() == vm.conditionerName.Trim());
                }
                if(!string.IsNullOrWhiteSpace(vm.shipperName))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.ShipperOrganization.Name, "%" + vm.shipperName.Trim() + "%") || s.ShipperId.ToString() == vm.shipperName.Trim());
                }
                if(vm.searchCrops != null && vm.searchCrops.Any())
                {
                    oecdToFind = oecdToFind.Where(s => vm.searchCrops.Contains(s.Variety.CropId));
                }
                if(!string.IsNullOrWhiteSpace(vm.variety))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.Variety.Name, "%" + vm.variety.Trim() + "%") || s.Variety.Id.ToString().Contains(vm.variety.Trim()));
                }                
                if(!string.IsNullOrWhiteSpace(vm.searchClass) && vm.searchClass != "Any")              
                {
                    oecdToFind = oecdToFind.Where(s => s.Class.Class == vm.searchClass);
                }
                if(!string.IsNullOrWhiteSpace(vm.PrintedToSearch) && vm.PrintedToSearch == "Printed")
                {
                    oecdToFind = oecdToFind.Where(o => o.DatePrinted.HasValue);
                }
                if (!string.IsNullOrWhiteSpace(vm.PrintedToSearch) && vm.PrintedToSearch == "Not Printed")
                {
                    oecdToFind = oecdToFind.Where(o => !o.DatePrinted.HasValue);
                }

                var viewModel = new AdminOECDSearchViewModel
                {
                    oecds = await oecdToFind.ToListAsync(),  
                    yearsToSelectFrom = CertYearFinder.certYearListReverse, 
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                    classOptions = classList,
                    
                };  
                return viewModel;


            }
            var freshModel = new AdminOECDSearchViewModel
            {
                oecds = new List<OECD>(), 
                yearsToSelectFrom = CertYearFinder.certYearListReverse,              
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                classOptions = classList,
                followUp = 2,
                PrintedToSearch = "Both",
            };           

            return freshModel;
        }

        
    }    
    
}
