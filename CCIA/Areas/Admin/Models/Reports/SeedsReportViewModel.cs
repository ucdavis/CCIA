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
     
    public class AdminSeedsReportViewModel
    {
       public List<SeedsReport> reports { get; set; }
       

        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> cropsReport { get; set; }

        public List<int> certYears { get; set; }
        
        public List<int> certYearsReport { get; set; }

        public int varietyIdReport { get; set; }

        public List<County> counties { get; set; }

        public List<int> countiesReport { get; set; }        

        public string reportType { get; set; }
      
               
        public static async Task<AdminSeedsReportViewModel> Create(CCIAContext _dbContext, AdminSeedsReportViewModel vm )
        { 
            List<SeedsReport> reportsFound = new List<SeedsReport>(); 
            var model = new AdminSeedsReportViewModel(); 
            if(vm.reportType != null)
            {
                var p0 = new SqlParameter("@cert_year", string.Join(",", vm.certYearsReport));
                var p2 = new SqlParameter("@crop_id", "0");
                var p3 = new SqlParameter("@county", "0");
                var p4 = new SqlParameter("@report_type", vm.reportType);
                var p5 = new SqlParameter("@variety_id", "0");
               
                if(vm.cropsReport != null && vm.cropsReport.Count > 0)
                {
                    p2 = new SqlParameter("@crop_id", string.Join(",", vm.cropsReport));
                } 
                if(vm.countiesReport != null && vm.countiesReport.Count > 0)
                {
                    p3 = new SqlParameter("@county", string.Join(",", vm.countiesReport));
                }                 
                if(vm.varietyIdReport != 0)
                {
                    p5 = new SqlParameter("@variety_id", vm.varietyIdReport);
                } 

                reportsFound = await _dbContext.SeedsReport.FromSqlRaw($"EXEC mvc_report_application_summary @cert_year, @crop_id, @county, @report_type, @variety_id", p0, p2, p3, p4, p5).ToListAsync();  

                model = new AdminSeedsReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                    certYears = CertYearFinder.certYearListReverse,
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                    cropsReport = vm.cropsReport,
                    certYearsReport = vm == null ?  new List<int>(new int[] { CertYearFinder.CertYear - 1 }) : vm.certYearsReport,
                    varietyIdReport = vm.varietyIdReport,
                    countiesReport = vm.countiesReport,
                    reportType = vm.reportType,
                };

            } else
            {                
                
                model = new AdminSeedsReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                    certYears = CertYearFinder.certYearListReverse,
                    certYearsReport =  new List<int>(new int[] { CertYearFinder.CertYear - 1 }),
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                };
            }

            return model;
        }

        
    }    
    
}
