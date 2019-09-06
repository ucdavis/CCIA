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



namespace CCIA.Controllers
{
    public class TurfgrassCertificateController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public TurfgrassCertificateController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
           
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            
            if (certYear == 0)
            {
                certYear = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.ApplicantId == orgId).Select(a => a.CertYear).MaxAsync();
            }

            var model = await TurgrassCertificateIndexViewModel.Create(_dbContext, orgId, certYear);            
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await TurfgrassCertificatesViewModel.Edit(_dbContext, id);
           
            return View(model);
        }

        // GET: Application/Create
        public async Task<IActionResult> Create(int id)
        {
            var model = await TurfgrassCertificatesViewModel.Create(_dbContext, id);
           
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        public async Task<IActionResult> Create(int id, TurfgrassCertificatesViewModel model)
        {

            if (model.TurfgrassCertificates.Sprigs == null)
                model.TurfgrassCertificates.Sprigs = 0;

            if (model.TurfgrassCertificates.Sod == null)
                model.TurfgrassCertificates.Sod = 0;

            if (model.TurfgrassCertificates.Sprigs == 0 && model.TurfgrassCertificates.Sod == 0) {
                ErrorMessage = "Certificate Sprigs or Sods needs a value.";
                return RedirectToAction(nameof(Create), new { id });
            }

            Applications application = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.Id == id)
                    .Include(a => a.TurfgrassCertificates)
                    .FirstOrDefaultAsync();

            if (application != null) {

                TurfgrassCertificates turfgrassCertificates = new TurfgrassCertificates();
                turfgrassCertificates.Id = model.TurfgrassCertificates.Id;
                turfgrassCertificates.AppId = model.TurfgrassCertificates.AppId;
                turfgrassCertificates.Sprigs = model.TurfgrassCertificates.Sprigs;
                turfgrassCertificates.Sod = model.TurfgrassCertificates.Sod;
                turfgrassCertificates.BillingInvoice = model.TurfgrassCertificates.BillingInvoice;
                turfgrassCertificates.HarvestDate = model.TurfgrassCertificates.HarvestDate;
                turfgrassCertificates.HarvestNumber = model.TurfgrassCertificates.HarvestNumber;

                application.TurfgrassCertificates.Add(turfgrassCertificates);

                // check ModelState before saving the changes
                if(ModelState.IsValid) {
                    await _dbContext.SaveChangesAsync();
                    Message = "Certificate Created Successfully";
                } else {
                    ErrorMessage = "Something went wrong.";
                    return RedirectToAction(nameof(Create), new { id });
                }

                return RedirectToAction(nameof(Details), new { id });
            } else {
                
                ErrorMessage = "Application not found.";

                var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
                var certYear = await _dbContext.Applications.Where(a => a.AppType == "TG" && a.ApplicantId == orgId).Select(a => a.CertYear).MaxAsync();
            
                return RedirectToAction(nameof(Index), new { certYear });
            }
        }

        public async Task<IActionResult> Certificate(int id, int certId) {
            var model = await TurfgrassCertificatesViewModel.Certificate(_dbContext, id, certId);

            return View(model);
        }
    }
}