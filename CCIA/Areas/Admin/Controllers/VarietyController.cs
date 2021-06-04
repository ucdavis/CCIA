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
   
    public class VarietyController : AdminController
    {
        private readonly CCIAContext _dbContext;

        private readonly IFullCallService _helper;

        public VarietyController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await AdminVarietyDetailsViewModel.Create(_dbContext,_helper, id);
            
            if(model.variety == null)
            {
                ErrorMessage = "Variety not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        
        
       
    }
}
