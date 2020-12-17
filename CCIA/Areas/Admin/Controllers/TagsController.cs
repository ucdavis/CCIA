using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;

namespace CCIA.Controllers.Admin
{

    public class TagsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public TagsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public ActionResult Index ()
        {
            return View();
        }

        public async Task<IActionResult> Process(string stage = "Requested")
        {
            var tag = _helper.FullTag();
            var model = await tag.Where(t => t.Stage == stage).ToListAsync();            
            return View(model);
        }   

        public async Task<IActionResult> Details(int id)    
        {
            var tag = _helper.FullTag();
            var model = await tag
                .Include(t => t.TagBagging)
                .Include(t => t.EmployeePrinted)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> UpdateAdminComments(int Id, string AdminComments)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == Id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.AdminComments = AdminComments;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = Id});
        }

        public async Task<IActionResult> UpdateTrackingNumber(int Id, string TrackingNumber)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == Id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.TrackingNumber = TrackingNumber;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = Id});
        }
    }
}