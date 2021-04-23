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



namespace CCIA.Controllers.Client
{
    public class SeedTransferController : ClientController
    {
        private readonly CCIAContext _dbContext;

        public SeedTransferController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {           
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.Id).SingleAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.SeedTransfers.Where(a => a.OriginatingOrganizationId == orgId).Select(a => a.CertificateDate.Year).MaxAsync();;
            }
            var model = await SeedTransferIndexViewModel.Create(_dbContext, orgId, certYear);          
            return View(model);
        }

        // GET: Application/Details/5
        public ActionResult Details(int id)
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

        // GET: Application/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}