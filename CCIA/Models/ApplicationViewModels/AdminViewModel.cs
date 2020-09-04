using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.DetailsViewModels
{   
    public class AdminViewModel 
    {
        public Applications application { get; set; }
        public List<IsolationConflicts> conflicts { get; set; }

        public List<String> AppTypes { get; set; }

        public List<Crops> Crops { get; set; }

        public List<County> Counties { get; set; }

        public static async Task<AdminViewModel> CreateDetails(CCIAContext _dbContext, int id)
        {
           var app = await _dbContext.Applications.Where(a => a.Id == id)
                .Include(a => a.GrowerOrganization)
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
                .Include(a => a.Changes).ThenInclude(c => c.Employee)
                .FirstOrDefaultAsync();  
            var viewModel = new AdminViewModel
            {
                application = app, 
                conflicts = await _dbContext.IsolationConflicts.FromSql("dbo.check_for_isolation_in_apps_mvc @p0", id).ToListAsync()          
            };           

            return viewModel;
        }

        public static async Task<AdminViewModel> CreateEdit(CCIAContext _dbContext, int id)
        {                   
            var app = await _dbContext.Applications.Where(a => a.Id == id)
                .Include(a => a.GrowerOrganization)
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
                .Include(a => a.Changes).ThenInclude(c => c.Employee)
                .FirstOrDefaultAsync();  
            var viewModel = new AdminViewModel
            {
                application = app,
                AppTypes = await _dbContext.AbbrevAppType.Select(a => a.Abbreviation).ToListAsync(),
                Crops = await _dbContext.Crops.ToListAsync(),   
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync(),     
            };           

            return viewModel;
        }
    } 
}
