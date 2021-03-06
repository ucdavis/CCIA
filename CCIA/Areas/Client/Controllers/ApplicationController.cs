﻿using System;
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

namespace CCIA.Controllers.Client
{

    public class ApplicationController : ClientController
    {
        private readonly CCIAContext _dbContext;

        public ApplicationController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.Id).FirstAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.Applications.Where(a => a.ApplicantId == orgId).Select(a => a.CertYear).MaxAsync(); ;
            }
            var model = await ApplicationIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.Id).FirstAsync();
            var model = await _dbContext.Applications.Where(a => a.Id == id && a.ApplicantId == orgId)
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
        public async Task<IActionResult> CreateSeedApplication(int orgId, int appTypeId)
        {
            var model = new SeedApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Seed/CreateSeedApplication", model);
        }

        // POST: Application/CreateSeedApplication
        [HttpPost]
        public async Task<IActionResult> CreateSeedApplication(SeedApp seedApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == seedApp.GrowerId).FirstOrDefaultAsync();
                var days = await _dbContext.Crops.Where(c => c.CropId == seedApp.CropId).Select(c => c.AppDue).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateSeedAppRecord(seedApp, contactId, "SD");
                if (app.DatePlanted.HasValue && days.HasValue)
                {
                    app.Deadline = app.DatePlanted.Value.AddDays(days.Value); 
                }
                
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)seedApp.GrowerId, (int)AppTypes.SEED);
            if (SecondPlantingStockErrors())
            {
                appViewModel.RenderSecondPlantingStock = true;
            }
            appViewModel.RenderFormRemainder = true;

            // Newtonsoft.Json.Linq.JArray fhIndices = Newtonsoft.Json.Linq.JArray.Parse(seedApp.AppViewModel.FieldHistoryIndicesStr);
            appViewModel.FieldHistoryIndices = seedApp.AppViewModel.FieldHistoryIndices;
            seedApp.AppViewModel = appViewModel;
            return View("Seed/CreateSeedApplication", seedApp);
        }

        private bool SecondPlantingStockErrors()
        {
            foreach (var key in ModelState.Keys)
            {
                var val = ModelState[key];
                foreach (var error in val.Errors)
                {
                    if (key.Contains("PlantingStocks[1]"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // GET: Application/CreatePotatoApplication
        public async Task<IActionResult> CreatePotatoApplication(int orgId, int appTypeId)
        {
            var model = new PotatoApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Potato/CreatePotatoApplication", model);
        }

        // POST: Application/CreatePotatoApplication
        [HttpPost]
        public async Task<IActionResult> CreatePotatoApplication(PotatoApp potatoApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == potatoApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreatePotatoAppRecord(potatoApp, contactId, "PO");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)potatoApp.GrowerId, (int)AppTypes.POTATO);
            appViewModel.RenderFormRemainder = true;
            potatoApp.AppViewModel = appViewModel;
            return View("Potato/CreatePotatoApplication", potatoApp);
        }

        // GET: Application/CreateHeritageGrainApplication
        public async Task<IActionResult> CreateHeritageGrainApplication(int orgId, int appTypeId)
        {
            var model = new HeritageGrainQAApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("HeritageGrain/CreateHeritageGrainApplication", model);
        }

        // POST: Application/CreateHeritageGrainApplication
        [HttpPost]
        public async Task<IActionResult> CreateHeritageGrainApplication(HeritageGrainQAApp heritageGrainApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == heritageGrainApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateHeritageGrainAppRecord(heritageGrainApp, contactId, "GQ");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)heritageGrainApp.GrowerId, (int)AppTypes.GRAINQA);
            appViewModel.RenderFormRemainder = true;
            heritageGrainApp.AppViewModel = appViewModel;
            return View("HeritageGrain/CreateheritageGrainApplication", heritageGrainApp);
        }

        // GET: Application/CreateGermplasmApplication
        public async Task<IActionResult> CreatePreVarietyGermplasmApplication(int orgId, int appTypeId)
        {
            var model = new PreVarietyGermplasmApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("PreVarietyGermplasm/CreatePreVarietyGermplasmApplication", model);
        }

        // POST: Application/CreatePreVarietyGermplasmApplication
        [HttpPost]
        public async Task<IActionResult> CreatePreVarietyGermplasmApplication(PreVarietyGermplasmApp preVarietyGermplasmApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == preVarietyGermplasmApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreatePreVarietyGermplasmAppRecord(preVarietyGermplasmApp, contactId, "PV");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)preVarietyGermplasmApp.GrowerId, (int)AppTypes.GERMPLASM);
            appViewModel.RenderFormRemainder = true;
            preVarietyGermplasmApp.AppViewModel = appViewModel;
            return View("PreVarietyGermplasm/CreatepreVarietyGermplasmApplication", preVarietyGermplasmApp);
        }

        // GET: Application/CreateRiceApplication
        public async Task<IActionResult> CreateRiceApplication(int orgId, int appTypeId)
        {
            var model = new RiceQAApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Rice/CreateRiceApplication", model);
        }

        // POST: Application/CreateRiceApplication
        [HttpPost]
        public async Task<IActionResult> CreateRiceApplication(RiceQAApp riceApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == riceApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateRiceAppRecord(riceApp, contactId, "RQ");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)riceApp.GrowerId, (int)AppTypes.RICE);
            appViewModel.RenderFormRemainder = true;
            riceApp.AppViewModel = appViewModel;
            return View("Rice/CreatericeApplication", riceApp);
        }

        // GET: Application/CreateHempFromSeedApplication
        public async Task<IActionResult> CreateHempFromSeedApplication(int orgId, int appTypeId)
        {
            var model = new HempFromSeedApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("HempFromSeed/CreateHempFromSeedApplication", model);
        }

        // POST: Application/CreateHempFromSeedApplication
        [HttpPost]
        public async Task<IActionResult> CreateHempFromSeedApplication(HempFromSeedApp hempFromSeedApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == hempFromSeedApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateHempFromSeedAppRecord(hempFromSeedApp, contactId, "HS");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)hempFromSeedApp.GrowerId, (int)AppTypes.HEMP);
            appViewModel.RenderFormRemainder = true;
            hempFromSeedApp.AppViewModel = appViewModel;
            return View("HempFromSeed/CreatehempFromSeedApplication", hempFromSeedApp);
        }

        // GET: Application/GrowerLookup
        public async Task<IActionResult> GrowerLookup(int appTypeId)
        {
            // Get contact ID -- will correspond to logged-in user
            var contact = await _dbContext.Contacts
                                .Where(c => c.Id == 1)
                                .FirstOrDefaultAsync();

            // Check if grower is same as applicant
            var abbrevAppType = await _dbContext.AbbrevAppType
                            .Where(a => a.AppTypeId == appTypeId)
                            .FirstOrDefaultAsync();
            if (abbrevAppType.GrowerSameAsApplicant)
            {
                int orgId = Convert.ToInt32(contact.OrgId);
                return RedirectToAction("CreatePotatoApplication", new { orgId = orgId, appTypeId = appTypeId });
            }
            else
            {
                return View(abbrevAppType);
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

        // GET: Application/Lookup
        [HttpGet]
        public async Task<IActionResult> Lookup(string lookupVal, int appTypeId)
        {
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(lookupVal, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.Id == id)
                    .Include(o => o.Address).ThenInclude(a => a.StateProvince)
                    .ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.Name.Contains(lookupVal.ToLower()))
                    .Include(o => o.Address).ThenInclude(a => a.StateProvince)
                    .ToListAsync();
            }
            GrowerInfo growerInfo = new GrowerInfo();
            growerInfo.Orgs = orgs;
            growerInfo.AppTypeId = appTypeId;
            growerInfo.ActionType = ApplicationPostMap.ActionTypes[appTypeId];
            string fullPartialPath = "~/Client/Views/Application/Shared/_GrowerLookupInfoTable.cshtml";
            return PartialView(fullPartialPath, growerInfo);
        }

        // GET: Application/FindStateProvince
        [HttpGet]
        public async Task<JsonResult> FindStateProvince(int code)
        {
            ModelState.Clear();
            var state_province = await _dbContext.StateProvince.Where(sp => sp.StateProvinceId == code)
                .Select(sp => sp.StateProvinceCode).ToListAsync();
            return Json(state_province);
        }

        // GET: Application/FindVariety
        [HttpGet]
        public async Task<JsonResult> FindVariety(string name, int cropId)
        {
            var varieties = await _dbContext.VarOfficial
                .Where(v => v.VarOffName.ToLower() == name.ToLower())
                .Where(v => v.CropId == cropId)
                .Select(v => new VarOfficial
                {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/FindCropVarieties
        [HttpGet]
        public async Task<JsonResult> FindCropVarieties(int cropId)
        {
            var varieties = await _dbContext.VarOfficial.Where(v => v.CropId == cropId)
                .Select(v => new VarOfficial
                {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/FindGermplasmEntities
        [HttpGet]
        public async Task<JsonResult> FindGermplasmEntities(string name)
        {
            var varieties = await _dbContext.VarOfficial
                .Where(v => v.VarOffName.ToLower() == name.ToLower())
                .Where(v => v.GermplasmEntity == true)
                .Select(v => new VarOfficial
                {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/NewApp
        public async Task<IActionResult> NewApp()
        {
            var model = await _dbContext.AbbrevAppType
                .ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> GetPartial(string folder, string partialName, int orgId, int appTypeId, int fhEntryId = -1)
        {
            var app = ApplicationPostMap.CreateAppByAppType(appTypeId);
            var appViewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId, fhEntryId);
            app.AppViewModel = appViewModel;
            string fullPartialPath = $"~/Views/Application/{folder}/{partialName}.cshtml";
            ViewData["fhEntryId"] = fhEntryId;
            return PartialView(fullPartialPath, app);
        }

    }
}