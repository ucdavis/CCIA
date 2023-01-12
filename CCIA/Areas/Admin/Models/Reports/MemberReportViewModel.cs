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
     
    public class AdminMemberReportViewModel
    {
       public List<Organizations> reports { get; set; }      

       
        public string showReport { get; set; }       
        
        public string memberTypeReport { get; set; }

        public string districtReport { get; set; }      
      
               
        public static async Task<AdminMemberReportViewModel> Create(CCIAContext _dbContext, AdminMemberReportViewModel vm )
        { 
            var reportsFound = _dbContext.Organizations
                .Include(o => o.RepresentativeContact)
                .Include(o => o.Addresses.Where(a => a.Active))
                .ThenInclude(a =>a.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(o => o.Addresses.Where(a=>a.Active))
                .ThenInclude(a =>a.Address)
                .ThenInclude(a => a.County)
                .AsQueryable(); 
            var model = new AdminMemberReportViewModel(); 
            if(vm.showReport != null)
            {
                if(vm.showReport != "Both")
                {
                    if(vm.showReport == "Members")
                    {
                        reportsFound = reportsFound.Where(o => o.Member);
                    } else
                    {
                         reportsFound = reportsFound.Where(o => !o.Member);
                    }                    
                }

                if(vm.memberTypeReport != "Any")
                {
                    reportsFound = reportsFound.Where(o => o.MemberType == vm.memberTypeReport);
                }

                if(vm.districtReport != "Any")
                {
                    reportsFound = reportsFound.Where(o => o.District == vm.districtReport);
                }
                

                model = new AdminMemberReportViewModel
                { 
                    reports = await reportsFound.ToListAsync(),
                    showReport = vm.showReport,
                    memberTypeReport = vm.memberTypeReport,
                    districtReport = vm.districtReport,
                };

            } else
            {                
                
                model = new AdminMemberReportViewModel
                { 
                    reports = await _dbContext.Organizations.Include(o => o.RepresentativeContact).Include(o => o.Addresses.Where(a=>a.Active)).ThenInclude(o => o.Address).ThenInclude(a => a.StateProvince).Include(o => o.Addresses.Where(a=>a.Active)).ThenInclude(o => o.Address).ThenInclude(a => a.County).Where(o => o.Member && o.MemberType == "Voting member").ToListAsync(),                    
                };
            }

            return model;
        }

        
    }    
    
}
