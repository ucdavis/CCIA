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



namespace CCIA.Controllers
{
    enum AppTypes
    {
        SEED = 1,
        POTATO,
        GRAINQA,
        GERMPLASM,
        RICE,
        TURFGRASS,
        HEMP
    }


    public class ApplicationController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public ApplicationController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).FirstAsync();
            var bud = 10;
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
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).ToArrayAsync();
            var model = await _dbContext.Applications.Where(a => a.Id == id && orgId.Contains(a.ApplicantId))
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
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View("Seed/CreateSeedApplication", model);
        }

        // POST: Application/CreateSeedApplication
        [HttpPost]
        public async Task<IActionResult> CreateSeedApplication(Applications seedApp)
        {
            if (ModelState.IsValid)
            {
                // Remove invalid fieldhistories
                List<FieldHistory> newFieldHistories = new List<FieldHistory>();
                foreach (var fh in seedApp.FieldHistories)
                {
                    if (fh.Year != 0 && fh.Crop != null)
                    {
                        newFieldHistories.Add(fh);
                    }
                }
                seedApp.FieldHistories = newFieldHistories;
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == seedApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateAppRecord(seedApp, contactId, "SD");
                _dbContext.Add(app);
                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Create class produced object
                app.ClassProduced = new AbbrevClassProduced()
                {
                    ClassProducedId = (int)app.ClassProducedId
                };

                // Planting stocks
                CreateFirstPlantingStocksRecord(app.PlantingStocks.ElementAt(0), app);

                // Create second plantingstocks entry if required fields aren't null.
                var ps2 = app.PlantingStocks.ElementAt(1);
                if (ps2.PsCertNum != null && ps2.PoundsPlanted != null
                    && ps2.PsClass != null && ps2.StateCountryGrown != null
                    && ps2.StateCountryTagIssued != null)
                {
                    CreateSecondPlantingStocksRecord(app.PlantingStocks.ElementAt(1), app);
                }

                // Field history
                CreateFieldHistoryRecords(app.FieldHistories, app);

                _dbContext.Add(app);
                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var model = await ApplicationViewModel.Create(_dbContext, (int)seedApp.GrowerId, (int)AppTypes.SEED);
            model.RenderFormRemainder = true;
            Message = "You are missing certain required fields.";
            return View("Seed/CreateSeedApplication", model);
        }

        // GET: Application/CreatePotatoApplication
        public async Task<IActionResult> CreatePotatoApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View("Potato/CreatePotatoApplication", model);
        }

        // POST: Application/CreatePotatoApplication
        [HttpPost]
        public async Task<IActionResult> CreatePotatoApplication(PotatoPostModel potatoApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == potatoApp.GrowerId).FirstOrDefaultAsync();

                Applications app = CreatePotatoAppRecord(potatoApp, contactId, "PO");
                _dbContext.Add(app);
                await _dbContext.SaveChangesAsync();

                // CreateFirstPlantingStocksRecord(Potatoapp);

                // CreateFieldHistoryRecords(app);

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var model = await ApplicationViewModel.Create(_dbContext, potatoApp.GrowerId, 1);
            model.RenderFormRemainder = true;
            Message = "You are missing certain required fields.";
            return View("Potato/CreatePotatoApplication", model);
        }

        public Applications CreatePotatoAppRecord(PotatoPostModel potatoApp, int contactId, string appType)
        {
            return new Applications()
            {
                AcresApplied = potatoApp.AcresApplied,
                ApplicantComments = potatoApp.AdditionalInfo,
                ApplicantId = contactId,
                AppOriginalCertYear = potatoApp.CropYear,
                AppReceived = DateTime.Now,
                AppType = appType,
                CertYear = potatoApp.CropYear,
                ClassProducedId = potatoApp.ClassProduced,
                CropId = potatoApp.Crop,
                DatePlanted = potatoApp.DatePlanted,
                EnteredVariety = potatoApp.Variety,
                FarmCounty = potatoApp.County,
                FieldName = potatoApp.NameOrNum,
                GrowerId = potatoApp.GrowerId,
                MapVe = false,
                SelectedVarietyId = potatoApp.VarietyId,
                Status = "Pending supporting material",
                WarningFlag = false,

                FieldHistories = potatoApp.FieldHistories,
                // PlantingStocks = new List<PlantingStocks> {potatoApp.PlantingStock1, potatoApp.PlantingStock2}
            };
        }

        /* Iterates through FieldHistories List to find valid entries,
        Stages those to be committed to the FieldHistories table
        Then sets our app's FieldHistories to be only the valid entries */
        private void CreateFieldHistoryRecords(ICollection<FieldHistory> fieldHistories, Applications app)
        {
            ICollection<FieldHistory> newFieldHistories = new List<FieldHistory>();
            // Iterate through fieldhistories and make a new record for each
            foreach (var fh in fieldHistories)
            {
                if (fh.Year != 0 && fh.Crop != null)
                {
                    fh.AppId = app.Id;
                    //fh.Application = app;
                    newFieldHistories.Add(fh);
                    _dbContext.Add(fh);
                }
            }
            app.FieldHistories = newFieldHistories;
        }

        private void CreateFirstPlantingStocksRecord(PlantingStocks ps, Applications app)
        {
            ps.PsEnteredVariety = app.EnteredVariety;
            ps.PsClass = app.ClassProducedId;
            ps.AppId = app.Id;
            //ps.Application = app;

            _dbContext.Add(ps);
        }

        private void CreateSecondPlantingStocksRecord(PlantingStocks ps, Applications app)
        {
            // var ps2 = new PlantingStocks()
            // {
            //     OfficialVarietyId = app.VarietyId,
            // };
            ps.PsClass = app.ClassProducedId;
            ps.AppId = app.Id;
            //ps.Application = app;

            _dbContext.Add(ps);
        }

        // GET: Application/CreateHeritageGrainApplication
        public async Task<IActionResult> CreateHeritageGrainApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreateGermplasmApplication
        public async Task<IActionResult> CreateGermplasmApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreateRiceApplication
        public async Task<IActionResult> CreateRiceApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreateTurfgrassApplication
        public async Task<IActionResult> CreateTurfgrassApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreateHempFromSeedApplication
        public async Task<IActionResult> CreateHempFromSeedApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreateHempFromClonesApplication
        public async Task<IActionResult> CreateHempFromClonesApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/GrowerLookup
        public async Task<IActionResult> GrowerLookup(int appTypeId)
        {
            // Get contact ID -- will correspond to logged-in user
            var contact = await _dbContext.ContactToOrg
                                .Where(c => c.ContactId == 1)
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
        public async Task<JsonResult> Lookup(String lookupVal)
        {
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID instead of a name)
            if (Int32.TryParse(lookupVal, out id))
            {

                orgs = await _dbContext.Organizations.Where(o => o.OrgId == id)
                    .Select(o => new Organizations
                    {
                        OrgId = o.OrgId,
                        OrgName = o.OrgName,
                        Address = o.Address != null ? o.Address : new Address()
                    })
                    .ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.OrgName.Contains(lookupVal.ToLower()))
                   .Select(o => new Organizations
                   {
                       OrgId = o.OrgId,
                       OrgName = o.OrgName,
                       Address = o.Address != null ? o.Address : new Address()
                   })
                   .ToListAsync();
            }
            return Json(orgs);
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

        public async Task<IActionResult> GetPartial(string folder, string partialName, int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            string fullPartialPath = $"~/Views/Application/{folder}/{partialName}.cshtml";
            return PartialView(fullPartialPath, model);
        }

    }
}