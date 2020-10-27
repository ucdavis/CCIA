using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;

namespace CCIA.Controllers.Admin
{

    public class SeedsController : AdminController
    {
        private readonly CCIAContext _dbContext;

        public SeedsController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Pending()
        {
            var model = await _dbContext.Seeds.Where(s => s.Status == "Pending Acceptance")
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(s => s.Application)
                .ThenInclude(a => a.Crop)
                .Include(s => s.Application)
                .Include(a => a.Variety)
                .Include(s => s.Variety)
                .ToListAsync();
            return View(model);
        }

        
        public async Task<IActionResult> Index(int certYear)
        {            
            if (certYear == 0)
            {
                certYear = await _dbContext.Applications.MaxAsync(a => a.CertYear);
            }
            var model = await AdminApplicationIndexViewModel.Create(_dbContext, certYear, false);
            return View(model);
        }
    }
}