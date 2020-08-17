using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.SeedsViewModels;
using CCIA.Models.IndexViewModels;
using CCIA.Models.SeedsCreateViewModel;
using CCIA.Models.SeedsCreateOOSViewModel;
using CCIA.Models.SeedsCreateQAViewModel;



namespace CCIA.Controllers.Client
{
    
    public class SeedsController : ClientController
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

        public async Task<ActionResult> NewOOSSeedLot()
        {
            var model = await SeedsCreateOOSViewModel.Create(_dbContext);
            return View(model);
        }

        public ActionResult SelectApp()
        {
            int[] years = Enumerable.Range(2007, CertYearFinder.CertYear - 2006).ToArray();
            return View(years);
        }

        [HttpPost]
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

        public async Task<ActionResult> CreateQALot()
        {
            var model = await SeedsCreateQAViewModel.Create(_dbContext);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQALot(SeedsCreateQAViewModel model)
        {
            if(model.Seed.LotNumber == null){
                var returnModel = await SeedsCreateQAViewModel.Return(_dbContext, model.Seed);
                if(returnModel.Application == null)
                {
                    ErrorMessage = "That application was not found. Please check the values entered.";
                }
                ModelState.Clear();
                return View(returnModel);
            }

            var seed = model.Seed;
            #region Begin error checking
            bool error = false;
            
            if(seed.CountyDrawn == 0 || seed.CountyDrawn == null){
                ErrorMessage = "Must Select county";
                error = true;         
            }
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.AppId == seed.AppId))
            {
                ErrorMessage = "SID with same Lot, Cert Year, and AppID found. Duplicates are not allowed.";
                error = true;                
            } 
            
            var appType = await _dbContext.AbbrevAppType.Where(a => a.AppTypeId == seed.AppType).Select(a => a.Abbreviation).FirstOrDefaultAsync(); 
            var app = await _dbContext.Applications.Where(a => a.Id == seed.AppId && a.AppType == appType && a.CertYear == seed.CertYear)
                .Include(a => a.Variety)
                .FirstOrDefaultAsync();
            

            if(app == null)
            {
                ErrorMessage = "Application not found. Please check values.";
                error = true;
            }

            if(error)
            {
                var returnModel = await SeedsCreateQAViewModel.Return(_dbContext, seed);
                return View(returnModel);
            }

            #endregion  
            var state = await _dbContext.County.Where(c => c.CountyId == seed.CountyDrawn).Select(c => c.StateProvinceId).FirstAsync();
            var country = await _dbContext.Countries.Where(c => c.Code == "USA").Select(c => c.Id).FirstAsync();       

            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.AppId = seed.AppId;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = app.ApplicantId;
            // TODO Used logged in user org ID and logged in contact ID
            newSeed.ConditionerId = 168;
            newSeed.UserEntered = 1;
            newSeed.SampleFormVarietyId = app.SelectedVarietyId;
            if(app.Variety != null)
            {
                newSeed.OfficialVarietyId = app.Variety.ParentId;
            }     
            newSeed.CertProgram = app.AppType;      
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
            newSeed.SamplerID = seed.SamplerId;
            newSeed.OECDLot = false;
            newSeed.Confirmed = false;
            newSeed.Status = "Pending supporting material";
            newSeed.SampleFormCertNumber = app.QACertNumber;
            var seedapps = new List<SeedsApplications>();
           
            newSeed.SeedsApplications = seedapps;
            
            if(ModelState.IsValid)
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                var labresults = new SampleLabResults();
                labresults.SeedsId = newSeed.Id;
                await _dbContext.SampleLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();

