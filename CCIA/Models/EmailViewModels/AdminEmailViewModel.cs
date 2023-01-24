using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Models
{   
    public class AdminEmailViewModel 
    {
        public List<Applications> Apps { get; set; }
        public List<cropSummaryCount> SummaryCounts { get; set; }
        
        public int PendingAcceptance { get; set; }   
        public int WeekAppCount { get; set; }  
        public int TotalAppCount { get; set; } 
        public decimal? WeekAcres { get; set; }
        public decimal? TotalAcres { get; set; }
        public int WeeklyFir { get; set; }
        public int TotalFir { get; set; } 
        public List<CCIAEmployees> EmployeesToEmail { get; set; }            

        
        public static async Task<AdminEmailViewModel> CreateBase(CCIAContext _dbContext)
        {                
            var viewModel = new AdminEmailViewModel
            {
                PendingAcceptance = await _dbContext.Applications.Where(a => a.Status == ApplicationStatus.PendingAcceptance.GetDisplayName()).CountAsync(),
                WeekAppCount = await _dbContext.Applications.Where(a => !a.Submitable && a.Postmark > DateTime.Now.AddDays(-7) && 
                    (a.Status == ApplicationStatus.FieldInspectionReportReady.GetDisplayName() || a.Status == ApplicationStatus.PendingAcceptance.GetDisplayName() || 
                    a.Status == ApplicationStatus.FieldInspectionInProgress.GetDisplayName())).CountAsync(),
                TotalAppCount = await _dbContext.Applications.Where(a=>a.CertYear == CertYearFinder.CertYear && 
                    (a.Status == ApplicationStatus.FieldInspectionReportReady.GetDisplayName() || a.Status == ApplicationStatus.PendingAcceptance.GetDisplayName() || 
                    a.Status == ApplicationStatus.FieldInspectionInProgress.GetDisplayName())).CountAsync(),
                WeekAcres = await _dbContext.Applications.Where(a => !a.Submitable && a.Postmark > DateTime.Now.AddDays(-7) && 
                    (a.Status == ApplicationStatus.FieldInspectionReportReady.GetDisplayName() || a.Status == ApplicationStatus.PendingAcceptance.GetDisplayName() || 
                    a.Status == ApplicationStatus.FieldInspectionInProgress.GetDisplayName())).SumAsync(a=>a.AcresApplied),
                TotalAcres = await _dbContext.Applications.Where(a=>a.CertYear == CertYearFinder.CertYear && 
                    (a.Status == ApplicationStatus.FieldInspectionReportReady.GetDisplayName() || a.Status == ApplicationStatus.PendingAcceptance.GetDisplayName() || 
                    a.Status == ApplicationStatus.FieldInspectionInProgress.GetDisplayName())).SumAsync(a => a.AcresApplied),
                WeeklyFir = await _dbContext.FieldInspectionReport.Where(f => f.Complete && f.DateComplete > DateTime.Now.AddDays(-7)).CountAsync(),
                TotalFir = await _dbContext.Applications.Where(a => a.Status == ApplicationStatus.FieldInspectionReportReady.GetDisplayName() 
                    && a.CertYear == CertYearFinder.CertYear).CountAsync(),
                EmployeesToEmail = await _dbContext.CCIAEmployees.Include(c => c.AssignedCrops).Where(c=> c.AssignedCrops.Any()).Distinct().ToListAsync()
            };           

            return viewModel;
        }

        public static async Task<AdminEmailViewModel> GetAppsForEmployee(AdminEmailViewModel model, CCIAContext _dbContext, IFullCallService _helper, string employeeId)
        {
            model.Apps = null;
            var crops = await _dbContext.CropAssignments.Where(c => c.EmployeeId == employeeId).Select(c => c.CropId).ToListAsync();
            model.Apps =  await _helper.OverviewApplications().Where(a => a.Postmark >= DateTime.Now.AddDays(-7) && !a.Cancelled && crops.Contains(a.CropId.Value)).ToListAsync();
            var p0 = new SqlParameter("@current_year", CertYearFinder.CertYear);
            var p1 = new SqlParameter("@employee_id", employeeId);
            model.SummaryCounts =  await _dbContext.CropSummaryCount.FromSqlRaw($"EXEC mvc_weekly_staff_summary_counts @current_year, @employee_id", p0, p1).ToListAsync();
            return model;
        }

        

        
    } 

    
    
}
