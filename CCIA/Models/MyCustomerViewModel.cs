using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    public class MyCustomerViewModel {
        public MyCustomers MyCustomer { get; set; }

        public List<StateProvince> StateProvinces {get; set;}

        public List<Countries> Countries { get; set; }

        public List<County> Counties { get; set; }
 
        public static async Task<MyCustomerViewModel> Create(CCIAContext _dbContext) {
            
            var stateProvinces = await _dbContext.StateProvince
                .OrderBy(s => s.StateProvinceName)
                .OrderByDescending(s => s.CountryId)
                .Select(s => new StateProvince { StateProvinceId = s.StateProvinceId, StateProvinceName = s.StateProvinceName } )
                .ToListAsync();
            
            var countries = await _dbContext.Countries
                .OrderBy(c => c.Name)
                .Select(c => new Countries { Id = c.Id, Name = c.Name })
                .ToListAsync();
            
            var state = await _dbContext.StateProvince.Where(s => s.StateProvinceName == "California").Select(s => s.StateProvinceId).FirstAsync();

            var counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                .OrderBy(c => c.CountyName)
                .Select(c => new County { CountyId = c.CountyId, CountyName = c.CountyName.ToUpper() })
                .ToListAsync();

            counties.Insert(0, new County { CountyId = 0, CountyName = "Select a county" });
            
            var viewModel = new MyCustomerViewModel()
            {
                MyCustomer = new MyCustomers(),
                StateProvinces = stateProvinces,
                Countries = countries,
                Counties = counties
            };

            return viewModel;
        }

        public static async Task<MyCustomerViewModel> Edit(CCIAContext _dbContext, int id) {
            
            var model = await _dbContext.MyCustomers.Where(a => a.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            var stateProvinces = await _dbContext.StateProvince
                .OrderBy(s => s.StateProvinceName)
                .Select(s => new StateProvince { StateProvinceId = s.StateProvinceId, StateProvinceName = s.StateProvinceName } )
                .ToListAsync();
            
            var countries = await _dbContext.Countries
                .OrderBy(c => c.Name)
                .Select(c => new Countries { Id = c.Id, Name = c.Name })
                .ToListAsync();
            
            var state = await _dbContext.StateProvince.Where(s => s.StateProvinceName == "California").Select(s => s.StateProvinceId).FirstAsync();

            var counties = await _dbContext.County.Where(c => c.StateProvinceId == state)
                .OrderBy(c => c.CountyName)
                .Select(c => new County { CountyId = c.CountyId, CountyName = c.CountyName.ToUpper() })
                .ToListAsync();

            counties.Insert(0, new County { CountyId = 0, CountyName = "Select a county" });
            
            var viewModel = new MyCustomerViewModel()
            {
                MyCustomer = model,
                StateProvinces = stateProvinces,
                Countries = countries,
                Counties = counties
            };

            return viewModel;
        }
    }
}