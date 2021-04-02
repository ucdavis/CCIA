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
     
    public class AdminAddressEditCreateViewModel
    {
       public Address address { get; set; }

       public int OrgId { get; set; } 

       public string OrgName { get; set; }
        
       public List<Countries> countries { get; set; }

       public List<County> counties { get; set; }

       public List<StateProvince> states { get; set; }
               
        public static async Task<AdminAddressEditCreateViewModel> Create(CCIAContext _dbContext, int id)
        {       
             var thisAddress = await _dbContext.Address.Where(a => a.Id == id)
                .Include(a => a.Countries)
                .Include(a => a.StateProvince)
                .Include(a => a.County)
                .FirstOrDefaultAsync();

            var org = await _dbContext.Organizations.Where(o => o.AddressId == id).FirstOrDefaultAsync();
            var countyList = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(x => x.Name).ToListAsync();
            countyList.Add(new County{CountyId = 0, Name= "Outside California"});
                       
            var model = new AdminAddressEditCreateViewModel
            {
                address = thisAddress,                
                countries = await _dbContext.Countries.OrderBy(x => x.Name).ToListAsync(),
                counties = countyList,
                states = await _dbContext.StateProvince.OrderBy(x => x.Name).ToListAsync(),
                OrgId = org.Id, 
                OrgName = org.Name
            };

            return model;
        }

        
    }    
    
}
