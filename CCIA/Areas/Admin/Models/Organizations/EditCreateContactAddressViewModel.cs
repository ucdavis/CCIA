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
     
    public class AdminContactAddressEditCreateViewModel
    {
       public ContactAddress address { get; set; } 

       public string ContactName { get; set; } 

       public int ContactId { get; set; }
        
       public List<Countries> countries { get; set; }

       public List<County> counties { get; set; }

       public List<StateProvince> states { get; set; }
               
        public static async Task<AdminContactAddressEditCreateViewModel> Create(CCIAContext _dbContext, int id, int contactId = 0)
        {       
             var thisAddress = await _dbContext.ContactAddress.Where(a => a.Id == id)
                .Include(c => c.Address)
                .ThenInclude(a => a.Countries)
                .Include(c => c.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(c => c.Address)
                .ThenInclude(a => a.County)
                .FirstOrDefaultAsync();

            var countyList = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(x => x.Name).ToListAsync();
            countyList.Add(new County{CountyId = 0, Name= "Outside California"});
            var contactSearch = _dbContext.Contacts.Where(c => c.Id == thisAddress.ContactId).AsQueryable();// .FirstOrDefaultAsync();
            if(contactId != 0)
            {
                contactSearch = _dbContext.Contacts.Where(c => c.Id == contactId).AsQueryable();
            } else
            {
                contactId = thisAddress.ContactId;
            }
            var contact = await contactSearch.Select(c => c.FirstName + ' ' + c.LastName).FirstOrDefaultAsync();
            
                       
            var model = new AdminContactAddressEditCreateViewModel
            {
                address = thisAddress,                
                countries = await _dbContext.Countries.OrderBy(x => x.Name).ToListAsync(),
                counties = countyList,
                states = await _dbContext.StateProvince.OrderBy(x => x.Name).ToListAsync(),                
                ContactName = contact,
                ContactId = contactId,
            };

            return model;
        }

        
    }    
    
}
