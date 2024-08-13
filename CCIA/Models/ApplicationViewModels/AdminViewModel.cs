using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
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

        public List<FIRDocuments> documents { get; set; }

        public List<Ecoregions> Ecoregions { get; set; }

        public List<StatesAndCountries> StatesAndCountries { get; set; }

        public List<Subspecies> Subspecies { get; set; }

        public static async Task<AdminViewModel> CreateDetails(CCIAContext _dbContext, int id, IFullCallService _helper)
        {   
            var p0 = new SqlParameter("@app_id", id);
            var viewModel = new AdminViewModel
            {
                application =  await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync(), 
                conflicts =  await _dbContext.IsolationConflicts.FromSqlRaw($"EXEC check_for_isolation_in_apps_mvc @app_id", p0).ToListAsync()
            };           

            return viewModel;
        }

        public static async Task<AdminViewModel> CreateEdit(CCIAContext _dbContext, int id, IFullCallService _helper)
        {
            var app = await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
            var viewModel = new AdminViewModel
            {
                application =  app,
                AppTypes = await _dbContext.AbbrevAppType.Select(a => a.Abbreviation).ToListAsync(),
                Crops = await _dbContext.Crops.Where(c => c.CertifiedCrop || c.Heritage || c.PreVarietyGermplasm || c.LacTracker || c.Crop == "hemp").OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),   
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync(),   
                Ecoregions = await _dbContext.Ecoregions.OrderBy(e => e.Name).ToListAsync(),
                StatesAndCountries = await _dbContext.StatesAndCountries.Where(s => s.Ord == 1).OrderBy(s => s.Name).ToListAsync(),
                Subspecies = await _dbContext.Subspecies.Where(s => s.CropId == app.CropId).OrderBy(s => s.Name).ToListAsync(),
            };           

            return viewModel;
        }

        public static async Task<AdminViewModel> CreateFIR(CCIAContext _dbContext, int id, IFullCallService _helper)
        {                 
            var viewModel = new AdminViewModel
            {
                application =  await _helper.FIRApplications().Where(a => a.Id == id).FirstOrDefaultAsync(),    
                documents = await _dbContext.FIRDocuments.Where(d => d.AppId == id).ToListAsync(),
            };    
            if(viewModel.application.FieldInspectionReport == null)
            {
                var newFIR = new FieldInspectionReport();
                newFIR.AppId = viewModel.application.Id;                        
                _dbContext.Add(newFIR);
                viewModel.application = await _helper.FIRApplications().Where(a => a.Id == id).FirstOrDefaultAsync();
            }       

            return viewModel;
        }


    } 
}
