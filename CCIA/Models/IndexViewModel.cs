using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.IndexViewModels
{    
    public class IndexViewModel
    {
        public List<int>  certYears { get; set; }  
        public int CertYear { get; set; }      
    }

    public class ApplicationIndexViewModel : IndexViewModel
    {        
        public List<Applications> applications { get; set; }

        public static async Task<ApplicationIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new ApplicationIndexViewModel
            {
                applications = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.ApplicantId == orgId)
                .Include(a => a.GrowerOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .ToListAsync(),
               certYears =  await _dbContext.Applications.Where(a => a.ApplicantId == orgId).Select(a => a.CertYear).Distinct().ToListAsync(),
               CertYear = certYear                
            };

            return viewModel;
        }
    }

    public class BlendIndexViewModel : IndexViewModel
    {
        public List<BlendRequests> blends { get; set; }

        public static async Task<BlendIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new BlendIndexViewModel
            {
                blends = await _dbContext.BlendRequests.Where(b => b.ConditionerId == orgId && b.CertYear == certYear)
                .Include(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from knownh app) => indirt => application => variety
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Variety)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                .ThenInclude(i => i.Application) 
                .ThenInclude(a => a.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                .ThenInclude(i => i.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                .ThenInclude(i => i.Variety)
                .Include(b => b.Variety) // blendrequest (varietal) => variety => crop
                .ThenInclude(v => v.Crop)
                .ToListAsync(),
                certYears =  await _dbContext.BlendRequests.Where(a => a.ConditionerId == orgId).Select(a => a.CertYear).Distinct().ToListAsync(),
               CertYear = certYear 
            };
            
            return viewModel;
        }
    }

    public class BulkSalesCertificatesIndexViewModel : IndexViewModel
    {
        public List<BulkSalesCertificates> bulkSalesCertificates { get; set; }

        public static async Task<BulkSalesCertificatesIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new BulkSalesCertificatesIndexViewModel
            {
                bulkSalesCertificates = await _dbContext.BulkSalesCertificates.Where(b => b.ConditionerOrganizationId == orgId && b.Date.Year == certYear)   
                .Include(b => b.Seeds)
                .Include(b => b.PurchaserState)                            
                .Include(b => b.PurchaserCountry)
                .Include(b => b.Class)
                .Include(b => b.CreatedByContact)
                .Include(b => b.ConditionerOrganization)
                .Include(b => b.AdminEmployee)
                .Include(b => b.BulkSalesCertificatesShares)
                .ToListAsync(),
                certYears =  await _dbContext.BulkSalesCertificates.Where(a => a.ConditionerOrganizationId == orgId).Select(a => a.Date.Year).Distinct().ToListAsync(),
               CertYear = certYear 
            };
            
            return viewModel;
        }
    }

    public class OECDIndexViewModel : IndexViewModel
    {
        public List<OECD> oecd { get; set; }

        public static async Task<OECDIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new OECDIndexViewModel
            {
                oecd = await _dbContext.OECD.Where(o => o.ConditionerId == orgId && o.DataEntryYear == certYear)  
                .Include(o => o.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(o => o.Class)
                .Include(o => o.Country)              
                .ToListAsync(),
                certYears =  await _dbContext.OECD.Where(a => a.ConditionerId == orgId).Select(a => a.DataEntryYear).Distinct().ToListAsync(),
               CertYear = certYear 
            };
            
            return viewModel;
        }
    }

    public class SeedsIndexViewModel : IndexViewModel
    {
        public List<Seeds> seeds { get; set; }

        public static async Task<SeedsIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new SeedsIndexViewModel
            {
                seeds = await _dbContext.Seeds.Where(s => s.CertYear == certYear && s.ConditionerId == orgId)
                .Include(a => a.ApplicantOrganization)
                .Include(v => v.Variety)
                .ThenInclude(v => v.Crop)
                .Include(c => c.ClassProduced)
                .Include(l => l.LabResults)
                .ToListAsync(),
                certYears =  await _dbContext.Seeds.Where(a => a.ConditionerId == orgId).Select(a => a.CertYear.Value).Distinct().ToListAsync(),
               CertYear = certYear 
            };
            
            return viewModel;
        }
    }
}
