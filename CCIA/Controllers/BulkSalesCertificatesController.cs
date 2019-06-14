using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;



namespace CCIA.Controllers
{
    public class BulkSalesCertificatesController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public BulkSalesCertificatesController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {

            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.BulkSalesCertificates.Where(b => b.ConditionerOrganizationId == orgId).Select(b => b.Date.Year).MaxAsync();
            }
            var model = await BulkSalesCertificatesIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }

        // GET: Application/Details/5
        public ActionResult Details(int id)
        {
            return View();
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

        [HttpGet]
        public async Task<JsonResult> GetSidOrBlend(string lookupType, int id)
        {
            if (lookupType == "Blend")
            {
                var model = await _dbContext.BlendRequests.Where(b => b.BlendId == id)
                .Include(b => b.Conditioner)
                .Include(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from knownh app) => indirt => application => variety
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Variety)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                .ThenInclude(i => i.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                .ThenInclude(i => i.Variety)
                .Include(b => b.Variety) // blendrequest (varietal) => variety => crop
                .ThenInclude(v => v.Crop)
                .Select(b =>  new 
                { 
                    id = b.BlendId, 
                    appTtype = "Blend", 
                    applicant = b.ConditionerId + " " + b.Conditioner.OrgName,
                    conditioner = b.ConditionerId + " " + b.Conditioner.OrgName,
                    crop = b.GetCrop(),
                    variety = b.GetVarietyName(),
                }).SingleAsync();
                return Json(model);
            }
            else
            {
                var model = await _dbContext.Seeds.Where(s => s.Id == id)
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(c => c.ClassProduced)
                .Select(s =>  new 
                { 
                    id = s.Id, 
                    appType = "Seed",
                    applicant = s.ApplicantId + " " + s.ApplicantOrganization.OrgName,
                    conditioner = s.ConditionerId + " " + s.ConditionerOrganization.OrgName,
                    crop = s.GetCropName(),
                    variety = s.GetVarietyName(),
                }).SingleAsync();
                return Json(model);
            }

        }
    }
}