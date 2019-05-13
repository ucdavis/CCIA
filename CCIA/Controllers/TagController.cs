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



namespace CCIA.Controllers
{
    public class TagController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public TagController(CCIAContext dbContext)
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
            var orgId = await _dbContext.Contacts.Where(c => c.ContactId == 1).Select(c => c.OrgId).SingleAsync();
            var model = await _dbContext.Tags.Where(t => t.TaggingOrg == orgId && t.DateRequested.Value.Year == certYear)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Variety)                
                .ThenInclude(v => v.Crop)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.ClassProduced)
                .Include(t => t.Blend) 
                .ThenInclude(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from knownh app) => indirt => application => variety
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Variety)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                .ThenInclude(i => i.Application) 
                .ThenInclude(a => a.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                .ThenInclude(i => i.Crop)
                .Include(t => t.Blend)
                .ThenInclude(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                .ThenInclude(i => i.Variety)
                .Include(t => t.Blend)
                .ThenInclude(b => b.Variety) // blendrequest (varietal) => variety => crop
                .ThenInclude(v => v.Crop)
                .Include(t => t.TagAbbrevClass)
                .Include(t => t.AbbrevTagType)
                .ToListAsync();            
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.ContactId == 1).Select(c => c.OrgId).SingleAsync();
            var model = await ClientSeedsViewModel.Create(_dbContext, orgId, id);
            return View(model);
        }

        // GET: Application/Create
        public ActionResult Create()
        {
            return View();
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
    }
}