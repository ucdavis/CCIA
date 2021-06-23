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

         public async Task<IActionResult> Edit(int id)
        {
            var model = await AdminVarietyDetailsViewModel.Edit(_dbContext,_helper, id);
            
            if(model.variety == null)
            {
                ErrorMessage = "Variety not found";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }  

        public async Task<IActionResult> Search()
        {
            var model = await AdminVarietySearchViewModel.Create(_dbContext, _helper, null);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(AdminVarietySearchViewModel vm)
        {
            var model = await AdminVarietySearchViewModel.Create(_dbContext, _helper, vm);
            return View(model);
        }

        public async Task<IActionResult> RemoveCountry(int variety, int country)    
        {
            var varCountryToRemove = await _dbContext.VarCountires.Where(c => c.VarId == variety && c.CountryId == country).FirstOrDefaultAsync();
            if(varCountryToRemove==null)
            {
                ErrorMessage = "That Variety and Country combo not found";                
                return RedirectToAction(nameof(Details), new {id = variety});
            }
            _dbContext.Remove(varCountryToRemove);
            await _dbContext.SaveChangesAsync();
            Message = "Country removed from variety";
            return RedirectToAction(nameof(Details), new {id = variety});

        }

        public async Task<IActionResult> AddCountry(int id)
        {
            var model = await AdminAddCountryVarietyViewModel.Create(_dbContext, id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddCountry(int id, int CountryId)
        {
            var checkVar = await _dbContext.VarFull.Where(c => c.Id == id).AnyAsync();
            if(!checkVar)
            {
                ErrorMessage = "Variety not found";
                return RedirectToAction(nameof(Details), new {id = id});
            }
            var checkCountry = await _dbContext.Countries.Where(c => c.Id == CountryId).AnyAsync();
            if(!checkCountry)
            {
                ErrorMessage = "Country not found";
                return RedirectToAction(nameof(Details), new {id = id});                
            }

            var checkVarietyCounty = await _dbContext.VarCountires.Where(vc => vc.VarId == id && vc.CountryId == CountryId).AnyAsync();
            if(checkVarietyCounty)
            {
                ErrorMessage = "That country is already on this variety";
                return RedirectToAction(nameof(Details), new {id = id});
            }
            var newVarietyCountry = new VarCountries();
            newVarietyCountry.CountryId = CountryId;
            newVarietyCountry.VarId = id;
            _dbContext.Add(newVarietyCountry);
            await _dbContext.SaveChangesAsync();
            Message = "Country added to Variety";
            return RedirectToAction(nameof(Details), new {id = id});
        }

        
        
       
    }
}
