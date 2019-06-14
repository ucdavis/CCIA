using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.BulkSalesCreateViewModel
{
    public class BulkSalesCreateViewModel
    {
        public BulkSalesCertificates BulkSalesCertificate { get; set; }
        public List<MyCustomers> Customers { get; set; }


        public static async Task<BulkSalesCreateViewModel> Create(CCIAContext _dbContext, int orgId)
        {
            var customers = await _dbContext.MyCustomers.Where(m => m.OrganizationId == orgId)                    
                    .Select(m => new MyCustomers { Id = m.Id, Name = m.Name})
                    .ToListAsync();
            customers.Insert(0, new MyCustomers { Id = 0, Name = "Select Customer"});
            var viewModel = new BulkSalesCreateViewModel
            {
                BulkSalesCertificate = new BulkSalesCertificates(),
                Customers = customers,
            };

            return viewModel;
        }
    }
}
