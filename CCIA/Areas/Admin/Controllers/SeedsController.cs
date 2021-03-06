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
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Controllers.Admin
{

    public class SeedsController : AdminController
    {
        private readonly CCIAContext _dbContext;

        private readonly IFullCallService _helper;

        public SeedsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public async Task<IActionResult> Pending()
        {
            var model = await _helper.OverviewSeeds().Where(s => s.Status == "Pending Acceptance").ToListAsync();
            return View(model);
        }

        // TODO: Edit seed lots!
        // TODO: Add ability to associate applications with a SID

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
           
            var p0 = new SqlParameter("@seeds_id", id);
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC accept_seeds_post_action @seeds_id", p0);

            return  RedirectToAction(nameof(Pending));
        }

        public ActionResult Index()
        {
            return View();
        }

        
        public async Task<IActionResult> Details(int id)
        {  
            var model = await AdminSeedsViewModel.CreateDetails(_dbContext, id, _helper);
            return View(model);
        }

        public ActionResult Seeds()
        {
            return View();
        }

        public ActionResult Certs()
        {           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Certs(IFormCollection form)
        {
            var certs = _helper.FullCerts();
            var entry = form["id"];
            var how = form["searchHow"];
            switch (how)
            {
                case "Exact":
                    certs = certs.Where(c => c.CertNum == int.Parse(entry));
                    break;
                case "Begin" :
                    certs = certs.Where(c => c.CertNum.ToString().StartsWith(entry));
                    break;
                case "End" :
                    certs = certs.Where(c => c.CertNum.ToString().EndsWith(entry));
                    break;
                case "Contain" :
                    certs = certs.Where(c => c.CertNum.ToString().Contains(entry));
                    break;
                default:
                    certs = certs.Where(c => c.CertNum == -1);
                    break;
            }            
            
            var model = await certs.ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> WeightLog(int year)
        {
            if(year == 0)
            {
                year = CertYearFinder.CertYear;
            }

            var model = await WeightLogViewModel.Create(_dbContext, year);
            return View(model);

        }

         public async Task<IActionResult> Search(int id, AdminSeedSearchViewModel vm)
        {
            if(!vm.Search){
                var freshmodel = await AdminSeedSearchViewModel.Create(_dbContext, null);
                return View(freshmodel);  
            }
                var model = await AdminSeedSearchViewModel.Create(_dbContext, vm);                
                return View(model);            
        }  
    }
}