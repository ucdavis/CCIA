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
using CCIA.Services;

namespace CCIA.Controllers.Client
{
    public class OECDController : ClientController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
		private readonly INotificationService _notification;

		public OECDController(CCIAContext dbContext, IFullCallService helper, INotificationService notification)
        {
            _dbContext = dbContext;
            _helper = helper;
			_notification = notification;
		}

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        { 
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.OECD.Where(o => o.ConditionerId == orgId).MaxAsync(x => (int?)x.DataEntryDate.Value.Year);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }
            
            var model = await OECDIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);            
            return View(model);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminOECDEditCreateViewModel.Create(_dbContext, _helper, id);
            if(model == null)
            {
                ErrorMessage = "OECD record not found";
                return RedirectToAction("Index");
            }
            if (model.oecd.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner for that OECD record! Access denied.";
                return RedirectToAction(nameof(Index));
            }
            if (model.oecd.DatePrinted.HasValue)
            {
                ErrorMessage = "OECD Certificate marked complete by CCIA staff. No edits allowed.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, AdminOECDEditCreateViewModel vm)
        {
            var oecdEdit = vm.oecd;
            var oecdToUpdate = await _dbContext.OECD.Where(o => o.Id == id).FirstOrDefaultAsync();
			if (oecdToUpdate == null)
			{
				ErrorMessage = "OECD record not found";
				return RedirectToAction("Index");
			}
			if (oecdToUpdate.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
			{
				ErrorMessage = "You are not the conditioner for that OECD record! Access denied.";
				return RedirectToAction(nameof(Index));
			}
			if (oecdToUpdate.DatePrinted.HasValue)
			{
				ErrorMessage = "OECD Certificate marked complete by CCIA staff. No edits allowed.";
				return RedirectToAction(nameof(Index));
			}

            oecdToUpdate.UpdateUser = User.Claims.FirstOrDefault(c => c.Type == "contactId").Value;
            oecdToUpdate.UpdateDate = DateTime.Now;
            oecdToUpdate.CloseDate = oecdEdit.CloseDate;
            oecdToUpdate.CountryId = oecdEdit.CountryId;
            oecdToUpdate.Pounds = oecdEdit.Pounds;
            oecdToUpdate.ClassId = oecdEdit.ClassId;
            oecdToUpdate.Comments = oecdEdit.Comments;
            oecdToUpdate.TagsRequested = oecdEdit.TagsRequested;

			if (ModelState.IsValid)
			{
                await _notification.OECDClientUpated(oecdToUpdate);
				await _dbContext.SaveChangesAsync();
				Message = "OECD Updated";
			}
			else
			{
				if (ModelState.ErrorCount == 1)
				{
					foreach (var modelStateKey in ViewData.ModelState.Keys)
					{
						var modelStateVal = ViewData.ModelState[modelStateKey];
						foreach (var error in modelStateVal.Errors)
						{
							var key = modelStateKey;
							if (key == "oecd.Variety.Name")
							{
                                await _notification.OECDClientUpated(oecdToUpdate);
								await _dbContext.SaveChangesAsync();								
								Message = "OECD Updated";
								return RedirectToAction(nameof(Edit), new { id = oecdToUpdate.Id });
							}
						}
					}
				}
				ErrorMessage = "Something went wrong.";
				var model = await AdminOECDEditCreateViewModel.Create(_dbContext, _helper, id);
				return View(model);
			}
			return RedirectToAction(nameof(Edit), new { id = oecdToUpdate.Id });
		}

        public async Task<IActionResult> Complete(int id, AdminOECDEditCreateViewModel vm)
        {			
			var oecdToUpdate = await _dbContext.OECD.Where(o => o.Id == vm.oecd.Id).FirstOrDefaultAsync();
			if (oecdToUpdate == null)
			{
				ErrorMessage = "OECD record not found";
				return RedirectToAction("Index");
			}
			if (oecdToUpdate.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
			{
				ErrorMessage = "You are not the conditioner for that OECD record! Access denied.";
				return RedirectToAction(nameof(Index));
			}
			if (oecdToUpdate.DatePrinted.HasValue)
			{
				ErrorMessage = "OECD Certificate marked complete by CCIA staff. No edits allowed.";
				return RedirectToAction(nameof(Index));
			}
            await _notification.OECDClientComplete(oecdToUpdate);
            await _dbContext.SaveChangesAsync();
            Message = "OECD submitted for final print";
            return RedirectToAction(nameof(Index));
		}


		public async Task<IActionResult> Certificate(int id)
        {
            var oecd = _helper.FullOECD();
            var model = await oecd.Where(o => o.Id == id).FirstOrDefaultAsync();
            if(model.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the conditioner for that certificate! Access denied.";
                return RedirectToAction(nameof(Index));
            }         
            if(model.DatePrinted.HasValue)
            {
                return View("~/Areas/Admin/Views/OECD/Certificate.cshtml",model);
            }   else {
                ErrorMessage = "OECD Certificate not marked complete by CCIA staff. Please check back later.";
                return RedirectToAction(nameof(Index));
            }
             
        }


      
    }
}