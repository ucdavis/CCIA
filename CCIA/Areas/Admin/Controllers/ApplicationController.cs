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
using CCIA.Models.DetailsViewModels;

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
            var model = await AdminApplicationIndexViewModel.Create(_dbContext, certYear, false);
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

         public async Task<IActionResult> Accepted(int certYear)
        {            
            if (certYear == 0)
            {
                certYear = CertYearFinder.CertYear;
            }
            var model = await AdminApplicationIndexViewModel.Create(_dbContext, certYear, true);
            return View("Index", model);
        }

        public async Task<IActionResult> Renew()
        {
            var certYear = CertYearFinder.CertYear;
            var model = await _dbContext.RenewFields.Where(r => r.Year == certYear && r.Action == 0)
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
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
            var appToRenew = await _dbContext.Applications.Where(a => a.Id == renew.AppId).FirstAsync();
            var newApp =  MapRenewFromApp(appToRenew);   

            renew.Action = 1;
            renew.DateRenewed = DateTime.Now;

            // TODO add notifications

            _dbContext.Add(newApp);                
            _dbContext.Update(renew);
            await _dbContext.SaveChangesAsync();

            Message = $"App renewed. New App ID: {newApp.Id}";		
		
            return  RedirectToAction(nameof(Renew));;
        }

        public async Task<IActionResult> Renew_noseed(int id)
        {
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
            var appToRenew = await _dbContext.Applications.Where(a => a.Id == renew.AppId).FirstAsync();
            var newApp =  MapRenewFromApp(appToRenew);  

            newApp.Comments = "App renew, NO CROP";          

            renew.Action = 3;
            renew.DateRenewed = DateTime.Now;

            // TODO add notifications

            _dbContext.Add(newApp);                
            _dbContext.Update(renew);
            await _dbContext.SaveChangesAsync();

            Message = $"App renewed for NO SEED. New App ID: {newApp.Id}";		
		
            return  RedirectToAction(nameof(Renew));;
        }

        public async Task<IActionResult> Renew_cancel(int id)
        {
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
           
            renew.Action = 2;
            renew.DateRenewed = DateTime.Now;
            
            _dbContext.Update(renew);
            await _dbContext.SaveChangesAsync();

            Message = $"App renewal cancelled.";		
		
            return  RedirectToAction(nameof(Renew));;
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {   
            var model = await AdminViewModel.CreateDetails(_dbContext, id);
            return View(model);  
        }

        // GET: Application/CreateSeedApplication
      
        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminViewModel.CreateEdit(_dbContext, id);
            return View(model);            
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

        public async Task<IActionResult> LookupOrg (string lookup)
        {
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(lookup, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.OrgId == id).ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.OrgName.Contains(lookup.ToLower())).ToListAsync();
            }                        
            return PartialView("_LookupOrg", orgs);

        }

        public async Task<IActionResult> LookupVariety (string lookup, int cropId) 
        {
            var varieties = await _dbContext.VarFull.Where(v => v.CropId == cropId && v.Name.Contains(lookup)).ToListAsync();
            return PartialView("_LookupVariety", varieties);
        }

        public async Task<IActionResult> LookupClass(string appType)
        {
            int appId = await _dbContext.AbbrevAppType.Where(a => a.Abbreviation == appType).Select(a => a.AppTypeId).FirstAsync();
            var classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppType == appId).OrderBy(c => c.SortOrder).ToListAsync();
            return PartialView("_LookupClass", classes);
        }


        private Applications MapRenewFromApp(Applications appToRenew)
        {
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
            newApp.AcresApplied = appToRenew.AcresApplied;
            newApp.Renewal = true;
            newApp.FieldName = appToRenew.FieldName;
            newApp.FarmCounty = appToRenew.FarmCounty;
            newApp.UserDataentry = 0;
            newApp.Maps = appToRenew.Maps;
            newApp.MapsSubmissionDate = appToRenew.MapsSubmissionDate;
            newApp.MapCenterLat = appToRenew.MapCenterLat;
            newApp.MapCenterLong = appToRenew.MapCenterLong;
            newApp.MapVe = appToRenew.MapVe;
            newApp.MapUploadFile = appToRenew.MapUploadFile;
            newApp.TextField = appToRenew.TextField;
            newApp.GeoTextField = appToRenew.GeoTextField;
            newApp.GeoField = appToRenew.GeoField;
            newApp.DatePlanted = appToRenew.DatePlanted;
            newApp.Tags = appToRenew.Tags;

            return newApp;

        }

       

    }
}