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
    public class SeedTransferController : ClientController
    {

        // TODO Get shipping location from address w/ Facility
        private readonly CCIAContext _dbContext;

        private readonly IFullCallService _helper;

        public SeedTransferController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {           
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value); 
            if (certYear == 0)
            {
                var maxCertYear =  await _dbContext.SeedTransfers.Where(a => a.OriginatingOrganizationId == orgId).MaxAsync(a => (int?)a.CertificateDate.Year);
                if(maxCertYear.HasValue)
                {
                    certYear = maxCertYear.Value;
                } else {
                    certYear = CertYearFinder.CertYear;
                }               
            }
            var model = await SeedTransferIndexViewModel.Create(_dbContext, orgId, certYear);          
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {           
             var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public ActionResult Initiate()
        {
            return View();
        }

        // GET: Application/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public async Task<IActionResult> Certificate(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Admin/Views/SeedTransfers/Certificate.cshtml",model);            
        }
    }
}