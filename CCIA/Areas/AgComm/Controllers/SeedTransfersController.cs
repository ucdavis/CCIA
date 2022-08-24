using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;
using CCIA.Models.AgComm;
using CCIA.Helpers;
using System;
using System.Security.Claims;

namespace CCIA.Controllers.AgComm
{


    public class SeedTransfersController : AgCommController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        private readonly INotificationService _notification;

        public SeedTransfersController(CCIAContext dbContext, IFullCallService helper, INotificationService notification)
        {
            _dbContext = dbContext;
            _helper = helper;
            _notification = notification;
        }

        public async Task<IActionResult> Index()
        {            
            var model = await AgCommSeedTransferSearchViewModel.Create(_dbContext, null, _helper, 0);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AgCommSeedTransferSearchViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            var model = await AgCommSeedTransferSearchViewModel.Create(_dbContext, vm, _helper, countyId.Value);
            return View(model);
        }               

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.OriginatingCountyId != countyId.Value && model.PurchaserCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from or to your county.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Certificate(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.OriginatingCountyId != countyId.Value && model.PurchaserCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from or to your county.";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Admin/Views/SeedTransfers/Certificate.cshtml",model);
        }

         public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminBSeedTransfersEditViewModel.Create(_dbContext, id, _helper);
            if(model.stc == null)
            {
                ErrorMessage = "Seed Transfer Certificate not found!";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.stc.OriginatingCountyId != countyId.Value && model.stc.PurchaserCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from or to your county.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminBSeedTransfersEditViewModel vm)
        {
            var stcToUpdate = await _dbContext.SeedTransfers.Where(s => s.Id ==id).FirstOrDefaultAsync();
            var update = vm.stc;
            if(stcToUpdate == null || stcToUpdate.Id != update.Id)
            {
                ErrorMessage = "Seed Transfer Certificate not found";
                return RedirectToAction(nameof(Index));
            }

            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(stcToUpdate.OriginatingCountyId != countyId.Value && stcToUpdate.PurchaserCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from or to your county.";
                return RedirectToAction(nameof(Index));
            }

            var errors = SeedTransferValidator.CheckStandards(update);

            if (errors.HasWarnings)
            {
                ModelState.AddModelError("stc.StageFromFieldNumberOfAcres", errors.Error);
                var model = await AdminBSeedTransfersEditViewModel.Create(_dbContext, id, _helper);
                return View(model);
            }

            stcToUpdate.SeedsID = update.SeedsID;
            stcToUpdate.BlendId = update.BlendId;
            stcToUpdate.ApplicationId = update.ApplicationId;
            stcToUpdate.CertificateDate = update.CertificateDate;           
            stcToUpdate.Pounds = update.Pounds;
            stcToUpdate.PurchaserName = update.PurchaserName;
            stcToUpdate.PurchaserAddressLine1 = update.PurchaserAddressLine1;
            stcToUpdate.PurchaserAddressLine2 = update.PurchaserAddressLine2;
            stcToUpdate.PurchaserCity = update.PurchaserCity;
            stcToUpdate.PurchaserCountryId = update.PurchaserCountryId;
            stcToUpdate.PurchaserEmail = update.PurchaserEmail;
            stcToUpdate.PurchaserPhone = update.PurchaserPhone;
            stcToUpdate.PurchaserStateId = update.PurchaserStateId;
            stcToUpdate.PurchaserCountyId = update.PurchaserCountyId;
            stcToUpdate.PurchaserZip = update.PurchaserZip;
            stcToUpdate.AdminUpdatedDate = DateTime.Now;
            stcToUpdate.AdminUpdatedId = User.Claims.FirstOrDefault(c => c.Type == "contactId").Value;
            stcToUpdate.SeedstockLotNumbers = update.SeedstockLotNumbers;
            stcToUpdate.StageInDirt = update.StageInDirt;
            stcToUpdate.StageFromField = update.StageFromField;
            stcToUpdate.StageFromStorage = update.StageFromStorage;
            stcToUpdate.TypeRetail = update.TypeRetail;
            stcToUpdate.TypeTote = update.TypeTote;
            stcToUpdate.TypeBulk = update.TypeBulk;
            stcToUpdate.StageConditioned = update.StageConditioned;
            stcToUpdate.StageNotFinallyCertified = update.StageNotFinallyCertified;
            stcToUpdate.NumberOfTrucks  = update.NumberOfTrucks;
            stcToUpdate.StageCertifiedSeed = update.StageCertifiedSeed;
            stcToUpdate.StageTreatment = update.StageTreatment;
            stcToUpdate.StageTagging = update.StageTagging;
            stcToUpdate.StageBagging = update.StageBagging;
            stcToUpdate.StageBlending = update.StageBlending;
            stcToUpdate.StageStorage = update.StageStorage;
            stcToUpdate.StageOther = update.StageOther;
            stcToUpdate.StageOtherValue = update.StageOtherValue;
            stcToUpdate.StageFromFieldNumberOfAcres = update.StageFromFieldNumberOfAcres;
            stcToUpdate.AdminUpdated = false;
            stcToUpdate.OriginatingCountyId = update.OriginatingCountyId;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Seed Transfer Certificate Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminBSeedTransfersEditViewModel.Create(_dbContext, id, _helper);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = stcToUpdate.Id });  
        }

        [HttpPost]
        public async Task<IActionResult> Approve (int id)
        {
            var stcToUpdate = await _dbContext.SeedTransfers.Where(s => s.Id == id).FirstOrDefaultAsync();
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(stcToUpdate.OriginatingCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from your county. Only originating county Ag Comm can respond";
                return RedirectToAction(nameof(Index));
            }

            stcToUpdate.AgricultureCommissionerAccurate = true;
            stcToUpdate.AgricultureCommissionerInaccurate = false;
            stcToUpdate.AgricultureCommissionerApprove = true;
            stcToUpdate.AgricultureCommissionerDateRespond = DateTime.Now;
            stcToUpdate.AgricultureCommissionerContactRespondId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = stcToUpdate.Id });
        }

        [HttpPost]
        public async Task<IActionResult> InAccurate (int id)
        {
            var stcToUpdate = await _dbContext.SeedTransfers.Where(s => s.Id == id).FirstOrDefaultAsync();
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(stcToUpdate.OriginatingCountyId != countyId.Value)
            {
                ErrorMessage = "Seed Transfer Certificate not from your county. Only originating county Ag Comm can respond";
                return RedirectToAction(nameof(Index));
            }

            stcToUpdate.AgricultureCommissionerAccurate = false;
            stcToUpdate.AgricultureCommissionerInaccurate = true;
            stcToUpdate.AgricultureCommissionerApprove = false;
            stcToUpdate.AgricultureCommissionerDateRespond = DateTime.Now;
            stcToUpdate.AgricultureCommissionerContactRespondId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

            await _notification.SeedTransferResponded(stcToUpdate);

            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = stcToUpdate.Id });
        }
    }
}