using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.BulkSalesCertificateShareViewModel
{
    public class BulkSalesCertificateShareViewModel
    {
        public BulkSalesCertificates BulkSalesCertificate { get; set; }
        public List<BulkSalesCertificatesShares> Shares { get; set; }

              

        public static async Task<BulkSalesCertificateShareViewModel> Create(CCIAContext _dbContext, int id, int orgId)
        {
           var certificates = await _dbContext.BulkSalesCertificates.Where(b => b.Id == id && b.ConditionerOrganizationId == orgId)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.AppTypeTrans)
                .Include(b => b.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)               
                .Include(b => b.PurchaserState)
                .Include(b => b.PurchaserCountry)               
                .SingleOrDefaultAsync();
            var shares = await _dbContext.BulkSalesCertificatesShares.Where(s => s.BulkSalesCertificatesId == id)
                .Include(s => s.ShareOrganization)
                .ToListAsync();
            var viewModel = new BulkSalesCertificateShareViewModel
            {
                BulkSalesCertificate = certificates,
                Shares = shares,
            };

            return viewModel;
        }
    }
}
