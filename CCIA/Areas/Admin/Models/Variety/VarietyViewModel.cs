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
     
    public class AdminVarietyDetailsViewModel
    {
       public VarFull variety { get; set; }
       
      public List<VarFull> familyInfo { get; set; }
               
        public static async Task<AdminVarietyDetailsViewModel> Create(CCIAContext _dbContext, IFullCallService _helper, int id)
        {      
           var var = await _dbContext.VarFull
                .Include(v => v.Crop)
                .Include(v => v.VarietyOfficial)
                .ThenInclude(o => o.OwnerOrganization)
                .Include(v => v.VarietyOfficial)
                .ThenInclude(o => o.EcoRegionTranslate)
                .Include(v => v.VarietyOfficial)
                .ThenInclude(o => o.HarvestCounty)
                .Include(v => v.VarietyOfficial)
                .ThenInclude(o => o.StateHarvested)
                .Include(v => v.VarietyFamily)
                .Include(v => v.Countries.OrderBy(v => v.Country.Name))
                .ThenInclude(vc => vc.Country)
                .Where(v => v.Id == id).FirstOrDefaultAsync();           
                       
            var model = new AdminVarietyDetailsViewModel
            {
                variety = var,
                familyInfo = await _dbContext.VarFull.Where(v => v.Id== id || v.ParentId == var.ParentId).ToListAsync(),                
            };

            return model;
        }

        
    }    
    
}
