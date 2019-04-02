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
        public async Task<IActionResult> CreateSeedApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
        }

        // GET: Application/CreatePotatoApplication
        public async Task<IActionResult> CreatePotatoApplication(int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return View(model);
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
            var model = await _dbContext.AbbrevAppType.Where(a => a.AppTypeId == appTypeId).FirstOrDefaultAsync();
            return View(model);
        }

        // POST: Application/CreateSeedApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeedApplication(SeedPostModel seedApp)
        {
            if (ModelState.IsValid) {
                // Create new db entry

                var app = new Applications() {
                    AcresApplied = seedApp.AcresApplied,
                    ApplicantId = seedApp.GrowerId,
                    AppOriginalCertYear = seedApp.CropYear,
                    AppType = "SD",              
                    CertNum = seedApp.CertLotNum,
                    CertYear = seedApp.CropYear,
                    ClassProducedId = seedApp.ClassProduced,
                    CropId = seedApp.Crop,
                    DatePlanted = seedApp.DatePlanted,
                    FarmCounty = seedApp.County,
                    EnteredVariety = seedApp.Variety,
                    FieldName = seedApp.NameOrNum,
                    GrowerId = seedApp.GrowerId,
                    MapVe = false,
                    SelectedVarietyId = seedApp.SelectedVarietyId,
                    Status = "Pending supporting material",
                    WarningFlag = false
                };
                
                _dbContext.Add(app);
                _dbContext.SaveChanges();
                return RedirectToAction("Index");
            }
            var model = await ApplicationViewModel.Create(_dbContext, seedApp.GrowerId, 1);
            return View(model);
            // return Json(ModelState.Values);
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
            ModelState.Clear();
            var state_province = await _dbContext.StateProvince.Where(sp => sp.StateProvinceId == code)
                .Select(sp => sp.StateProvinceCode).ToListAsync();
            return Json(state_province);
        }

        // POST: Application/FindVariety
        [HttpPost]
        public async Task<JsonResult> FindVariety(string name) 
        {
            var varieties = await _dbContext.VarOfficial.Where(v => v.VarOffName.ToLower() == name.ToLower())
                .Select(v => new VarOfficial {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // POST: Application/FindCropVarieties
        [HttpPost]
        public async Task<JsonResult> FindCropVarieties(int cropId) 
        {
            var varieties = await _dbContext.VarOfficial.Where(v => v.CropId == cropId)
                .Select(v => new VarOfficial {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // POST: Application/FindGermplasmEntities
        [HttpPost]
        public async Task<JsonResult> FindGermplasmEntities(string name) 
        {
            var varieties = await _dbContext.VarOfficial
                .Where(v => v.VarOffName.ToLower() == name.ToLower())
                .Where(v => v.GermplasmEntity == true)
                .Select(v => new VarOfficial {
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
    }
}