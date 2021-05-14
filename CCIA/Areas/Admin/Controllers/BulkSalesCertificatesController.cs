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
   
    public class BulkSalesCertificatesController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public BulkSalesCertificatesController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lookup()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullBulkSalesCertificates().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Bulks Sales Certificate not found!";
                return RedirectToAction(nameof(Lookup));
            }
            return View(model);
        }

        
       
    }
}
