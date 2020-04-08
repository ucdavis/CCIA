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
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CCIA.Controllers.Admin
{

    public class ApplicationController : AdminController
    {
        private readonly CCIAContext _dbContext;

        public ApplicationController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        public async Task<IActionResult> Index(int certYear)
        {            
            if (certYear == 0)
            {
                certYear = CertYearFinder.CertYear;
            }
            var model = await AdminApplicationIndexViewModel.Create(_dbContext, certYear);
            return View(model);
        }

         public async Task<IActionResult> Pending()
        {            
            var model =  await _dbContext.Applications.Where(a => a.Status == "Pending acceptance")
                .Include(a => a.GrowerOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.FieldResults)
                .ToListAsync();                        

            return View(model);
        }

        public async Task<IActionResult> Renew()
        {
            var certYear = CertYearFinder.CertYear;
            var model = await _dbContext.RenewFields.Where(r => r.Year == certYear)
                .Include(r => r.Application)
                .ThenInclude(a => a.GrowerOrganization)
                .Include(r => r.Application)
                .ThenInclude(a => a.ApplicantOrganization)
                .Include(r => r.Application)
                .ThenInclude(a => a.Crop)
                .Include(r => r.Application)
                .ThenInclude(a => a.Variety)
                .Include(r => r.Application)
                .ThenInclude(a => a.ClassProduced)
                .ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Renew_app(int id)
        {
            var appToRenew = await _dbContext.Applications.Where(a => a.Id == id).FirstAsync();
            var newApp = new Applications();
            
            newApp.PaperAppNum = appToRenew.Id;
            newApp.CertNum = appToRenew.CertNum;
            newApp.CertYear = CertYearFinder.CertYear;
            newApp.OriginalCertYear = appToRenew.OriginalCertYear;
            newApp.AppType = appToRenew.AppType;
            newApp.ApplicantId = appToRenew.ApplicantId;
            newApp.GrowerId = appToRenew.GrowerId;
            newApp.CropId = appToRenew.CropId;
            newApp.SelectedVarietyId = appToRenew.SelectedVarietyId;
            newApp.EnteredVariety = appToRenew.EnteredVariety;
            newApp.ClassProducedId = appToRenew.ClassProducedId;
            newApp.Received = DateTime.Now;
            newApp.Postmark = DateTime.Now;
            newApp.Deadline = DateTime.Now.AddDays(1);
            newApp.Status = "Pending acceptance";
            
		//  acres_applied, renewal (1_),  field_name, farm_county, user_app_dataentry (0),
		// maps, maps_sub_dt, map_center_lat, map_center_long, map_ve, map_upload_file, text_field, map_zoom,
		// date_planted, tags, geo_text_field, geo_field)

            return  RedirectToAction(nameof(Renew));;
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {   
            var model = await _dbContext.Applications.Where(a => a.Id == id)
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