using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;
using CCIA.Helpers;

namespace CCIA.Controllers.Admin
{

    public class OECDController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public OECDController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public ActionResult Index ()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();
            return View(model);
        }

        public async Task<IActionResult> Certificate(int id, bool charge)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();
            return View(model);
        }
       
    }
}