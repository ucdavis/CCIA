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
     
    public class AdminRebateReportViewModel
    {
       public List<RebateReport> reports { get; set; }

        public List<int> fiscalYears { get; set; }
        
        public int fiscalYearReport { get; set; }
      
               
        public static async Task<AdminRebateReportViewModel> Create(CCIAContext _dbContext, AdminRebateReportViewModel vm )
        { 
            List<RebateReport> reportsFound = new List<RebateReport>(); 
            var model = new AdminRebateReportViewModel(); 
            if(vm.fiscalYearReport != 0)
            {
                
                var p0 = new SqlParameter("@fiscalYear", "0"); 
                           
                if(vm.fiscalYearReport != 0)
                {
                    p0 = new SqlParameter("@fiscalYear", vm.fiscalYearReport);
                } 

                reportsFound = await _dbContext.RebateReport.FromSqlRaw($"EXEC mvc_rebate_report @fiscalYear", p0).ToListAsync();  

                model = new AdminRebateReportViewModel
                { 
                    reports = reportsFound,                    
                    fiscalYears = CertYearFinder.certYearListReverse,
                    fiscalYearReport = vm == null ?   CertYearFinder.CertYear - 1  : vm.fiscalYearReport,
                };

            } else
            {                
                
                model = new AdminRebateReportViewModel
                { 
                    reports = reportsFound,
                    fiscalYears = CertYearFinder.certYearListReverse,
                    fiscalYearReport =  (CertYearFinder.CertYear - 1),                    
                };
            }

            return model;
        }

        
    }    
    
}
