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
using CCIA.Models.ViewModels;
using CCIA.Services;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace CCIA.Controllers.Client
{
    
    [Authorize(Roles = "AllowSeeds")]
    public class SeedsController : ClientController
    {
        private readonly CCIAContext _dbContext;
         private readonly IFullCallService _helper;
        private readonly INotificationService _notificationService;

         private readonly IFileIOService _fileService;

        public SeedsController(CCIAContext dbContext, IFullCallService helper, INotificationService notificationService, IFileIOService fileIOService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _notificationService = notificationService;
            _fileService = fileIOService;
        }
        
        public async Task<IActionResult> Index(int certYear)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if (certYear == 0)
            {
                if(await _dbContext.Seeds.Where(s=> s.ConditionerId == orgId).AnyAsync())
                {
                    certYear = await _dbContext.Seeds.Where(s => s.ConditionerId == orgId).Select(s => s.CertYear.Value).MaxAsync();
                } else
                {
                    certYear = CertYearFinder.CertYear;
                }               
            }
            var model = await SeedsIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await ClientSeedsViewModel.Create(_dbContext, orgId, id);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            if(model.seed.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }      

        public async Task<IActionResult> SIR(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await ClientSeedsViewModel.CreateSIR(_dbContext, _helper, id);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            if(model.seed.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }    

        [HttpPost]
        public async Task<IActionResult> Submit(int id)
        {
            var seedToSubmit = await _dbContext.Seeds.Where(s => s.Id == id).FirstOrDefaultAsync();
            if(seedToSubmit == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            if(seedToSubmit.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }

            seedToSubmit.Status = SeedsStatus.PendingAcceptance.GetDisplayName();
            seedToSubmit.DateSampleReceived = DateTime.Now;
            seedToSubmit.Submitted = true;

            await _notificationService.SeedLotSubmitted(seedToSubmit);

            await _dbContext.SaveChangesAsync();
            Message = "Seed lot submitted for review";

            return RedirectToAction("Details", new { id = seedToSubmit.Id });
        } 
       
        public async Task<IActionResult> SelectOrigin()
        {     
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }       
            var permissionOk = await CheckConditionerPermission();
            if(!permissionOk)
            {
                ErrorMessage = "You do not have current permission to create new seed lots. Please contact CCIA to correct.";
                return RedirectToAction(nameof(Index));
            }
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
            var permissionOk = await CheckConditionerPermission();
            if(!permissionOk)
            {
                ErrorMessage = "You do not have current permission to create new seed lots. Please contact CCIA to correct.";
                return RedirectToAction(nameof(Index));
            }
            var model = await SeedsCreateOOSViewModel.Create(_dbContext);
            return View(model);
        }

        private async Task<bool> CheckConditionerPermission()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var approverStatus = new List<string> {"AP", "AC"};
            return await _dbContext.CondStatus.Where(s => s.OrgId == orgId && s.Year == CertYearFinder.ConditionerYear && approverStatus.Contains(s.Status)).AnyAsync();            
        }

        public ActionResult SelectApp()
        {
             int[] years = CertYearFinder.certYearListReverse.ToArray();
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
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }
            var model = await SeedsCreateQAViewModel.Create(_dbContext);
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> CreateQALot(SeedsCreateQAViewModel model)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
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
            newSeed.ConditionerId = orgId;
            newSeed.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
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
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var seed = model.Seed;
            
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
            newSeed.ConditionerId = orgId;
            newSeed.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
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
           
            
            
            if(ModelState.IsValid)
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                 var seedapps = new List<SeedsApplications>();
                foreach(var sa in seed.AppId)
                {
                    seedapps.Add(new SeedsApplications { AppId = sa});
                }

                newSeed.SeedsApplications = seedapps;
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
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var seed = model.Seed;  
            seed.LotNumber = seed.LotNumber.Trim();
            seed.SampleFormCertNumber = seed.SampleFormCertNumber.Trim();   
           
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
            
            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.SampleFormCertNumber = seed.SampleFormCertNumber;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = seed.ApplicantId;
            newSeed.SampleFormVarietyId = seed.SampleFormVarietyId;
            newSeed.OfficialVarietyId = seed.SampleFormVarietyId.HasValue ? seed.SampleFormVarietyId.Value : 0;
            newSeed.ConditionerId = orgId;
            newSeed.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);;           
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
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.Name, acres = a.AcresApplied })
                    .ToListAsync();
                if(model != null)
                {
                    return Json(model);
                }                
            } else
            {
               var model = await _dbContext.Applications.Where(a => a.CertYear == certYear && a.CertNum == certNumber)
                    .Include(a => a.GrowerOrganization)
                    .Select(a => new { appId = a.Id, grower = a.GrowerOrganization.Name, acres = a.AcresApplied })
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
            search = search.Trim();
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(search, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.Id == id)
                    .Select(o => new Organizations{ Id = o.Id, Name = o.Name})
                    .ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.Name.Contains(search.ToLower()))
                    .Select(o => new Organizations{ Id = o.Id, Name = o.Name})
                    .ToListAsync();
            }
            return Json(orgs);
        }

        public async Task<IActionResult> Edit(int id)
        {  
            var model = await AdminSeedsViewModel.ClientEditDetails(_dbContext, id, _helper);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            if(model.seed.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminSeedsViewModel vm)
        {
            var seedEdit = vm.seed;
            var seedToUpdate = await _dbContext.Seeds.Where(s => s.Id == seedEdit.Id).FirstOrDefaultAsync();  
            if(seedToUpdate == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }

            if(seedToUpdate.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }        
           
            seedToUpdate.LotNumber = seedEdit.LotNumber;
            seedToUpdate.PoundsLot = seedEdit.PoundsLot;            
            seedToUpdate.Class = seedEdit.Class;
            seedToUpdate.Remarks = seedEdit.Remarks;        

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Seed updated";
            } else {
                ErrorMessage = "Something went wrong";
                var model = await AdminSeedsViewModel.ClientEditDetails(_dbContext, seedEdit.Id, _helper);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = seedEdit.Id });

        }

         public async Task<IActionResult> GetSeedFile(int id)
        {
            var doc = await _dbContext.SeedDocuments.Where(d => d.Id == id).Include(d => d.DocumentType).FirstOrDefaultAsync();
            var seed = await _dbContext.Seeds.Where(s => s.Id == doc.SeedsId).FirstOrDefaultAsync();
            if(seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            if(seed.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadSeedFile(doc, seed.CertYear.Value), contentType, doc.Link);
        }        

        [HttpPost]
        public async Task<IActionResult> UploadSeedDocument(int id, string certName, int docType, IFormFile file)
        {
            certName = certName.Trim();
           var sid = await _dbContext.Seeds.Where(s => s.Id==id).FirstOrDefaultAsync();
           if(sid.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner of that seed lot";
                return RedirectToAction(nameof(Index));
            }
           var documentType = await _dbContext.SeedsDocumentTypes.Where(t => t.Id == docType).FirstOrDefaultAsync();
           if(sid == null)
           {
               ErrorMessage = "SID not found";
               return RedirectToAction(nameof(Index));
           }
           if(string.IsNullOrWhiteSpace(certName))
           {                
            ErrorMessage = "Must provide a certificate name for this file!";
            return RedirectToAction(nameof(Details), new { id = id });
           }
           var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

           if(_fileService.CheckDeniedExtension(ext))
           {
               ErrorMessage = "File extension not allowed!";
               return RedirectToAction(nameof(Details), new { id = id });
           }      
           
           if(file.Length >0)
           {
               await _fileService.SaveSeedDocument(sid, documentType.Folder, file); 
               var seedDoc = new SeedDocuments();
               seedDoc.SeedsId = sid.Id;
               seedDoc.DocType = docType;
               seedDoc.Name = certName;
               seedDoc.Link = file.FileName;                             
                _dbContext.Add(seedDoc);
                await _dbContext.SaveChangesAsync();
           }
           Message = "File uploaded";
           return RedirectToAction(nameof(Details), new { id = id }); 
        }

    }
}