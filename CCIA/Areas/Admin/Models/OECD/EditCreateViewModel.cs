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
     
    public class AdminOECDEditCreateViewModel
    {
        public OECD oecd { get; set; } 
        
       public List<AbbrevOECDClass> classes { get; set; }

       public List<Countries> countries { get; set; }
               
        public static async Task<AdminOECDEditCreateViewModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id)
        {       
            var thisoecd = await _helper.FullOECD().Where(o => o.Id == id).FirstOrDefaultAsync();            
                       
            var model = new AdminOECDEditCreateViewModel
            {
                oecd = thisoecd,
                classes = await _dbContext.AbbrevOECDClass.OrderBy(x => x.SortOrder).ToListAsync(),
                countries = await _dbContext.Countries.OrderBy(x => x.Name).ToListAsync(),
            };

            return model;
        }

        
    }    
    
}
