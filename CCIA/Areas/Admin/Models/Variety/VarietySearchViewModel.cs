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
     
    public class AdminVarietySearchViewModel
    {
       public List<VarFull> varieties { get; set; }

        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> searchCrops { get; set; }

        public int varietyIdToSearch { get; set; }

        public string varietyNameToSearch { get; set; }
        
         public List<string> typeOptions { get; set; } 
        
        [Display(Name="Variety Type")]
        public List<string> searchType { get; set; } 

        public List<string> statusOptions { get; set;}

        public string statusToSearch { get; set; }

        public List<string> categoryOptions { get; set;}

        public string categoryToSearch { get; set; }
       
      
               
        public static async Task<AdminVarietySearchViewModel> Create(CCIAContext _dbContext, IFullCallService _helper, AdminVarietySearchViewModel vm)
        {      
           var varietySearch = _helper.FullVariety().AsQueryable(); 

           if(vm != null)
           {
           
                // var = var.Where(v => v.Id == 13381); 
                if(vm.varietyIdToSearch != 0)
                {
                    varietySearch = varietySearch.Where(v => v.Id == vm.varietyIdToSearch);
                }
                if(!string.IsNullOrWhiteSpace(vm.varietyNameToSearch))
                {
                    varietySearch = varietySearch.Where(v => EF.Functions.Like(v.Name, "%" + vm.varietyNameToSearch.Trim() + "%"));
                }
                if(vm.searchCrops != null && vm.searchCrops.Count > 0)
                {
                    varietySearch = varietySearch.Where(v => vm.searchCrops.Contains(v.CropId));
                }
                if(vm.searchType != null && vm.searchType.Count > 0)
                {
                    varietySearch = varietySearch.Where(v => vm.searchType.Contains(v.Type));
                }
                if(vm.categoryToSearch != "Both")
                {
                    varietySearch = varietySearch.Where(v => v.Category == vm.categoryToSearch);
                }
                if(vm.statusToSearch != "Both")
                {
                    varietySearch = varietySearch.Where(v => v.Status == vm.statusToSearch);
                }
           } else
           {
               varietySearch = varietySearch.Where(v => v.Id == -1);
           }       

           
                       
            var model = new AdminVarietySearchViewModel
            {
                varieties = await varietySearch.ToListAsync(), 
                typeOptions = EnumHelper.GetListOfDisplayNames<VarietyTypes>(), 
                crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                statusOptions = new List<string> {  "Both", "Certified", "Pending"}, 
                categoryOptions = new List<string> { "Both","Proprietary", "Public" }, 
            };

            return model;
        }

        
    }    
    
}
