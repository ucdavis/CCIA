using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using CCIA.Services;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Controllers
{
    public class OrganizationsController : AdminController
    {

        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public OrganizationsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        // TODO: Add Roles - Seasonal Field get nothing in here. View (can edit phone, fax, website). EditOrg has full edit & create. CondStatus allows you to update conditioner status settings.

        public async Task<IActionResult> Index(string term = "")
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return View(null);
            }
            var model = await _helper.FullOrg().Where(o => EF.Functions.Like(o.Id.ToString(), "%" + term + "%") || EF.Functions.Like(o.Name, "%" + term + "%")).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullOrg().Where(o => o.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

       
       
    }
}
