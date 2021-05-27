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
     
    public class AdminBlendsSearchViewModel
    {
        public List<BlendRequests> blends { get; set; }                     
       
        public List<String> blendTypes {get; set;} 

        public string typeSearchValue { get; set; }
        
        public string conditionerSearchTerm { get; set; }           

         public List<string> statusOptions { get; set; } 

        [Display(Name="Status")]
        public List<string> searchStatus { get; set; }       



               
        public static async Task<AdminBlendsSearchViewModel> Create(CCIAContext _dbContext, AdminBlendsSearchViewModel vm, IFullCallService helper)
        {               
            var typeList = new List<string> { "Any", "Lot", "Varietal", "In Dirt"}; 

            if(vm != null)
            {
                var blendsToFind = helper.FullBlendRequest().AsQueryable();

               if(vm.searchStatus != null && vm.searchStatus.Count > 0)
                {                    
                    blendsToFind = blendsToFind.Where(b => vm.searchStatus.Contains(b.Status));
                } 
                if(vm.typeSearchValue != "Any")             
                {
                    blendsToFind = blendsToFind.Where(b => b.BlendType == vm.typeSearchValue);
                }
                if(!string.IsNullOrWhiteSpace(vm.conditionerSearchTerm))
                {
                    blendsToFind = blendsToFind.Where(b => EF.Functions.Like(b.Conditioner.Name, "%" + vm.conditionerSearchTerm + "%") || b.ConditionerId.ToString() == vm.conditionerSearchTerm);
                }
                
                
                var viewModel = new AdminBlendsSearchViewModel
                {
                    blends = await blendsToFind.ToListAsync(),  
                    blendTypes = typeList,
                    typeSearchValue = vm.typeSearchValue,
                    statusOptions = EnumHelper.GetListOfDisplayNames<BlendStatus>(),
                };  
                return viewModel;


            }
            var freshModel = new AdminBlendsSearchViewModel
            {
                blends = new List<BlendRequests>(),
                blendTypes = typeList,
                typeSearchValue = "Any",
                statusOptions = EnumHelper.GetListOfDisplayNames<BlendStatus>(),
            };           

            return freshModel;
        }

        
    } 

     
    
}
