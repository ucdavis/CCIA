using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.CertificateViewModel
{
    public class CertificateViewModel
    {
        public BulkSalesCertificates BulkSalesCertificate { get; set; }

        public List<AbbrevClassSeeds> Classes { get; set; }

        public string StandardsMessage { get; set; }

        public string AssayMessage { get; set; }
       
       

        public static async Task<CertificateViewModel> Create(CCIAContext _dbContext, int id, int orgId)
        {
            var programId = await _dbContext.BulkSalesCertificates.Include(b => b.CertProgram).Where(b => b.Id == id).Select(b => new Tuple<int?, int?>(b.CertProgram.AppTypeId, b.SeedsID.Value)).SingleAsync();
            var classes = await _dbContext.AbbrevClassSeeds.Where(s => s.Program == programId.Item1).ToListAsync();            
            var standardsMessage = await _dbContext.Seeds.Where(x => x.Id == programId.Item2).Select(d => CCIAContext.GetStandardsMessage(d.Id)).FirstOrDefaultAsync();
            var assayMessage = await _dbContext.Seeds.Where(x => x.Id == programId.Item2).Select(d => CCIAContext.GetAssayMessage(d.Id)).FirstOrDefaultAsync();
            var shares = await _dbContext.BulkSalesCertificatesShares.Where(s => s.ShareOrganizationId == orgId && s.BulkSalesCertificatesId == id).Select(s => s.BulkSalesCertificatesId).ToListAsync();
            var viewModel = new CertificateViewModel
            {
                BulkSalesCertificate = await _dbContext.BulkSalesCertificates.Where(b => b.Id == id && (b.ConditionerOrganizationId == orgId || shares.Contains(b.Id)))
                .Include(b => b.Seeds)
                .ThenInclude(s => s.AppTypeTrans)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.LabResults)
                .Include(b => b.ConditionerOrganization)
                .ThenInclude(c => c.Addresses.Where(a => a.Active))
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(b => b.ConditionerOrganization)
                .ThenInclude(c => c.Addresses.Where(a => a.Active))
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.Countries)
                .Include(b => b.PurchaserState)
                .Include(b => b.PurchaserCountry)
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
                .Include(b => b.CreatedByContact)
                .SingleAsync(),
                Classes = classes,
                StandardsMessage = standardsMessage,
                AssayMessage = assayMessage,
            };

            return viewModel;
        }
    }
}
