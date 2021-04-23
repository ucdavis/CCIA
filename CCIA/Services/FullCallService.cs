using System.Linq;
using CCIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Services
{
    public interface IFullCallService
    {
        IQueryable<Tags> FullTag();

        IQueryable<Applications> OverviewApplications();

        IQueryable<Applications> FullApplications();

        IQueryable<Applications> FIRApplications();

        IQueryable<RenewFields> FullRenewFields();

        IQueryable<Seeds> FullSeeds();

        IQueryable<Seeds> OverviewSeeds();

        IQueryable<Certs> FullCerts();

        IQueryable<OECD> FullOECD();

        IQueryable<Organizations> FullOrg();

        IQueryable<Contacts> FullContact();
    }

     public class FullCallService : IFullCallService
    {
        private readonly CCIAContext _context;       


        public FullCallService(CCIAContext context)
        {           
            _context = context;            
        }

        public IQueryable<Certs> FullCerts()
        {
            var certs =  _context.Certs
                .Include(c => c.Class)
                .Include(c => c.ApplicantOrganization)
                .Include(c => c.Variety)
                .ThenInclude(v => v.Crop)
                .AsQueryable();
            return certs;
        }

        public IQueryable<Seeds> FullSeeds()
        {
            var seed = _context.Seeds
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(v => v.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(c => c.ClassProduced)
                .Include(l => l.LabResults)
                .Include(s => s.CountryOfOrigin)
                .Include(s => s.StateOfOrigin)
                .Include(s => s.CountySampleDrawn)
                .Include(s => s.ContactEntered)
                .Include(s => s.Documents)
                .ThenInclude(d => d.DocumentType)                
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.GrowerOrganization)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.ApplicantOrganization)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.Crop)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.Variety)
                .Include(s => s.SeedsApplications)
                .ThenInclude(sa => sa.Application)
                .ThenInclude(a => a.ClassProduced)
                .AsQueryable();
            return seed;

        }

        public IQueryable<Seeds> OverviewSeeds()
        {
            var seed = _context.Seeds
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .Include(s => s.Variety)
                .AsQueryable();
            return seed;

        }

        public IQueryable<OECD> FullOECD()
        {
            var oecd = _context.OECD
                .Include(o => o.Seeds)
                .ThenInclude(s => s.ClassProduced)
                .Include(o => o.Seeds)
                .ThenInclude(s => s.LabResults)
                .Include(o => o.Class)
                .Include(o => o.ShipperOrganization)
                .ThenInclude(s => s.Address)
                .ThenInclude(sa => sa.StateProvince)
                .Include(o => o.ConditionerOrganization)
                .ThenInclude(c => c.Address)
                .ThenInclude(ca => ca.StateProvince)
                .Include(o => o.Country)
                .Include(o => o.Variety)
                .ThenInclude(v => v.Crop)
                .Include(o => o.Changes)
                .ThenInclude(c => c.Employee)
                .Include(o => o.EntryEmployee)
                .Include(o => o.UpdateEmployee)
                .AsQueryable();
            return oecd;

        }

        public IQueryable<Applications> OverviewApplications()
        {
            var app = _context.Applications
                .Include(a => a.GrowerOrganization)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.FieldInspection)
                .AsQueryable();
            return app;

        }

        public IQueryable<Applications> FullApplications()
        {
            var app = _context.Applications
                .Include(a => a.GrowerOrganization)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.Certificates)
                .Include(a => a.PlantingStocks)
                .ThenInclude(p => p.PsClassNavigation)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedStateProvince)
                .Include(a => a.FieldHistories).ThenInclude(fh => fh.FHCrops)
                .Include(a => a.AppCertRad)
                .Include(a => a.FieldInspection)
                .Include(a => a.Changes).ThenInclude(c => c.Employee)
                .AsQueryable();
            return app;

        }

        public IQueryable<Applications> FIRApplications()
        {
            var app = _context.Applications
                .Include(a => a.AppTypeTrans)
                .Include(a => a.GrowerOrganization)
                .Include(a => a.ApplicantOrganization)
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.AppCertRad)
                .Include(a => a.County)               
                .Include(a => a.FieldInspectionReport).ThenInclude(r => r.CompleteEmployee) 
                .Include(a => a.FieldInspection).ThenInclude(i => i.InspectorEmployee) 
                .AsQueryable();
            return app;

        }

        public IQueryable<RenewFields> FullRenewFields()
        {
            var renew = _context.RenewFields
                .Include(r => r.Application)
                .ThenInclude(a => a.GrowerOrganization)
                .Include(r => r.Application)
                .ThenInclude(a => a.ApplicantOrganization)
                .Include(r => r.Application)
                .ThenInclude(a => a.Crop)
                .Include(r => r.Application)
                .ThenInclude(a => a.Variety)
                .Include(r => r.Application)
                .ThenInclude(a => a.ClassProduced)
                .AsQueryable();
            return renew;

        }

        public IQueryable<Organizations> FullOrg()
        {
            var org = _context.Organizations
               .Include(o => o.Address)
               .ThenInclude(a => a.County)
               .Include(o => o.Address)
               .ThenInclude(a => a.StateProvince)
               .Include(o => o.Address)
               .ThenInclude(a => a.Countries)
               .Include(o => o.RepresentativeContact)
               .Include(o => o.Employees)
               .Include(o => o.ConditionerStatus)
               .Include(o => o.MapPermissions)
               .Include(o => o.MapCropPermissions)
               .ThenInclude(p => p.Crop)
               .Include(o => o.Addresses)
               .ThenInclude(oa => oa.Address)
               .ThenInclude(a => a.County)
               .Include(o => o.Addresses)
               .ThenInclude(oa => oa.Address)
               .ThenInclude(a => a.StateProvince)
               .Include(o => o.Addresses)
               .ThenInclude(oa => oa.Address)
               .ThenInclude(a => a.Countries)               
                .AsQueryable();
            return org;
        }

        public IQueryable<Contacts> FullContact()
        {
            var contact = _context.Contacts
                .Include(c => c.Addresses)
                .ThenInclude(a => a.Address)
                .ThenInclude(a => a.County)
                .Include(c => c.Addresses)
                .ThenInclude(a => a.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(c => c.Addresses)
                .ThenInclude(a => a.Address)
                .ThenInclude(a => a.Countries)
                .AsQueryable();

            return contact;
        }

         public IQueryable<Tags> FullTag() 
        {
            var tag = _context.Tags
                .Include(t => t.Application)
                .Include(t => t.TagSeries)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Variety)                
                .ThenInclude(v => v.Crop)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.ClassProduced)
                .Include(s => s.Seeds)
                .ThenInclude(s => s.LabResults)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Ecoregion)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.County)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(a => a.Ecoregion)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(a => a.CountyHarvested)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.ConditionerOrganization)
                .Include(t => t.Blend) 
                .ThenInclude(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => variety
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
                .Include(t => t.BulkCrop)
                .Include(t => t.BulkVariety)
                .Include(t => t.TaggingOrganization)
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(t => t.ContactEntered)
                .Include(t => t.OECDClass)
                .Include(t => t.OECDCountry)
                .Include(t => t.Changes)
                .ThenInclude(c => c.Employee)
                .AsQueryable();
            return tag;
        }
    }
}