using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Controllers.Admin
{
   
    public class SeedTransfersController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public SeedTransfersController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lookup()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Lookup));
            }
            return View(model);
        }

        public async Task<IActionResult> Previous(int id)
        {
            var previousId = await _dbContext.SeedTransfers.Where(x => x.Id < id).OrderBy(x => x.Id).Select(x => x.Id).LastOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Next(int id)
        {
            var previousId = await _dbContext.SeedTransfers.Where(x => x.Id > id).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminBSeedTransfersEditViewModel.Create(_dbContext, id, _helper);
            if(model.stc == null)
            {
                ErrorMessage = "Seed Transfer Certificate not found!";
                return RedirectToAction(nameof(Lookup));
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
                return RedirectToAction(nameof(Lookup));
            }
            var newOrg = await _dbContext.Organizations.Where(o => o.Id == update.OriginatingOrganizationId).FirstOrDefaultAsync();
            if(newOrg == null)
            {
                ErrorMessage = "Originating Org not found; Org must exist!";
                var orgErrorModel = await AdminBSeedTransfersEditViewModel.Create(_dbContext, id, _helper);
                return View(orgErrorModel); 
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
            stcToUpdate.OriginatingOrganizationId = update.OriginatingOrganizationId;
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
            stcToUpdate.AdminUpdatedId = User.FindFirstValue(ClaimTypes.Name);
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
            stcToUpdate.AdminUpdated = true;
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

        public async Task<IActionResult> Search()
        {
            var model = await AdminSeedTransferSearchViewModel.Create(_dbContext, null, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(AdminSeedTransferSearchViewModel vm)
        {
            var model = await AdminSeedTransferSearchViewModel.Create(_dbContext, vm, _helper);
            return View(model);
        }

        public async Task<IActionResult> Certificate(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Lookup));
            }
            return View(model);
        }

        
       
    }
}
