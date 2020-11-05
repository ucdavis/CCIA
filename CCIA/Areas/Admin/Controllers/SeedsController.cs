using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
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

        [HttpPost]
        public async Task<IActionResult> AcceptSeed(IFormCollection form)
        {
            // ToDo set up notifications; 
            var id = int.Parse(form["seed.Id"].ToString());
            var seedToAccept = await _dbContext.Seeds.Where(s => s.Id == id).FirstAsync();
            seedToAccept.Confirmed = true;
            seedToAccept.ConfirmedAt = DateTime.Now;
            seedToAccept.Status = SeedsStatus.SIRReady.GetDisplayName();
            
            await _dbContext.SaveChangesAsync();
            Message = "Seed Accepted";

            await _dbContext.Database.ExecuteSqlCommandAsync("accept_seeds_post_action @p0", id);

            return  RedirectToAction(nameof(Pending));
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Details(int id)
        {  
            var model = await AdminSeedsViewModel.CreateDetails(_dbContext, id);
            return View(model);
        }
    }
}