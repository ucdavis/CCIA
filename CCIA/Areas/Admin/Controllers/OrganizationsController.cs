using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using CCIA.Services;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Controllers
{
    public class OrganizationsController : AdminController
    {

        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public OrganizationsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        // TODO: Add Roles - Seasonal Field get nothing in here. View (can edit phone, fax, website). EditOrg has full edit & create. CondStatus allows you to update conditioner status settings.

        public async Task<IActionResult> Index(string term = "")
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return View(null);
            }
            var model = await _helper.FullOrg().Where(o => EF.Functions.Like(o.Id.ToString(), "%" + term + "%") || EF.Functions.Like(o.Name, "%" + term + "%")).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullOrg().Where(o => o.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> EditStatus(int id)
        {
            var model = await _dbContext.CondStatus.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Status entry not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, CondStatus update)
        {
            var statusToUpdate = await _dbContext.CondStatus.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(statusToUpdate == null || update == null || statusToUpdate.Id != update.Id)
            {
                ErrorMessage = "Status not found or not able to update";
                return RedirectToAction(nameof(Index));
            }

            statusToUpdate.Status = update.Status;
            statusToUpdate.AllowPretag = update.AllowPretag;
            statusToUpdate.PrintSeries = update.PrintSeries;
            statusToUpdate.RequestCciaPrintSeries = update.RequestCciaPrintSeries;
            statusToUpdate.DatePretagApproved = update.DatePretagApproved;
            statusToUpdate.DateUpdated = DateTime.Now;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Status Updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(update); 
            }

            return RedirectToAction(nameof(Details), new { id = statusToUpdate.OrgId });  
        }

        public ActionResult NewStatus(int id)
        {
            var model = new CondStatus();
            model.OrgId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewStatus(int id, CondStatus newStatus)
        {
            var statusToCreate = new CondStatus();
            statusToCreate.Year = newStatus.Year;
            statusToCreate.Status = newStatus.Status;
            statusToCreate.AllowPretag = newStatus.AllowPretag;
            statusToCreate.PrintSeries = newStatus.PrintSeries;
            statusToCreate.RequestCciaPrintSeries = newStatus.RequestCciaPrintSeries;
            statusToCreate.DatePretagApproved = newStatus.DatePretagApproved;
            statusToCreate.DateUpdated = DateTime.Now;
            statusToCreate.OrgId = newStatus.OrgId;

             if(ModelState.IsValid){
                 _dbContext.Add(statusToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Status Created";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(newStatus); 
            }

            return RedirectToAction(nameof(Details), new { id = newStatus.OrgId }); 
        }

        public async Task<IActionResult> EditAddress(int id)
        {
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
            if(model.address == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

       
       
    }
}
