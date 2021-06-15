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
           var var = _helper.FullVariety().AsQueryable(); 
           
           var = var.Where(v => v.Id == 13381); 
           var typesList =  EnumHelper.GetListOfDisplayNames<VarietyTypes>();
           typesList.Add("Any");
                       
            var model = new AdminVarietySearchViewModel
            {
                varieties = await var.ToListAsync(), 
                typeOptions = typesList, 
                crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                statusOptions = new List<string> {  "Both", "Certified", "Pending"}, 
                categoryOptions = new List<string> { "Both","Proprietary", "Public" }, 
            };

            return model;
        }

        
    }    
    
}
