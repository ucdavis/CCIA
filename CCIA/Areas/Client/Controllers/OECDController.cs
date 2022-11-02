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

        public OECDController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
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

        public ActionResult Details(int id)
        {
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