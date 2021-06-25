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

      public List<Crops> crops { get; set; }

      public List<string> categories { get; set; }

      public List<string> statuses { get; set;}

      public List<Ecoregions> ecoregions { get; set; }

      public List<County> counties { get; set; }

      public List<StateProvince> states { get; set; }

      public List<string> types { get; set; }
               
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

        public static async Task<AdminVarietyDetailsViewModel> Edit(CCIAContext _dbContext, IFullCallService _helper, int id)
        {      
           var varietySearch = await _dbContext.VarFull
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

            var countyList = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync();
            countyList.Insert(0, new County {CountyId = 0, Name ="NA"});

            var stateList = await _dbContext.StateProvince.Where(s => s.CountryId == 58).OrderBy(s => s.Name).ToListAsync();
            stateList.Insert(0, new StateProvince {StateProvinceId = 0, Name = "NA"});
                       
            var model = new AdminVarietyDetailsViewModel
            {
                variety = varietySearch,                
                crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).Select(c => new Crops { CropId = c.CropId, Crop = c.Crop, CropKind = c.CropKind }).ToListAsync(),
                categories = new List<string> { "Proprietary", "Public" },
                statuses = new List<string> {  "Certified", "Pending"}, 
                ecoregions = await _dbContext.Ecoregions.ToListAsync(),
                counties = countyList,
                states = stateList,
                types = EnumHelper.GetListOfDisplayNames<VarietyTypes>(),
            };

            return model;
        }

        
    }    
    
}
