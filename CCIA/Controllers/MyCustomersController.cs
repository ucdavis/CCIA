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
    public class MyCustomersController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public MyCustomersController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        
        // GET: Application
        public async Task<IActionResult> Index()
        {           
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();        
            var model = await MyCustomersIndexViewModel.Create(_dbContext, orgId, 0);             
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).ToArrayAsync();
            var model = await _dbContext.MyCustomers.Where(a => a.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            return View(model);
        }

        // GET: Application/Create
        public async Task<IActionResult> Create()
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
    }
}