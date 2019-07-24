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
using CCIA.Models.IndexViewModels;
using CCIA.Models.SeedsCreateViewModel;



namespace CCIA.Controllers
{
    public class SeedsController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public SeedsController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.Seeds.Where(s => s.ConditionerId == orgId).Select(s => s.CertYear.Value).MaxAsync();
            }
            var model = await SeedsIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            // TODO restrict to logged in user.
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            var model = await ClientSeedsViewModel.Create(_dbContext, orgId, id);
            return View(model);
        }

        // GET: Application/Create
        public ActionResult SelectOrigin()
        {
            // TODO: Check that logged in user has permission to create seeds.
            return View();
        }

        [HttpPost]
        public ActionResult SelectOrigin(string origin)
        {
            if(origin == "Ca")
            {
                return RedirectToAction("SelectApp");
            }
            if(origin == "OOS")
            {
                return RedirectToAction("NewOOSSeedLot");
            }
            return View();
        }

        public ActionResult NewOOSSeedLot()
        {
            return View();
        }

        public ActionResult SelectApp()
        {
            int[] years = Enumerable.Range(2007, CertYearFinder.CertYear - 2007 + 1).ToArray();
            return View(years);
        }

        [HttpGet]
        public async Task<ActionResult> CreateInState(int[] appId, int certYear, int certNum, int certRad)
        {
            if(appId == null || appId.Count() == 0)
            {
                ErrorMessage = "No apps selected";
                return RedirectToAction(nameof(SelectApp));

            }
            var model = await SeedsCreateViewModel.Create(_dbContext, appId, certYear, certNum, certRad);
           
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitInState(SeedsCreateViewModel model)
        {
            var seed = model.Seed;
            
            if(seed.CountyDrawn == 0 || seed.CountyDrawn == null){
                ErrorMessage = "Must Select county";
                return RedirectToAction("CreateInState", new {appId = seed.AppId, certYear = seed.CertYear, certNum = seed.SampleFormCertNumber, certRad = seed.SampleFormRad});
            }
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.SampleFormCertNumber == seed.SampleFormCertNumber.ToString() && s.SampleFormRad == seed.SampleFormRad))
            {
                ErrorMessage = "SID with same Lot, Cert Year, Cert Number, and Rad found. Duplicates are not allowed.";
                return RedirectToAction("CreateInState", new {appId = seed.AppId, certYear = seed.CertYear, certNum = seed.SampleFormCertNumber, certRad = seed.SampleFormRad});
            }            
            
            var app = await _dbContext.Applications.Where(a => a.Id == seed.AppId.First())
                .Include(a => a.Variety)
                .FirstAsync();
            var state = await _dbContext.County.Where(c => c.CountyId == seed.CountyDrawn).Select(c => c.StateProvinceId).FirstAsync();
            var country = await _dbContext.Countries.Where(c => c.Code == "USA").Select(c => c.Id).FirstAsync();
            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.SampleFormCertNumber = seed.SampleFormCertNumber.ToString();
            newSeed.SampleFormRad = seed.SampleFormRad;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = app.ApplicantId;
            // TODO Used logged in user org ID and logged in contact ID
            newSeed.ConditionerId = 168;
            newSeed.UserEntered = 1;
            newSeed.SampleFormVarietyId = app.SelectedVarietyId;
            newSeed.OfficialVarietyId = app.Variety.ParendId;
            newSeed.LotNumber = seed.LotNumber;
            newSeed.PoundsLot = seed.PoundsLot;
            newSeed.Class = seed.Class;
            newSeed.CountyDrawn = seed.CountyDrawn;
            newSeed.OriginState = state;
            newSeed.OriginCountry = country;
            if(seed.Type == "Original Run")
            {
                newSeed.OriginalRun = true;
            } else
            {
                newSeed.OriginalRun = false;
            }
            if(seed.Type == "Remill")
            {
                newSeed.Remill = true;
            } else
            {
                newSeed.Remill = false;
            }
            newSeed.Remarks = seed.Remarks;
            newSeed.SampleDrawnBy = seed.SampleDrawnBy + " - " + seed.SamplerName;
            newSeed.OECDLot = seed.OECDLot;
            newSeed.Confirmed = false;
            newSeed.Status = "Pending supporting material";
            newSeed.CertProgram = app.AppType;
            var seedapps = new List<SeedsApplications>();
            foreach(var sa in seed.AppId)
            {
                seedapps.Add(new SeedsApplications { AppId = sa});
            }
            newSeed.SeedsApplications = seedapps;
            
            if(ModelState.IsValid)
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                var labresults = new SxLabResults();
                labresults.SeedsId = newSeed.Id;
                await _dbContext.SxLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();

                Message = "Certified Seed Lot created";
            } else
            {
                ErrorMessage = "Error encountered saving seed lot";
                return RedirectToAction("CreateInState", new {appId = seed.AppId, certYear = seed.CertYear, certNum = seed.SampleFormCertNumber, certRad = seed.SampleFormRad});
            }
           
            return RedirectToAction("Details", new { id = newSeed.Id });
        }
        

        [HttpGet]
        public async Task<IActionResult> GetAppsFromCertNumber(int certYear, int? rad, int certNumber)
        {
            if(rad.HasValue){
                var certs = await _dbContext.CertRad.Where(c => c.CertYear == certYear && c.CertNum == certNumber && c.Rad == rad).FirstAsync();
                var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.CertNum == certs.CertNum)
                    .Include(a => a.GrowerOrganization)
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.OrgName, acres = a.AcresApplied })
                    .ToListAsync();
                if(model != null)
                {
                    return Json(model);
                }                
            } else
            {
               var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.CertNum == certNumber)
                    .Include(a => a.GrowerOrganization)
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.OrgName, acres = a.AcresApplied })
                    .ToListAsync();
               if(model != null)
                {
                    return Json(model);
                } 
            }
            return BadRequest();
        }

        [HttpGet]
        public async Task<IActionResult> GetApplicants(string search)
        {
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(search, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.OrgId == id)
                    .Select(o => new Organizations{ OrgId = o.OrgId, OrgName = o.OrgName})
                    .ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.OrgName.Contains(search.ToLower()))
                    .Select(o => new Organizations{ OrgId = o.OrgId, OrgName = o.OrgName})
                    .ToListAsync();
            }
            return Json(orgs);
        }
    }
}