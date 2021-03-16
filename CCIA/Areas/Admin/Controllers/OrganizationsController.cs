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

        public async Task<IActionResult> Index(string term = "")
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return View(null);
            }
            var model = await _helper.FullOrg().Where(o => EF.Functions.Like(o.OrgId.ToString(), "%" + term + "%") || EF.Functions.Like(o.OrgName, "%" + term + "%")).ToListAsync();

            return View(model);
        }

       
       
    }
}
