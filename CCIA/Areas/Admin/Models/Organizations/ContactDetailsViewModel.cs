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
     
    public class AdminContactDetailsViewModel
    {
       public Contacts contact { get; set; }
       
       public List<Maps> maps { get; set; }

       
               
        public static async Task<AdminContactDetailsViewModel> Create(CCIAContext _dbContext, IFullCallService _helper, int id)
        {      
            
            var employee = await _helper.FullContact().Where(c => c.Id == id).FirstOrDefaultAsync();
            var orgMapPermissions = await _dbContext.OrgMaps.Where(p => p.OrgId == employee.OrgId).Select(p => p.Map).ToListAsync();

            var model = new AdminContactDetailsViewModel
            {
                contact = employee,
                maps = await _dbContext.Maps.Where(m => orgMapPermissions.Contains(m.Name)).OrderBy(m => m.Name).ToListAsync()
            };

            return model;
        }

        
    }    
    
}
