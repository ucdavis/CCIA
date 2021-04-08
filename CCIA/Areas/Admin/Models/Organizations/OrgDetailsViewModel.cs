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
     
    public class AdminOrgDetailsViewModel
    {
       public Organizations org { get; set; }
       
       public List<Maps> maps { get; set; }
               
        public static async Task<AdminOrgDetailsViewModel> Create(CCIAContext _dbContext, IFullCallService _helper, int id)
        {      
            var organization = await _helper.FullOrg().Where(o => o.Id == id).FirstOrDefaultAsync();
                       
            var model = new AdminOrgDetailsViewModel
            {
                org = organization,
                maps = await _dbContext.Maps.OrderBy(m => m.Name).ToListAsync()
            };

            return model;
        }

        
    }    
    
}
