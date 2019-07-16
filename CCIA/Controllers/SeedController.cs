using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.SeedsViewModels;
using CCIA.Models.IndexViewModels;
using CCIA.Models.SeedsCreateViewModel;



namespace CCIA.Controllers
{
    public class SeedsController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public SeedsController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.Seeds.Where(s => s.ConditionerId == orgId).Select(s => s.CertYear.Value).MaxAsync();
            }
            var model = await SeedsIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            var model = await ClientSeedsViewModel.Create(_dbContext, orgId, id);
            return View(model);
        }

        // GET: Application/Create
        public ActionResult SelectOrigin()
        {
            // TODO: Check that logged in user has permission to create seeds.
            return View();
        }

        [HttpPost]
        public ActionResult SelectOrigin(string origin)
        {
            if(origin == "Ca")
            {
                return RedirectToAction("SelectApp");
            }
            if(origin == "OOS")
            {
                return RedirectToAction("NewOOSSeedLot");
            }
            return View();
        }

        public ActionResult SelectApp()
        {
            int[] years = Enumerable.Range(2007, CertYearFinder.CertYear - 2007 + 1).ToArray();
            return View(years);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInState(int[] appId)
        {
            var model = await SeedsCreateViewModel.Create(_dbContext, appId);
           
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAppsFromCertNumber(int certYear, int? rad, int certNumber)
        {
            if(rad.HasValue){
                var certs = await _dbContext.CertRad.Where(c => c.CertYear == certYear && c.CertNum == certNumber && c.Rad == rad).FirstAsync();
                var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.CertNum == certs.CertNum)
                    .Include(a => a.GrowerOrganization)
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.OrgName, acres = a.AcresApplied })
                    .ToListAsync();
                if(model != null)
                {
                    return Json(model);
                }                
            } else
            {
               var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.CertNum == certNumber)
                    .Include(a => a.GrowerOrganization)
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.OrgName, acres = a.AcresApplied })
                    .ToListAsync();
               if(model != null)
                {
                    return Json(model);
                } 
            }
            return BadRequest();
        }
    }
}