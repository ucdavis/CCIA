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
     
    public class AdminPotatoBookReportViewModel
    {

        public List<Applications> apps { get; set; }
        public List<Organizations> orgs { get; set; }      

        public List<int> certYears { get; set; }
       
        public int certYearsReport { get; set; }

       
          
      
               
        public static async Task<AdminPotatoBookReportViewModel> Create(CCIAContext _dbContext, AdminPotatoBookReportViewModel vm )
        { 
            var reportsFound = _dbContext.Applications
                .Include(a => a.FieldInspection)
                .Include(a => a.Variety)
                .Include(a => a.Crop)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.FieldInspectionReport)
                .Include(a => a.ClassProduced).AsQueryable(); 
            var model = new AdminPotatoBookReportViewModel(); 
            if(vm.certYearsReport != 0)
            {
                model = new AdminPotatoBookReportViewModel
                { 
                    apps = await reportsFound.Where(a => a.CertYear == vm.certYearsReport && a.AppType == AppTypes.Potato.GetDisplayName() && a.ApplicantId != 7000 && !a.Cancelled).ToListAsync(),
                    certYears = CertYearFinder.certYearListReverse,
                };

            } else
            {                
                
                model = new AdminPotatoBookReportViewModel
                { 
                    certYears = CertYearFinder.certYearListReverse,
                };
            }

            return model;
        }

        
    }    
    
}