                Message = "Certified Seed Lot created";
            } else
            {
                ErrorMessage = "Error encountered saving seed lot";                
                var returnModel = await SeedsCreateQAViewModel.Return(_dbContext, seed);
                return View(returnModel);
            }
           
            return RedirectToAction("Details", new { id = newSeed.Id });

        }

        [HttpPost]
        public async Task<ActionResult> SubmitInState(SeedsCreateViewModel model)
        {
            var seed = model.Seed;
            #region Begin error checking
            bool error = false;
            
            if(seed.CountyDrawn == 0 || seed.CountyDrawn == null){
                ErrorMessage = "Must Select county";
                error = true;         
            }
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.SampleFormCertNumber == seed.SampleFormCertNumber.ToString() && s.SampleFormRad == seed.SampleFormRad))
            {
                ErrorMessage = "SID with same Lot, Cert Year, Cert Number, and Rad found. Duplicates are not allowed.";
                error = true;                
            }   

            if(error)
            {
                var returnModel = await SeedsCreateViewModel.Return(_dbContext, seed);
                return View("CreateInState", returnModel);
            }

            #endregion         
            
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
            newSeed.OfficialVarietyId = app.Variety.ParentId;
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
            newSeed.SamplerID = seed.SamplerId;
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

                var labresults = new SampleLabResults();
                labresults.SeedsId = newSeed.Id;
                await _dbContext.SampleLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();

                Message = "Certified Seed Lot created";
            } else
            {
                ErrorMessage = "Error encountered saving seed lot";
                return RedirectToAction("CreateInState", new {appId = seed.AppId, certYear = seed.CertYear, certNum = seed.SampleFormCertNumber, certRad = seed.SampleFormRad});
            }
           
            return RedirectToAction("Details", new { id = newSeed.Id });
        }

        [HttpPost]
        public async Task<IActionResult> NewOOSSeedLot(SeedsCreateOOSViewModel model)
        {
            var seed = model.Seed;  
            seed.LotNumber = seed.LotNumber.Trim();
            seed.SampleFormCertNumber = seed.SampleFormCertNumber.Trim();   

            #region Begin error checking
            bool error = false;
            
             if(seed.CountyDrawn == 0 || seed.CountyDrawn == null){
                ErrorMessage = "Must Select county";
                error = true;
            }
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.SampleFormCertNumber == seed.SampleFormCertNumber))
            {
                ErrorMessage = "SID with same Lot, Cert Year, and Cert Number found. Duplicates are not allowed.";
                error = true;              
            }        
            if(error)
            {
                var returnModel = await SeedsCreateOOSViewModel.Return(_dbContext, seed);
                return View(returnModel); 
            }

            #endregion         
            
               
            
            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.SampleFormCertNumber = seed.SampleFormCertNumber;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = seed.ApplicantId;
            newSeed.SampleFormVarietyId = seed.SampleFormVarietyId;
            newSeed.OfficialVarietyId = seed.SampleFormVarietyId.HasValue ? seed.SampleFormVarietyId.Value : 0;
            
            // TODO Used logged in user org ID and logged in contact ID
            newSeed.ConditionerId = 168;
            newSeed.UserEntered = 1;           
            newSeed.LotNumber = seed.LotNumber;
            newSeed.PoundsLot = seed.PoundsLot;
            newSeed.Class = seed.Class;
            newSeed.CountyDrawn = seed.CountyDrawn;
            newSeed.OriginState = seed.OriginState;
            newSeed.OriginCountry = seed.OriginCountry;
            newSeed.OriginalRun = seed.Type == "Original Run" ? true : false;
            newSeed.Remill = seed.Type == "Remill" ? true : false;            
            newSeed.Remarks = seed.Remarks;
            newSeed.SampleDrawnBy = seed.SampleDrawnBy + " - " + seed.SamplerName;
            newSeed.SamplerID = seed.SamplerId;
            newSeed.OECDLot = seed.OECDLot;
            newSeed.Confirmed = false;
            newSeed.CertProgram = "SD";
            newSeed.Status = "Pending supporting material";
            //newSeed.CertProgram = app.AppType;
            
            
            if(ModelState.IsValid)
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                var labresults = new SampleLabResults();
                labresults.SeedsId = newSeed.Id;
                await _dbContext.SampleLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();

                Message = "Certified Seed Lot created";
            } else
            {
                ErrorMessage = "Error encountered saving seed lot";
                var returnModel = await SeedsCreateOOSViewModel.Return(_dbContext, seed);
                return View(returnModel);                
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