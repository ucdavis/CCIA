using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.DetailsViewModels
{   
    public class AdminPSViewModel 
    {
        public PlantingStocks plantingStocks { get; set; }
        public List<AbbrevClassProduced> psClass { get; set; }
        public List<StateProvince> states { get; set; }

        public bool potatoApp { get; set; }
        public static async Task<AdminPSViewModel> Create(CCIAContext _dbContext, int psId)
        {
           var ps = await _dbContext.PlantingStocks.Where(p => p.PsId == psId)                
                .Include(p => p.PsClassNavigation)
                .FirstAsync();  
            var statesWNull = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();
            statesWNull.Insert(0, new StateProvince{ StateProvinceId = 0, Name = "Select State/Country"});
            var viewModel = new AdminPSViewModel
            {
                plantingStocks = ps,
                psClass = await _dbContext.AbbrevClassProduced.Include(c => c.AppType).OrderBy(c => c.AppType.AppTypeTrans).ThenBy(c => c.SortOrder).ToListAsync(),
                states = statesWNull,
                potatoApp = await _dbContext.Applications.Where(a => a.Id == ps.AppId).AnyAsync(a => a.AppType == "PO"),
            };           

            return viewModel;
        }

        
    } 
}
