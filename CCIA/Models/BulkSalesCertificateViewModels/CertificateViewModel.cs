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
       
       

        public static async Task<CertificateViewModel> Create(CCIAContext _dbContext, int id, int orgId)
        {           
            var viewModel = new CertificateViewModel
            {
                BulkSalesCertificate = await _dbContext.BulkSalesCertificates.Where(b => b.Id == id && b.ConditionerOrganizationId == orgId)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.AppTypeTrans)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(b => b.ConditionerOrganization)
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
                .SingleAsync()
            };

            return viewModel;
        }
    }
}
