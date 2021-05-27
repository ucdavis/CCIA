using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Models
{   
     
    public class AdminBSeedTransfersEditViewModel
    {
       public SeedTransfers stc { get; set; }

       public List<Countries> countries { get; set;}  

       public List<County> counties { get; set;} 

       public List<StateProvince> stateProvinces { get; set; }

     

        public static async Task<AdminBSeedTransfersEditViewModel> Create(CCIAContext _dbContext, int id, IFullCallService _helper)
        {    
            var thisSTC = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync(); 
           
            var states = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();            
            states.Insert(0, new StateProvince{ StateProvinceId = 0, Name=""});           
            
            var countryList = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();            
            countryList.Insert(0, new Countries{ Id = 0, Name = ""});

            var countyList = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync();            
            countyList.Insert(0, new County{ CountyId = 0, Name = "Outside California"});             
     

            var model = new AdminBSeedTransfersEditViewModel
                {
                    stc = thisSTC,
                    countries = countryList,
                    stateProvinces = states,
                    counties = countyList,
                };
            
            return model;
        }
    }    
}
