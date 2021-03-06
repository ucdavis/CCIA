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
     
    public class AdminOECDReportingViewModel
    {
        public List<OECD> certs { get; set; } 
        
        
        [Display(Name="Beginning Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime startDate { get; set; }

        
        [Display(Name="Ending Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime endDate { get; set; }

               
        public static async Task<AdminOECDReportingViewModel> Create(CCIAContext _dbContext, AdminOECDReportingViewModel vm)
        {                       
            if(vm != null)
            {
                var oecdToFind = _dbContext.OECD
                    .Include(o => o.Seeds)
                    .ThenInclude(s => s.LabResults)  
                    .ThenInclude(l => l.LabOrganization)                  
                    .Include(o => o.Class)                                     
                    .Include(o => o.ConditionerOrganization)
                    .Include(o => o.Variety)
                    .ThenInclude(v => v.Crop)
                    .AsQueryable();  
                    
                oecdToFind = oecdToFind.Where(o => o.DatePrinted >= vm.startDate && o.DatePrinted <= vm.endDate && !o.USDAReported);
                
                var viewModel = new AdminOECDReportingViewModel
                {
                    certs = await oecdToFind.ToListAsync(),
                    startDate = vm.startDate,
                    endDate = vm.endDate,                    
                };  
                return viewModel;


            }
            var firstOfThisMonth = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            var freshModel = new AdminOECDReportingViewModel
            {
                certs = new List<OECD>(),  
                startDate = firstOfThisMonth.AddMonths(-3),
                endDate = firstOfThisMonth.AddDays(-1),              
            };           

            return freshModel;
        }

        
    }    
    
}
