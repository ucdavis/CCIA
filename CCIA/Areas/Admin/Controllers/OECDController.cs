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

         public ActionResult Lookup()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminOECDEditCreateViewModel.Create(_dbContext, _helper, null, id);
            return View(model);
        }

        public async Task<IActionResult> Certificate(int id, bool charge)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();

            if(charge)
            {
                if(model.DatePrinted == null)
                {
                    model.DatePrinted = DateTime.Now;
                    model.UpdateUser = User.FindFirstValue(ClaimTypes.Name);
                    await _dbContext.SaveChangesAsync();
                    await _dbContext.Database.ExecuteSqlCommandAsync("charge_OECD @p0", model.Id);
                }
            }
            return View(model);
        }

        public async Task<IActionResult> Invoice(int id)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();

            return View(model);
        }

        public async Task<IActionResult> Search()
        {
            var model = await AdminOECDSearchViewModel.Create(_dbContext, null);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(AdminOECDSearchViewModel vm)
        {
            var model = await AdminOECDSearchViewModel.Create(_dbContext, vm);
            return View(model);
        }

        public async Task<ActionResult> ReportingSheets()
        {
            var model = await AdminOECDReportingViewModel.Create(_dbContext, null);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> ReportingSheets(AdminOECDReportingViewModel vm, string button)
        {
            if(button == "Mark uploaded")
            {
                await _dbContext.Database.ExecuteSqlCommandAsync("usda_mark_certs @p0, @p1", vm.startDate, vm.endDate);
                Message = "Lots marked as uploaded. Date clicked is recorded in case you need to reverse this. :)";
            }
            var model = await AdminOECDReportingViewModel.Create(_dbContext, vm);
            return View(model);
        }

       
    }
}