using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.IndexViewModels
{
    public class IndexViewModel
    {
        public List<int> certYears { get; set; }
        public int CertYear { get; set; }

        public string PageTitle { get; set; }

        public string DropDownText { get; set; }
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
                certYears = await _dbContext.Applications.Where(a => a.ApplicantId == orgId).OrderByDescending(a => a.CertYear).Select(a => a.CertYear).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Applications",
                DropDownText = "Display Apps for Cert Year:"
            };

            return viewModel;
        }
    }

    public class AdminApplicationIndexViewModel : IndexViewModel
    {
        public List<Applications> applications { get; set; }

        public static async Task<ApplicationIndexViewModel> Create(CCIAContext _dbContext, int certYear, bool accepted, IFullCallService _helper)
        {
            var queryable = _helper.OverviewApplications().Where(a => a.CertYear == certYear);
            if(accepted)
            {
                 queryable = _helper.OverviewApplications().Where(a => a.CertYear == certYear && a.Approved == true);
            }
            
            var viewModel = new ApplicationIndexViewModel
            {
                applications = await queryable.ToListAsync(),
                certYears = await _dbContext.Applications.OrderBy(a => a.CertYear).OrderByDescending(a => a.CertYear).Select(a => a.CertYear).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Applications",
                DropDownText = "Display Apps for Cert Year:"
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
                certYears = await _dbContext.BlendRequests.Where(a => a.ConditionerId == orgId).OrderByDescending(a => a.CertYear).Select(a => a.CertYear).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Blends",
                DropDownText = "Display Blends for Cert Year:"
            };

            return viewModel;
        }
    }

    public class BulkSalesCertificatesIndexViewModel : IndexViewModel
    {
        public List<BulkSalesCertificates> bulkSalesCertificates { get; set; }

        public static async Task<BulkSalesCertificatesIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var shares = await _dbContext.BulkSalesCertificatesShares.Where(s => s.ShareOrganizationId == orgId).Select(s => s.BulkSalesCertificatesId).ToListAsync();
            var viewModel = new BulkSalesCertificatesIndexViewModel
            {
                bulkSalesCertificates = await _dbContext.BulkSalesCertificates.Where(b => (b.ConditionerOrganizationId == orgId || shares.Contains(b.Id)) && b.Date.Year == certYear)
                .Include(b => b.Seeds)
                .Include(b => b.PurchaserState)
                .Include(b => b.PurchaserCountry)
                .Include(b => b.Class)
                .Include(b => b.CreatedByContact)
                .Include(b => b.ConditionerOrganization)
                .Include(b => b.AdminEmployee)
                .Include(b => b.BulkSalesCertificatesShares)
                .ToListAsync(),
                certYears = await _dbContext.BulkSalesCertificates.Where(a => a.ConditionerOrganizationId == orgId).OrderByDescending(a => a.Date.Year).Select(a => a.Date.Year).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Bulk Sales Certificates",
                DropDownText = "Display Sales Certificates for Year:"
            };

            return viewModel;
        }
    }

    public class MyCustomersIndexViewModel : IndexViewModel
    {
        public List<MyCustomers> myCustomers { get; set; }

        public static async Task<MyCustomersIndexViewModel> Create(CCIAContext _dbContext, int orgId)
        {
            var viewModel = new MyCustomersIndexViewModel
            {
                myCustomers = await _dbContext.MyCustomers.Where(e => e.OrganizationId == orgId)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .ToListAsync(),
                PageTitle = "My Customers"
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
                certYears = await _dbContext.OECD.Where(a => a.ConditionerId == orgId).OrderByDescending(a => a.DataEntryYear).Select(a => a.DataEntryYear).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "OECD",
                DropDownText = "Display OECD Forms for Data Entry Year:"
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
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(c => c.ClassProduced)
                .Include(l => l.LabResults)
                .ToListAsync(),
                certYears = await _dbContext.Seeds.Where(a => a.ConditionerId == orgId).OrderByDescending(a => a.CertYear).Select(a => a.CertYear.Value).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Seeds",
                DropDownText = "Display SID for Cert Year:"
            };

            return viewModel;
        }
    }

    public class SeedTransferIndexViewModel : IndexViewModel
    {
        public List<SeedTransfers> seedsTransfers { get; set; }

        public static async Task<SeedTransferIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new SeedTransferIndexViewModel
            {
                seedsTransfers = await _dbContext.SeedTransfers.Where(s => s.OriginatingOrganizationId == orgId && s.CertificateDate.Year == certYear)
                .Include(st => st.Seeds)
                .ThenInclude(s => s.ClassProduced)
                .Include(st => st.Application)
                .ThenInclude(a => a.ClassProduced)
                .ToListAsync(),
                certYears = await _dbContext.SeedTransfers.Where(a => a.OriginatingOrganizationId == orgId).OrderByDescending(a => a.CertificateDate.Year).Select(a => a.CertificateDate.Year).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Seed Transfer Certificates",
                DropDownText = "Display Seed Transfer Certificates for Creation Year:"
            };

            return viewModel;
        }
    }

    public class TagIndexViewModel : IndexViewModel
    {
        public List<Tags> tags { get; set; }

        public static async Task<TagIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new TagIndexViewModel
            {
                tags = await _dbContext.Tags.Where(t => t.TaggingOrg == orgId && t.DateRequested.Value.Year == certYear)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Variety)                
                .ThenInclude(v => v.Crop)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.ClassProduced)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Variety)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(t => t.Blend) 
                .ThenInclude(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from knownh app) => indirt => application => variety
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Variety)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                .ThenInclude(i => i.Application) 
                .ThenInclude(a => a.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                .ThenInclude(i => i.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                .ThenInclude(i => i.Variety)
                .Include(t => t.Blend)
                .ThenInclude(b => b.Variety) // blendrequest (varietal) => variety => crop
                .ThenInclude(v => v.Crop)
                .Include(t => t.TagAbbrevClass)
                .Include(t => t.AbbrevTagType)
                .ToListAsync(),
                certYears = await _dbContext.Tags.Where(t => t.TaggingOrg == orgId).OrderByDescending(a => a.DateRequested.Value.Year).Select(t => t.DateRequested.Value.Year).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Tags",
                DropDownText = "Display Tags Requested in Calendar Year:"
            };

            return viewModel;
        }
    }

    public class TurgrassCertificateIndexViewModel : IndexViewModel
    {
        public List<Applications> applications { get; set; }

        public static async Task<TurgrassCertificateIndexViewModel> Create(CCIAContext _dbContext, int orgId, int certYear)
        {
            var viewModel = new TurgrassCertificateIndexViewModel
            {
                applications = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.ApplicantId == orgId && a.CertYear == certYear)                
                .Include(a => a.GrowerOrganization)                
                .Include(a => a.County)
                .Include(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(a => a.ClassProduced)
                .Include(a => a.TurfgrassCertificates)
                .ToListAsync(),
                certYears = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.ApplicantId == orgId).OrderByDescending(a => a.CertYear).Select(a => a.CertYear).Distinct().ToListAsync(),
                CertYear = certYear,
                PageTitle = "Turfgrass Certificates",
                DropDownText = "Display Certificates for Apps from Cert Year:"
            };

            return viewModel;
        }
    }
}
