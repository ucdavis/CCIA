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
     
    public class AdminApplicationReportViewModel
    {
       public List<ApplicationReport> reports { get; set; }

       public List<AbbrevAppType> appTypes {get; set; }

       public string appTypeReport { get; set; }

        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> cropsReport { get; set; }

        public List<int> certYears { get; set; }
       
        public List<int> certYearsReport { get; set; }

        public int varietyIdReport { get; set; }

        public List<County> counties { get; set; }

        public List<int> countiesReport { get; set; }        

        public string reportType { get; set; }
      
               
        public static async Task<AdminApplicationReportViewModel> Create(CCIAContext _dbContext, AdminApplicationReportViewModel vm )
        { 
            List<ApplicationReport> reportsFound = new List<ApplicationReport>();              
            var model = new AdminApplicationReportViewModel(); 

            var apptypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync();
            apptypes.Insert(0, new AbbrevAppType{ AppTypeId = 0, AppTypeTrans = "Any", Abbreviation = "%"});

            if(vm.appTypeReport != null)
            {
                var p0 = new SqlParameter("@cert_year", string.Join(",", vm.certYearsReport));
                var p1 = new SqlParameter("@app_type", vm.appTypeReport);
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

                reportsFound = await _dbContext.ApplicationReport.FromSqlRaw($"EXEC mvc_report_application_summary @cert_year, @app_type, @crop_id, @county, @report_type, @variety_id", p0, p1, p2, p3, p4, p5).ToListAsync();  

                model = new AdminApplicationReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                    appTypes = apptypes,
                    certYears = CertYearFinder.certYearListReverse,
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                    appTypeReport = vm.appTypeReport,
                    cropsReport = vm.cropsReport,
                    certYearsReport = vm == null ?  new List<int>(new int[] { CertYearFinder.CertYear - 1 }) : vm.certYearsReport,
                    varietyIdReport = vm.varietyIdReport,
                    countiesReport = vm.countiesReport,
                    reportType = vm.reportType,
                };

            } else
            {                
                model = new AdminApplicationReportViewModel
                { 
                    reports = reportsFound,
                    crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                    appTypes = apptypes,
                    certYearsReport = new List<int>(new int[] { CertYearFinder.CertYear - 1 }),
                    certYears = CertYearFinder.certYearListReverse,
                    counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                };
            }
            
            return model;
        }
        
    }    
    
}