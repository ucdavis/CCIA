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

        public static async Task<AdminViewModel> CreateDetails(CCIAContext _dbContext, int id, IFullCallService _helper)
        {   
            var p0 = new SqlParameter("@app_id", id);
            var viewModel = new AdminViewModel
            {
                application =  await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync(), 
                conflicts =  await _dbContext.IsolationConflicts.FromSqlRaw($"EXEC check_for_isolation_in_apps_mvc @p0", p0).ToListAsync()
            };           

            return viewModel;
        }

        public static async Task<AdminViewModel> CreateEdit(CCIAContext _dbContext, int id, IFullCallService _helper)
        {    
            var viewModel = new AdminViewModel
            {
                application =  await _helper.FullApplications().Where(a => a.Id == id).FirstOrDefaultAsync(),
                AppTypes = await _dbContext.AbbrevAppType.Select(a => a.Abbreviation).ToListAsync(),
                Crops = await _dbContext.Crops.ToListAsync(),   
                Counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync(),     
            };           

            return viewModel;
        }

        public static async Task<AdminViewModel> CreateFIR(CCIAContext _dbContext, int id, IFullCallService _helper)
        {                   
            var app = await _helper.FIRApplications().Where(a => a.Id == id).FirstOrDefaultAsync();  
            var viewModel = new AdminViewModel
            {
                application = app                  
            };           

            return viewModel;
        }


    } 
}
