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
    public class TagController : ClientController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;
        private readonly INotificationService _notificationService;

        public TagController(CCIAContext dbContext, IFullCallService helper, IFileIOService fileService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileService;
            _notificationService = notificationService;
        }
       

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {      
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.Tags.Where(o => o.TaggingOrg == orgId).MaxAsync(x => (int?)x.DateRequested.Value.Year);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }           
            var model = await TagIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);            
            return View(model);
        }

        public async Task<IActionResult> Details(int id)    
        {            
            var tag = _helper.FullTag();
            var model = await tag
                .Include(t => t.TagBagging)
                .Include(t => t.EmployeePrinted)
                .Include(t => t.Documents)
                .Where(t => t.Id == id).FirstOrDefaultAsync();                     

            if(model == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            
            if(model.TaggingOrg != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the company that requested that tag";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}