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
     
    public class AdminOECDReportViewModel
    {
       public List<OECDReport> reports { get; set; }
       

        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> cropsReport { get; set; }

        public List<int> certYears { get; set; }
        
        public List<int> certYearsReport { get; set; }

        public int varietyIdReport { get; set; }

        public List<County> counties { get; set; }

        public List<int> countiesReport { get; set; }        

        public string reportType { get; set; }
      
               
        public static async Task<AdminOECDReportViewModel> Create(CCIAContext _dbContext, AdminOECDReportViewModel vm )
        { 
            List<OECDReport> reportsFound = new List<OECDReport>(); 
            var model = new AdminOECDReportViewModel(); 
            if(vm.reportType != null)
            {
                var p0 = new SqlParameter("@confirmed_years", string.Join(",", vm.certYearsReport));
                var p1 = new SqlParameter("@crop_id", "0"); 
                var p2 = new SqlParameter("@variety_id", "0");
                var p3 = new SqlParameter("@county", "0");
                var p4 = new SqlParameter("@report_type", vm.reportType);
               
               
                if(vm.cropsReport != null && vm.cropsReport.Count > 0)
                {
                    p1 = new SqlParameter("@crop_id", string.Join(",", vm.cropsReport));
                } 
                if(vm.countiesReport != null && vm.countiesReport.Count > 0)
                {
                    p3 = new SqlParameter("@county", string.Join(",", vm.countiesReport));
                }                 
                if(vm.varietyIdReport != 0)
                {
                    p2 = new SqlParameter("@variety_id", vm.varietyIdReport);
                } 

                reportsFound = await _dbContext.OECDReport.FromSqlRaw($"EXEC mvc_report_oecd @confirmed_years, @crop_id, @variety_id, @county, @report_type", p0, p1, p2, p3, p4).ToListAsync();  

                model = new AdminOECDReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
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
                
                model = new AdminOECDReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                    certYears = CertYearFinder.certYearListReverse,
                    certYearsReport =  new List<int>(new int[] { CertYearFinder.CertYear - 1 }),
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                };
            }

            return model;
        }

        
    }    
    
}
