using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Models
{   
    public class AdminEmailViewModel 
    {
        public List<Applications> Apps { get; set; }
        
        public int PendingAcceptance { get; set; }   
        public int WeekAppCount { get; set; }  
        public int TotalAppCount { get; set; } 
        public decimal? WeekAcres { get; set; }
        public decimal? TotalAcres { get; set; }
        public int WeeklyFir { get; set; }
        public int TotalFir { get; set; }             

        
        public static async Task<AdminEmailViewModel> CreateStaff(CCIAContext _dbContext)
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
                    && a.CertYear == CertYearFinder.CertYear).CountAsync()
            };           

            return viewModel;
        }

        

        
    } 
    
}
