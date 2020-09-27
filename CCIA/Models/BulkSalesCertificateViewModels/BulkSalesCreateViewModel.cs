using System;
using System.Collections.Generic;
using System.ComponentModel;
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

        public List<StateProvince> StateProvinces {get; set;}

        public List<Countries> Countries { get; set; }

        [DisplayName("Id")]
        public int? textId { get; set; }

        public string selectType { get; set; }

       

        public static async Task<BulkSalesCreateViewModel> Create(CCIAContext _dbContext, int orgId)
        {
            var customers = await _dbContext.MyCustomers.Where(m => m.OrganizationId == orgId)                    
                    .Select(m => new MyCustomers { Id = m.Id, Name = m.Name})
                    .ToListAsync();
            customers.Insert(0, new MyCustomers { Id = 0, Name = "Select Customer"});
            var stateProvince = await _dbContext.StateProvince.Select(s => new StateProvince { StateProvinceId = s.StateProvinceId, Name = s.Name } ).ToListAsync();
            var countries = await _dbContext.Countries.Select(c => new Countries { Id = c.Id, Name = c.Name }).ToListAsync();
            var viewModel = new BulkSalesCreateViewModel
            {
                BulkSalesCertificate = new BulkSalesCertificates(),
                Customers = customers,
                StateProvinces = stateProvince,
                Countries = countries
            };

            return viewModel;
        }
    }
}
