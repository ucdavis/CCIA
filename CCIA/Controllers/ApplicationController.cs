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

        // POST: Application/CreatePotatoApplication
        [HttpPost]
        public async Task<IActionResult> CreatePotatoApplication(PotatoPostModel potatoApp)
        {
            // var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            return Json(potatoApp);
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

        // POST: Application/CreateSeedApplication
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSeedApplication(SeedPostModel seedApp)
        {
            if (ModelState.IsValid) {
                // Create new db entry

                // Get contact id associated with growerid
                var contactId = await _dbContext.Contacts.Select(c => c.ContactId).Where(c => c == seedApp.GrowerId).FirstOrDefaultAsync();

                Applications app = CreateApplicationsRecord(seedApp, contactId, "SD");
                _dbContext.Add(app);
                await _dbContext.SaveChangesAsync();

                // Planting stocks entries
                var ps = new PlantingStocks() {
                    AbbrevClassProducedClassProducedId = seedApp.ClassProduced,
                    AppId = app.AppId,
                    PsCertNum = seedApp.CertLotNum,
                    PsEnteredVariety = seedApp.Variety,
                    OfficialVarietyId = seedApp.VarietyId,
                    PoundsPlanted = seedApp.PoundsPlanted,
                    PsClass = seedApp.ClassPlanted,
                    StateCountryTagIssued = seedApp.StateCountryTagIssued,
                    StateCountryGrown = seedApp.StateCountryStockGrown,
                    SeedPurchasedFrom = seedApp.SeedFrom,
                };

                _dbContext.Add(ps);

                // Create second plantingstocks entry if required fields aren't null.
                if (seedApp.CertLotNum2 != null && seedApp.PoundsPlanted2 != null
                    && seedApp.ClassPlanted2 != null && seedApp.StateCountryStockGrown2 != null
                    && seedApp.StateCountryTagIssued2 != null) 
                {
                    var ps2 = new PlantingStocks() {
                        AppId = app.AppId,
                        PsCertNum = seedApp.CertLotNum2,
                        PsEnteredVariety = seedApp.Variety2,
                        OfficialVarietyId = seedApp.VarietyId,
                        PoundsPlanted = seedApp.PoundsPlanted2,
                        PsClass = seedApp.ClassPlanted2,
                        StateCountryTagIssued = seedApp.StateCountryTagIssued2,
                        StateCountryGrown = seedApp.StateCountryStockGrown2,
                        SeedPurchasedFrom = seedApp.SeedFrom,
                    };

                    _dbContext.Add(ps2);
                }

                // Field History
                if (seedApp.HistoryCropYear1 != null && seedApp.HistoryCrop1 != null)
                {
                    var fieldHistory1 = CreateFieldHistory1Record(app.AppId, seedApp);
                    _dbContext.Add(fieldHistory1);
                }
                if (seedApp.HistoryCropYear2 != null && seedApp.HistoryCrop2 != null)
                {
                    var fieldHistory2 = CreateFieldHistory2Record(app.AppId, seedApp);
                    _dbContext.Add(fieldHistory2);
                }
                if (seedApp.HistoryCropYear3 != null && seedApp.HistoryCrop3 != null)
                {
                    var fieldHistory3 = CreateFieldHistory3Record(app.AppId, seedApp);
                    _dbContext.Add(fieldHistory3);
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.AppId});
            }
            var model = await ApplicationViewModel.Create(_dbContext, seedApp.GrowerId, 1);
            model.RenderFormRemainder = true;
            Message = "You are missing certain required fields.";
            // Message = ConcatenateErrors(ModelState);
            // return Json(ModelState);
            return View(model);
        }

        public Applications CreateApplicationsRecord(SeedPostModel seedApp, int contactId, string appType) {
            return new Applications() {
                AcresApplied = seedApp.AcresApplied,
                ApplicantComments = seedApp.AdditionalInfo,
                ApplicantId = contactId,
                AppOriginalCertYear = seedApp.CropYear,
                AppReceived = DateTime.Now,
                AppType = appType,
                CertYear = seedApp.CropYear,
                ClassProducedId = seedApp.ClassProduced,
                CropId = seedApp.Crop,
                DatePlanted = seedApp.DatePlanted,
                EnteredVariety = seedApp.Variety,
                FarmCounty = seedApp.County,
                FieldName = seedApp.NameOrNum,
                GrowerId = seedApp.GrowerId,
                MapVe = false,
                SelectedVarietyId = seedApp.VarietyId,
                Status = "Pending supporting material",
                WarningFlag = false
            };
        }

        public FieldHistory CreateFieldHistory1Record(int appId, SeedPostModel seedApp) {
            return new FieldHistory() {
                AppId = appId,
                Year = seedApp.HistoryCropYear1,
                Crop = seedApp.HistoryCrop1,
                Variety = seedApp.HistoryVarietyCrop1,
                AppNumber = seedApp.HistoryApplicationNum1
            };
        }

        public FieldHistory CreateFieldHistory2Record(int appId, SeedPostModel seedApp) {
            return new FieldHistory() {
                AppId = appId,
                Year = seedApp.HistoryCropYear2,
                Crop = seedApp.HistoryCrop2,
                Variety = seedApp.HistoryVarietyCrop2,
                AppNumber = seedApp.HistoryApplicationNum2
            };
        }

        public FieldHistory CreateFieldHistory3Record(int appId, SeedPostModel seedApp) {
            return new FieldHistory() {
                AppId = appId,
                Year = seedApp.HistoryCropYear3,
                Crop = seedApp.HistoryCrop3,
                Variety = seedApp.HistoryVarietyCrop3,
                AppNumber = seedApp.HistoryApplicationNum3
            };
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
                .Select(v => new VarOfficial {
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
                .Select(v => new VarOfficial {
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

        public async Task<IActionResult> GetPartial(string partialName, int orgId, int appTypeId)
        {
            var model = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            // string fullPartialPath = "~/Views/Application/" + partialName + ".cshtml";
            return PartialView(partialName, model);
        }

        // private string ConcatenateErrors(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary ModelState)
        // {
        //     string res = "";
        //     foreach (var error in ModelState.Values.SelectMany(modelState => modelState.Errors))
        //     {
        //         res += (error.ToString() + "\n");
        //     }
        //     return res;
        // }
    }
}