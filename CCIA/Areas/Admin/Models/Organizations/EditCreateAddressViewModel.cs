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
       public OrganizationAddress orgAddress {get; set;}

       public int OrgId { get; set; } 

       public string OrgName { get; set; }
        
       public List<Countries> countries { get; set; }

       public List<County> counties { get; set; }

       public List<StateProvince> states { get; set; }
               
        public static async Task<AdminAddressEditCreateViewModel> Create(CCIAContext _dbContext, int id, int orgId = 0)
        {
            var thisOrgAddress = await _dbContext.OrganizationAddress.Where(a => a.Id == id)
                .Include(a => a.Address)       
                .ThenInclude(a => a.Countries)
                .Include(oa => oa.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(oa => oa.Address)
                .ThenInclude(a => a.County)
                .FirstOrDefaultAsync();                         

            var orgSearch = _dbContext.Organizations.Where(o => o.Id == thisOrgAddress.OrgId).AsQueryable();// .FirstOrDefaultAsync();
            if(orgId != 0)
            {
                orgSearch = _dbContext.Organizations.Where(o => o.Id == orgId).AsQueryable();
            }
            var org = await orgSearch.FirstOrDefaultAsync();
            var countyList = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(x => x.Name).ToListAsync();
            countyList.Add(new County{CountyId = 0, Name= "Outside California"});
            if(thisOrgAddress == null && orgId != 0)
            {
                thisOrgAddress = new OrganizationAddress();
                thisOrgAddress.Address = new Address();
                thisOrgAddress.OrgId = orgId;
            }
                       
            var model = new AdminAddressEditCreateViewModel
            {
                orgAddress = thisOrgAddress,                
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
