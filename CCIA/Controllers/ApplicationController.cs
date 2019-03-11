using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace CCIA.Controllers
{
    public class ApplicationController : Controller
    {
        private readonly CCIAContext _dbContext;

        public ApplicationController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            if (certYear == 0)
            {
                certYear = CertYearFinder.CertYear;
            }
            var orgId = await _dbContext.Contacts.Where(c => c.ContactId == 1).Select(c => c.OrgId).ToArrayAsync();
            var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && orgId.Contains(a.ApplicantId))
                .Include(a => a.GrowerOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .ToListAsync();
            return View(model);
        }

        // GET: Application/Details/5
         public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.ContactId == 1).Select(c => c.OrgId).ToArrayAsync();
            var model = await _dbContext.Applications.Where(a => a.AppId == id && orgId.Contains(a.ApplicantId))
                .Include(a => a.GrowerOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.Certificates)
                .Include(a => a.PlantingStocks)
                .ThenInclude(p => p.PsClassNavigation)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedStateProvince)                              
                .Include(a => a.FieldHistories).ThenInclude(fh => fh.FHCrops)
                .FirstOrDefaultAsync();
            return View(model);
        }

        // GET: Application/CreateSeedApplication
        public async Task<IActionResult> CreateSeedApplication(int id)
        {
            var model = await ApplicationViewModel.Create(_dbContext, id);
            return View(model);
        }

        // GET: Application/CreateSeedApplication
        public IActionResult GrowerLookup()
        {
            return View();
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(SeedPostModel seedApp)
        {
            if (ModelState.IsValid) {
                // Add to db
                
                return RedirectToAction("Index");
            }
            return Json(ModelState.Values);
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

        // POST: Application/Lookup
        [HttpPost]
        public async Task<JsonResult> Lookup(String lookupVal)
        {
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID instead of a name)
            if (Int32.TryParse(lookupVal, out id)) {
                 
                orgs = await _dbContext.Organizations.Where(o => o.OrgId == id)
                    .Select(o => new Organizations {
                        OrgId = o.OrgId,
                        OrgName = o.OrgName,
                        Address = o.Address != null ? o.Address : new Address() 
                    })
                    .ToListAsync();
            }
            else {
                 orgs = await _dbContext.Organizations.Where(o => o.OrgName.Contains(lookupVal.ToLower()))
                    .Select(o => new Organizations {
                        OrgId = o.OrgId,
                        OrgName = o.OrgName,
                        Address = o.Address != null ? o.Address : new Address()       
                    })
                    .ToListAsync();
            }
            return Json(orgs);
        }

        // POST: Application/FindStateProvince
        [HttpPost]
        public async Task<JsonResult> FindStateProvince(int code)
        {
            // we could try constructing an object of key val pairs with the ids as the keys and states as the vals
            ModelState.Clear();
            var state_province = await _dbContext.StateProvince.Where(sp => sp.StateProvinceId == code)
                .Select(sp => sp.StateProvinceCode).ToListAsync();
            return Json(state_province);
        }
    }
}