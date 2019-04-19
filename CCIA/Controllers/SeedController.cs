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
    public class SeedsController : Controller
    {
        private readonly CCIAContext _dbContext;

        public SeedsController(CCIAContext dbContext)
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
            var model = await _dbContext.Seeds.Where(s => s.CertYear == certYear && orgId.Contains(s.ConditionerId))
                .Include(a => a.ApplicantOrganization)
                .Include(v => v.Variety)
                .ThenInclude(v => v.Crop)
                .Include(c => c.ClassProduced)
                .Include(l => l.LabResults)
                .ToListAsync();
            return View(model);
        }

        // GET: Application/Details/5
         public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.ContactId == 1).Select(c => c.OrgId).ToArrayAsync();
            var model = await _dbContext.Seeds.Where(s => s.Id == id && orgId.Contains(s.ConditionerId))
                .FirstOrDefaultAsync();           
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