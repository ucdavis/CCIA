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
   
    public class BulkSalesCertificatesController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public BulkSalesCertificatesController(CCIAContext dbContext, IFullCallService helper)
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
            var model = await _helper.FullBulkSalesCertificates().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Bulk Sales Certificate not found!";
                return RedirectToAction(nameof(Lookup));
            }
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminBulkSalesCertificatesEditViewModel.Create(_dbContext, id, _helper);
            if(model.bsc == null)
            {
                ErrorMessage = "Bulk Sales Certificate not found!";
                return RedirectToAction(nameof(Lookup));
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminBulkSalesCertificatesEditViewModel vm)
        {
            var bscToUpdate = await _dbContext.BulkSalesCertificates.Where(b => b.Id ==id).FirstOrDefaultAsync();
            var update = vm.bsc;
            if(bscToUpdate == null || bscToUpdate.Id != update.Id)
            {
                ErrorMessage = "Bulk Sales Certificate not found";
                return RedirectToAction(nameof(Lookup));
            }

            bscToUpdate.SeedsID = update.SeedsID;
            bscToUpdate.BlendId = update.BlendId;
            bscToUpdate.Date = update.Date;
            bscToUpdate.ClassId = update.ClassId;
            bscToUpdate.Pounds = update.Pounds;
            bscToUpdate.PurchaserName = update.PurchaserName;
            bscToUpdate.PurchaserAddressLine1 = update.PurchaserAddressLine1;
            bscToUpdate.PurchaserAddressLine2 = update.PurchaserAddressLine2;
            bscToUpdate.PurchaserCity = update.PurchaserCity;
            bscToUpdate.PurchaserCountryId = update.PurchaserCountryId;
            bscToUpdate.PurchaserEmail = update.PurchaserEmail;
            bscToUpdate.PurchaserPhone = update.PurchaserPhone;
            bscToUpdate.PurchaserStateId = update.PurchaserStateId;
            bscToUpdate.PurchaserZip = update.PurchaserZip;
            bscToUpdate.AdminUpdatedDate = DateTime.Now;
            bscToUpdate.AdminUpdatedId = User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Bulk Sales Certificate Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminBulkSalesCertificatesEditViewModel.Create(_dbContext, id, _helper);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = bscToUpdate.Id });  
        }

        
       
    }
}
