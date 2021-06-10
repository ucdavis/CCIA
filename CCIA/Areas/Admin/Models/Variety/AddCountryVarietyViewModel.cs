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
     
    public class AdminAddCountryVarietyViewModel
    {
       public VarFull variety { get; set; }
       
      public List<Countries> countryList { get; set; }
               
        public static async Task<AdminAddCountryVarietyViewModel> Create(CCIAContext _dbContext, int id)
        {      
           var var = await _dbContext.VarFull.Where(v => v.Id == id).FirstOrDefaultAsync();           
           var currentCountries = await _dbContext.VarCountires.Where(c => c.VarId == id).Select(c => c.CountryId).ToListAsync();
                       
            var model = new AdminAddCountryVarietyViewModel
            {
                variety = var,
                countryList = await _dbContext.Countries.Where(c => !currentCountries.Contains(c.Id)).OrderBy(c => c.Name).ToListAsync(),                
            };

            return model;
        }

        
    }    
    
}
