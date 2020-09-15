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
using System.Security.Claims;

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
                .Include(a => a.FieldInspection)
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



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AcceptApplication(IFormCollection form)
        {
            var id = int.Parse(form["application.Id"].ToString());
            var appToAccept = await _dbContext.Applications.Where(a => a.Id == id).FirstAsync();
            appToAccept.Status = "Field Inspection in Progress";
            appToAccept.Approved = true;
            appToAccept.DateApproved = DateTime.Now;            
            appToAccept.Approver = User.FindFirstValue(ClaimTypes.Name);
            appToAccept.NotifyNeeded = true;
            appToAccept.NotifyDate = DateTime.Now;
            appToAccept.UserEmpDateMod = DateTime.Now;
            appToAccept.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);

            await _dbContext.SaveChangesAsync();
            Message = "Application Accepted";

            await _dbContext.Database.ExecuteSqlCommandAsync("accept_application_updates @p0", id);

            return  RedirectToAction(nameof(Pending));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CancelApplication(IFormCollection form)
        {
            var id = int.Parse(form["application.Id"].ToString());
            var appToCancel = await _dbContext.Applications.Where(a => a.Id == id).FirstAsync();
            appToCancel.Cancelled = true;
            appToCancel.CancelledBy =  User.FindFirstValue(ClaimTypes.Name);
            appToCancel.Comments = appToCancel.Comments +  "<br/> Cancelled at 'process pending apps' prior to inspection";
            appToCancel.Status = "Application cancelled";

            await _dbContext.SaveChangesAsync();
            Message = "Application Cancelled";
            return  RedirectToAction(nameof(Pending));
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
		
            return  RedirectToAction(nameof(Renew));
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

         public ActionResult LookupFIR()
         {
             return View();
         }

        public async Task<IActionResult> FIR(int id)
        {
            var model = await AdminViewModel.CreateFIR(_dbContext, id);
            return View(model);
        }

        public async Task<IActionResult> EditFIR(int id)
        {
            var model = await AdminFIRViewModel.Create(_dbContext, id);            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFIR(int id, AdminFIRViewModel vm)
        {
            var fir = vm.fir;
            var firToUpdate = await _dbContext.FieldInspectionReport.Where(f => f.AppId == id).FirstAsync();
            var potApp = await _dbContext.Applications.Where(a => a.Id == firToUpdate.AppId).AnyAsync(a => a.AppType == "PO");
            if(!potApp)
            {
                firToUpdate.AcresApproved = fir.AcresApproved;
                firToUpdate.AcresRejected = fir.AcresRejected;
                firToUpdate.AcresGrowout = fir.AcresGrowout;
                firToUpdate.AcresInspectionOnly = fir.AcresInspectionOnly;
                firToUpdate.AcresRefund = fir.AcresRefund;
                firToUpdate.AcresCancelled = fir.AcresCancelled;
                firToUpdate.AcresNoCrop = fir.AcresNoCrop;
            } else {
                firToUpdate.PathDate = fir.PathDate;
                firToUpdate.PathNumSamples = fir.PathNumSamples;
                firToUpdate.PathNumPlants = fir.PathNumPlants;
                firToUpdate.PathLab = fir.PathLab;
                firToUpdate.PathSentVia = fir.PathSentVia;
                firToUpdate.PassClass = fir.PassClass;
                firToUpdate.PathCms = fir.PathCms;
                firToUpdate.PathPva = fir.PathPva;
                firToUpdate.PathPvs = fir.PathPvs;
                firToUpdate.PathErw = fir.PathErw;
                firToUpdate.PathPvm = fir.PathPvm;
                firToUpdate.PathPvx = fir.PathPvx;
                firToUpdate.PathPstvd = fir.PathPstvd;
                firToUpdate.PathPlrv = fir.PathPlrv;
                firToUpdate.PathPvy = fir.PathPvy;
                firToUpdate.PathComments = fir.PathComments;
            }
            firToUpdate.Comments = fir.Comments;
            if(!firToUpdate.Complete && fir.Complete)
            {
                firToUpdate.CompleteBy = User.FindFirstValue(ClaimTypes.Name);
                firToUpdate.DateComplete = DateTime.Now;
            }
            firToUpdate.Complete = fir.Complete;
            
            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "FIR Updated";
            } else {
                ViewBag.AcresApplied = await _dbContext.Applications.Where(a => a.Id == id).Select(a => a.AcresApplied).SingleAsync();
                var model = await _dbContext.FieldInspectionReport.Where(f => f.AppId == id).FirstAsync();
                return View(model); 
            }

            return RedirectToAction(nameof(FIR), new { id = firToUpdate.AppId });  
        }


        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdminViewModel vm)
        {
            var appToUpdate = await _dbContext.Applications.Where(a => a.Id == id).FirstAsync();
            var edit = vm.application;
            appToUpdate.UserEmpDateMod = DateTime.Now;
            appToUpdate.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);
            appToUpdate.AppType = edit.AppType;
            appToUpdate.CertYear = edit.CertYear;
            if(appToUpdate.Postmark.HasValue && edit.Postmark.HasValue)
            {
                if(appToUpdate.Postmark.Value.Date != edit.Postmark.Value.Date)
                {
                    appToUpdate.Postmark = edit.Postmark;
                }
            }
            appToUpdate.DatePlanted = edit.DatePlanted;
            appToUpdate.OverrideLateFee = edit.OverrideLateFee;
            appToUpdate.ApplicantId = edit.ApplicantId;
            appToUpdate.GrowerId = edit.GrowerId;
            appToUpdate.CropId = edit.CropId;
            appToUpdate.EnteredVariety = edit.EnteredVariety;
            appToUpdate.SelectedVarietyId = edit.SelectedVarietyId;
            appToUpdate.ClassProducedId = edit.ClassProducedId;
            appToUpdate.AcresApplied = edit.AcresApplied;
            appToUpdate.FieldName = edit.FieldName;
            if(edit.PackageComplete && !appToUpdate.PackageComplete){
                appToUpdate.CompleteDate = DateTime.Now;                
            }
            appToUpdate.PackageComplete = edit.PackageComplete;
            
            appToUpdate.FarmCounty = edit.FarmCounty;
            appToUpdate.Comments = edit.Comments;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Application Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminViewModel.CreateEdit(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = appToUpdate.Id });  

        }

        public async Task<IActionResult> EditHistory(int id)
        {
            var model = await AdminHistoryViewModel.Create(_dbContext, id);
            return View(model);
        }

        public async Task<IActionResult> EditPS(int id)
        {
            var model = await AdminPSViewModel.Create(_dbContext, id);
            return View(model);  
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditHistory(int id, AdminHistoryViewModel historyVm)
        {
            var history = historyVm.history;
            var historyToUpdate = await _dbContext.FieldHistory.Where(f => f.Id == id).FirstAsync();
            historyToUpdate.Year = history.Year;
            historyToUpdate.Crop = history.Crop;
            historyToUpdate.Variety = history.Variety;
            historyToUpdate.AppNumber = history.AppNumber;
            historyToUpdate.DateModified = DateTime.Now;
            historyToUpdate.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);
            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Field History Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminPSViewModel.Create(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Edit), new { id = historyToUpdate.AppId });              
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPS(int id, AdminPSViewModel psEditVM)
        {
            var ps = psEditVM.plantingStocks;
            var PSToUpdate = await _dbContext.PlantingStocks.Where(p => p.PsId == id).FirstAsync();
            PSToUpdate.PsCertNum = ps.PsCertNum;
            PSToUpdate.PsEnteredVariety = ps.PsEnteredVariety;
            if(ps.PoundsPlanted != null){
                PSToUpdate.PoundsPlanted = ps.PoundsPlanted;
            }
            if(ps.PlantsPerAcre != null) {
                PSToUpdate.PlantsPerAcre = ps.PlantsPerAcre;
            }
            PSToUpdate.PsClass = ps.PsClass;
            PSToUpdate.StateCountryTagIssued = ps.StateCountryTagIssued;
            PSToUpdate.StateCountryGrown = ps.StateCountryGrown;
            if(ps.SeedPurchasedFrom != null){
                PSToUpdate.SeedPurchasedFrom = ps.SeedPurchasedFrom;
            }
            if(await _dbContext.Applications.Where(a => a.Id == PSToUpdate.AppId).AnyAsync(a => a.AppType == "PO")){
                PSToUpdate.WinterTest = ps.WinterTest;
                PSToUpdate.PvxTest = ps.PvxTest;
            }
            if(ps.ThcPercent != null){
                PSToUpdate.ThcPercent = ps.ThcPercent;
            }
            PSToUpdate.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);
            PSToUpdate.DateModified = DateTime.Now;

            if(ModelState.IsValid) {
                await _dbContext.SaveChangesAsync();
                Message = "Planting Stock Edit Successful";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminPSViewModel.Create(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Edit), new { id = PSToUpdate.AppId });                       
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
            var classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appId).OrderBy(c => c.SortOrder).ToListAsync();
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