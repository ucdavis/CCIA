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
                if(!string.IsNullOrWhiteSpace(vm.conditionerName))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.ConditionerOrganization.Name, "%" + vm.conditionerName + "%") || s.ConditionerId.ToString() == vm.conditionerName);
                }
                if(!string.IsNullOrWhiteSpace(vm.shipperName))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.ShipperOrganization.Name, "%" + vm.shipperName + "%") || s.ShipperId.ToString() == vm.shipperName);
                }
                if(vm.searchCrops != null && vm.searchCrops.Any())
                {
                    oecdToFind = oecdToFind.Where(s => vm.searchCrops.Contains(s.Variety.CropId));
                }
                if(!string.IsNullOrWhiteSpace(vm.variety))
                {
                    oecdToFind = oecdToFind.Where(s => EF.Functions.Like(s.Variety.Name, "%" + vm.variety + "%"));
                }                
                if(!string.IsNullOrWhiteSpace(vm.searchClass) && vm.searchClass != "Any")              
                {
                    oecdToFind = oecdToFind.Where(s => s.Class.Class == vm.searchClass);
                }
                
                var viewModel = new AdminOECDSearchViewModel
                {
                    oecds = await oecdToFind.ToListAsync(),  
                    yearsToSelectFrom = CertYearFinder.certYearListReverse, 
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c=> c.CropKind).ToListAsync(),
                    classOptions = classList,
                    
                };  
                return viewModel;


            }
            var freshModel = new AdminOECDSearchViewModel
            {
                oecds = new List<OECD>(), 
                yearsToSelectFrom = CertYearFinder.certYearListReverse,              
                crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                classOptions = classList,
            };           

            return freshModel;
        }

        
    }    
    
}
