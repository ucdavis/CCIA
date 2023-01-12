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
   
    public class TurfgrassController : AdminController
    {
        private readonly CCIAContext _dbContext;
        public TurfgrassController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Certificate(int id)
        {
            var model = await _dbContext.TurfgrassCertificates
                .Include(t => t.Application)
                .ThenInclude(a => a.ClassProduced)
                .Include(t => t.Application)
                .ThenInclude(a => a.Crop)
                .Include(t => t.Application)
                .ThenInclude(a => a.GrowerOrganization)
                .ThenInclude(g => g.Addresses.Where(a =>a.Active))
                .ThenInclude(g => g.Address)
                .ThenInclude(a => a.StateProvince)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Turfgrass Certificate not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        

        // public async Task<IActionResult> Search()
        // {
        //     var model = await AdminBulkSalesCertificateSearchViewModel.Create(_dbContext, null, _helper);
        //     return View(model);
        // }

        // [HttpPost]
        // public async Task<IActionResult> Search(AdminBulkSalesCertificateSearchViewModel vm)
        // {
        //     var model = await AdminBulkSalesCertificateSearchViewModel.Create(_dbContext, vm, _helper);
        //     return View(model);
        // }

        
       
    }
}
