using System;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
using CCIA.Services;
using Microsoft.Data.SqlClient;
using System.IO;
using System.Security.Claims;
using CCIA.Models.SeedsViewModels;

namespace CCIA.Controllers.Admin
{

    public class SeedsController : AdminController
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

         public async Task<IActionResult> ToggleFollowup(int id)
        {
            var seedToToggle = await _dbContext.Seeds.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(seedToToggle == null)
            {
                ErrorMessage = "Seed not found";
                return RedirectToAction(nameof(Index));
            }
            seedToToggle.FollowUp = !seedToToggle.FollowUp;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = id});
        }

        public async Task<IActionResult> Create()
        {
            var model = await AdminSeedsViewModel.CreateNew(_dbContext);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AdminSeedsViewModel vm)
        {
            var seedToCreate = new Seeds();
            var seedSubmitted = vm.seed;

            seedToCreate.CertProgram = seedSubmitted.CertProgram;
            seedToCreate.DateSampleReceived = seedSubmitted.DateSampleReceived;
            seedToCreate.CertYear = seedSubmitted.CertYear;
            seedToCreate.ConditionerId = seedSubmitted.ConditionerId;
            seedToCreate.ApplicantId = seedSubmitted.ApplicantId;
            seedToCreate.OfficialVarietyId = seedSubmitted.OfficialVarietyId;
            seedToCreate.Class = seedSubmitted.Class;
            seedToCreate.SampleDrawnBy = seedSubmitted.SampleDrawnBy;
            seedToCreate.OriginCountry = seedSubmitted.OriginCountry;
            seedToCreate.OriginState = seedSubmitted.OriginState;
            seedToCreate.SampleFormCertNumber = seedSubmitted.SampleFormCertNumber;
            seedToCreate.LotNumber = seedSubmitted.LotNumber;
            seedToCreate.PoundsLot = seedSubmitted.PoundsLot;
            seedToCreate.NotFinallyCertified = seedSubmitted.NotFinallyCertified;
            seedToCreate.CountyDrawn = seedSubmitted.CountyDrawn;
            seedToCreate.OriginalRun = seedSubmitted.OriginalRun;
            seedToCreate.Remill = seedSubmitted.Remill;
            seedToCreate.OECDLot = seedSubmitted.OECDLot;
            seedToCreate.Remarks = seedSubmitted.Remarks;
            seedToCreate.SampleFormVarietyId = seedSubmitted.OfficialVarietyId;
            seedToCreate.SampleFormDate = seedSubmitted.SampleFormDate;

            if(ModelState.IsValid){
                _dbContext.Add(seedToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Seed created";
            } else {
                ErrorMessage = "Something went wrong";
                var model = await AdminSeedsViewModel.CreateNew(_dbContext);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = seedToCreate.Id });

        }

        public async Task<IActionResult> GetSeedFile(int id)
        {
            var doc = await _dbContext.SeedDocuments.Where(d => d.Id == id).Include(d => d.DocumentType).FirstOrDefaultAsync();
            var certYear = await _dbContext.Seeds.Where(s => s.Id == doc.SeedsId).Select(s => s.CertYear).FirstOrDefaultAsync();
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadSeedFile(doc, certYear.Value), contentType, doc.Link);
        }        

        [HttpPost]
        public async Task<IActionResult> UploadSeedDocument(int id, string certName, int docType, IFormFile file)
        {
           var sid = await _dbContext.Seeds.Where(s => s.Id==id).FirstOrDefaultAsync();
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


        public async Task<IActionResult> Pending()
        {
            var model = await _helper.OverviewSeeds().Where(s => s.Status == "Pending Acceptance").ToListAsync();
            return View(model);
        }

        
        // TODO: Add ability to associate applications with a SID

        [HttpPost]
        public async Task<IActionResult> AcceptSeed(IFormCollection form)
        {            
            var id = int.Parse(form["seed.Id"].ToString());
            var seedToAccept = await _dbContext.Seeds.Where(s => s.Id == id).FirstAsync();
            seedToAccept.Confirmed = true;
            seedToAccept.ConfirmedAt = DateTime.Now;
            seedToAccept.Status = SeedsStatus.SIRReady.GetDisplayName();

            await _notificationService.SeedLotAccepted(seedToAccept);
            
            await _dbContext.SaveChangesAsync();
            Message = "Seed Accepted";
           
            var p0 = new SqlParameter("@seeds_id", id);
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC accept_seeds_post_action @seeds_id", p0);

           return  RedirectToAction(nameof(SIR), new {id = id}); 
        }

        public ActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> SIR(int id)
        {           
            var model = await ClientSeedsViewModel.CreateSIR(_dbContext, _helper, id);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Client/Views/Seeds/SIR.cshtml",model);  
        }

        public async Task<IActionResult> ChargeSIR(int id)
        {  
            var p0 = new SqlParameter("@seeds_id", id);
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC accept_seeds_post_action @seeds_id", p0);

            return  RedirectToAction(nameof(SIR), new {id = id});            
        }    

        
        public async Task<IActionResult> Details(int id)
        {  
            var model = await AdminSeedsViewModel.CreateDetails(_dbContext, id, _helper);
            return View(model);
        }

        public async Task<IActionResult> Previous(int id)
        {
            var previousId = await _dbContext.Seeds.Where(x => x.Id < id).OrderBy(x => x.Id).Select(x => x.Id).LastOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Next(int id)
        {
            var previousId = await _dbContext.Seeds.Where(x => x.Id > id).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Edit(int id)
        {  
            var model = await AdminSeedsViewModel.EditDetails(_dbContext, id, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminSeedsViewModel vm)
        {
            var seedEdit = vm.seed;
            var seedToUpdate = await _dbContext.Seeds.Where(s => s.Id == seedEdit.Id).FirstOrDefaultAsync();

            seedToUpdate.SampleFormDate = seedEdit.SampleFormDate;
            seedToUpdate.CertYear = seedEdit.CertYear;
            seedToUpdate.ConditionerId = seedEdit.ConditionerId;
            seedToUpdate.ApplicantId = seedEdit.ApplicantId;
            seedToUpdate.OfficialVarietyId = seedEdit.OfficialVarietyId;
            seedToUpdate.SampleFormCertNumber = seedEdit.SampleFormCertNumber;
            seedToUpdate.LotNumber = seedEdit.LotNumber;
            seedToUpdate.PoundsLot = seedEdit.PoundsLot;
            seedToUpdate.NotFinallyCertified = seedEdit.NotFinallyCertified;
            seedToUpdate.ChargeFullFees = seedEdit.ChargeFullFees;
            seedToUpdate.Class = seedEdit.Class;
            seedToUpdate.SampleDrawnBy = seedEdit.SampleDrawnBy;
            seedToUpdate.OriginCountry = seedEdit.OriginCountry;
            seedToUpdate.OriginState = seedEdit.OriginState;
            seedToUpdate.OriginalRun = seedEdit.OriginalRun;
            seedToUpdate.Remill = seedEdit.Remill;
            seedToUpdate.OECDLot = seedEdit.OECDLot;
            seedToUpdate.EmployeeModified = User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Seed updated";
            } else {
                ErrorMessage = "Something went wrong";
                var model = await AdminSeedsViewModel.EditDetails(_dbContext, seedEdit.Id, _helper);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = seedEdit.Id });

        }

        public ActionResult Seeds()
        {
            return View();
        }

        public ActionResult Certs()
        {           
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Certs(IFormCollection form)
        {
            var certs = _helper.FullCerts();
            var entry = form["id"];
            var how = form["searchHow"];
            switch (how)
            {
                case "Exact":
                    certs = certs.Where(c => c.CertNum == int.Parse(entry));
                    break;
                case "Begin" :
                    certs = certs.Where(c => c.CertNum.ToString().StartsWith(entry));
                    break;
                case "End" :
                    certs = certs.Where(c => c.CertNum.ToString().EndsWith(entry));
                    break;
                case "Contain" :
                    certs = certs.Where(c => c.CertNum.ToString().Contains(entry));
                    break;
                default:
                    certs = certs.Where(c => c.CertNum == -1);
                    break;
            }            
            
            var model = await certs.ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> WeightLog(int year)
        {
            if(year == 0)
            {
                year = CertYearFinder.CertYear;
            }

            var model = await WeightLogViewModel.Create(_dbContext, year);
            return View(model);

        }

        public async Task<IActionResult> Search(int id, AdminSeedSearchViewModel vm)
        {
            if(!vm.Search){
                var freshmodel = await AdminSeedSearchViewModel.Create(_dbContext, null);
                return View(freshmodel);  
            }
                var model = await AdminSeedSearchViewModel.Create(_dbContext, vm);                
                return View(model);            
        }  

        [HttpPost]
        public async Task<IActionResult> Sublot(int id)
        {
            var sid = await _dbContext.Seeds.Where(s => s.Id == id).FirstOrDefaultAsync();
            if(sid == null)
            {
                ErrorMessage = "SID not found!";
                return RedirectToAction(nameof(Index));
            }

            var newSid = new Seeds();
            newSid.CertProgram = sid.CertProgram;
            newSid.AppId = sid.AppId;
            newSid.SampleFormCertNumber = sid.SampleFormCertNumber;
            newSid.SampleFormDate = sid.SampleFormDate;
            newSid.Sublot = true;
            newSid.SampleFormRad = sid.SampleFormRad;
            newSid.CertYear = sid.CertYear;
            newSid.ApplicantId = sid.ApplicantId;
            newSid.ConditionerId = sid.ConditionerId;
            newSid.SampleFormVarietyId = sid.SampleFormVarietyId;
            newSid.OfficialVarietyId = sid.OfficialVarietyId;
            newSid.LotNumber = sid.LotNumber + "-1";
            newSid.PoundsLot = 0;
            newSid.Class = sid.Class;
            newSid.ClassAccession = sid.ClassAccession;
            newSid.Status = sid.Status;
            newSid.CountyDrawn = sid.CountyDrawn;
            newSid.OriginState = sid.OriginState;
            newSid.OriginCountry = sid.OriginCountry;
            newSid.Bulk = sid.Bulk;
            newSid.OriginalRun = sid.OriginalRun;
            newSid.Remill = sid.Remill;
            newSid.Treated = sid.Treated;
            newSid.OECDTestRequired = sid.OECDTestRequired;
            newSid.Resampled = sid.Resampled;
            newSid.Remarks = sid.Remarks + $"; sublot of SID{sid.Id}";
            newSid.SampleDrawnBy = sid.SampleDrawnBy;
            newSid.SamplerID = sid.SamplerID;
            newSid.SampleId = sid.SampleId;
            newSid.OECDLot = sid.OECDLot;
            newSid.Rush = sid.Rush;
            newSid.InDirt = sid.InDirt;
            newSid.BlendNumber = sid.BlendNumber;
            newSid.DateSampleReceived = sid.DateSampleReceived;
            newSid.CropFee = 0;
            newSid.CertFee = 0;
            newSid.ResearchFee = 0;
            newSid.MinimumFee = 0;
            newSid.LotCertOk = sid.LotCertOk;
            newSid.UserEntered = sid.UserEntered;
            newSid.Submitted = sid.Submitted;
            newSid.Confirmed = sid.Confirmed;
            newSid.ConfirmedAt = sid.ConfirmedAt;
            newSid.Docs = false;
            newSid.NotFinallyCertified = sid.NotFinallyCertified;
            newSid.ChargeFullFees = false;

            if(ModelState.IsValid){
                _dbContext.Add(newSid);
                await _dbContext.SaveChangesAsync();
                Message = "Sublot created. Please update the Lot# & Pounds";
            } else {
                ErrorMessage = "Something went wrong";
                return RedirectToAction(nameof(Details), new { id = id});
            }

            return RedirectToAction(nameof(Details), new { id = newSid.Id });
        }

    }
}