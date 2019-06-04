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
        public async Task<IActionResult> CreateSeedApplication(SeedApp seedApp)
        {
            if (ModelState.IsValid)
            {
                // seedApp.FieldHistories = ApplicationPostMap.RemoveInvalidFieldHistories(seedApp);

                seedApp.PlantingStocks = ApplicationPostMap.RemoveInvalidPlantingStocks<SeedPlantingStocks>(seedApp.PlantingStocks);

                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == seedApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateAppRecord(seedApp, contactId, "SD");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                foreach (FieldHistory fh in app.FieldHistories)
                {
                    fh.AppId = app.Id;
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var model = await ApplicationViewModel.Create(_dbContext, (int)seedApp.GrowerId, (int)AppTypes.SEED);
            model.RenderFormRemainder = true;

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
        public async Task<IActionResult> CreatePotatoApplication(PotatoApp potatoApp)
        {
            if (ModelState.IsValid)
            {
                // Remove invalid fieldhistories
                List<FieldHistory> newFieldHistories = new List<FieldHistory>();
                foreach (var fh in potatoApp.FieldHistories)
                {
                    if (fh.Year != 0 && fh.Crop != null)
                    {
                        newFieldHistories.Add(fh);
                    }
                }
                potatoApp.FieldHistories = newFieldHistories;

                // Remove invalid plantingstocks
                List<SeedPlantingStocks> newPlantingStocks = new List<SeedPlantingStocks>();
                foreach (var ps in potatoApp.PlantingStocks)
                {
                    if (ps.PoundsPlanted != null || ps.PsCertNum != null || ps.PsClass != null)
                    {
                        newPlantingStocks.Add(ps);
                    }
                }
                potatoApp.PlantingStocks = newPlantingStocks;

                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.Id).Where(c => c == potatoApp.GrowerId).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateAppRecord(potatoApp, contactId, "SD");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                foreach (FieldHistory fh in app.FieldHistories)
                {
                    fh.AppId = app.Id;
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var model = await ApplicationViewModel.Create(_dbContext, (int)potatoApp.GrowerId, (int)AppTypes.SEED);
            model.RenderFormRemainder = true;

            return View("Seed/CreatepotatoApplication", model);
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