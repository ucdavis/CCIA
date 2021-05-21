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
            var stcToUpdate = await _dbContext.BulkSalesCertificates.Where(b => b.Id ==id).FirstOrDefaultAsync();
            var update = vm.stc;
            if(stcToUpdate == null || stcToUpdate.Id != update.Id)
            {
                ErrorMessage = "Seed Transfer Certificate not found";
                return RedirectToAction(nameof(Lookup));
            }

            stcToUpdate.SeedsID = update.SeedsID;
            stcToUpdate.BlendId = update.BlendId;
            // stcToUpdate.Date = update.Date;
            // stcToUpdate.ClassId = update.ClassId;
            // stcToUpdate.Pounds = update.Pounds;
            stcToUpdate.PurchaserName = update.PurchaserName;
            stcToUpdate.PurchaserAddressLine1 = update.PurchaserAddressLine1;
            stcToUpdate.PurchaserAddressLine2 = update.PurchaserAddressLine2;
            stcToUpdate.PurchaserCity = update.PurchaserCity;
            stcToUpdate.PurchaserCountryId = update.PurchaserCountryId;
            stcToUpdate.PurchaserEmail = update.PurchaserEmail;
            stcToUpdate.PurchaserPhone = update.PurchaserPhone;
            stcToUpdate.PurchaserStateId = update.PurchaserStateId;
            stcToUpdate.PurchaserZip = update.PurchaserZip;
            stcToUpdate.AdminUpdatedDate = DateTime.Now;
            stcToUpdate.AdminUpdatedId = User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Bulk Sales Certificate Updated";
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

        
       
    }
}
