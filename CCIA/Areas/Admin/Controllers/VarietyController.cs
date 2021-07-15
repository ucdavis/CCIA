using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize(Roles = "EditVarieties")]
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

        [HttpPost]
        [Authorize(Roles = "EditVarieties")]
        public async Task<IActionResult> Edit(int id, AdminVarietyDetailsViewModel vm)
        {
            var varFullItem = await _dbContext.VarFull.Where(v => v.Id == id).FirstOrDefaultAsync();
            if(varFullItem == null || varFullItem.Id != vm.variety.Id)
            {
                ErrorMessage = "Variety not found";
                return RedirectToAction(nameof(Index));
            }
            if(varFullItem.Type == "official")
            {
                var varOffToUpdate = await _dbContext.VarOfficial.Where(v => v.VarOffId == id).FirstOrDefaultAsync();
                varOffToUpdate.VarOffName = vm.variety.Name;
                varOffToUpdate.CropId = vm.variety.CropId;
                varOffToUpdate.VarCategory = vm.variety.VarietyOfficial.VarCategory;
                varOffToUpdate.OECD = vm.variety.VarietyOfficial.OECD;
                varOffToUpdate.CCIACertified = vm.variety.VarietyOfficial.CCIACertified;
                varOffToUpdate.PendingCertification = vm.variety.VarietyOfficial.PendingCertification;
                varOffToUpdate.GermplasmEntity = vm.variety.VarietyOfficial.GermplasmEntity;
                varOffToUpdate.PlantPatent = vm.variety.VarietyOfficial.PlantPatent;
                varOffToUpdate.Pvp = vm.variety.VarietyOfficial.Pvp;
                varOffToUpdate.PvpExpDate = vm.variety.VarietyOfficial.PvpExpDate;
                varOffToUpdate.VarReviewBoard = vm.variety.VarietyOfficial.VarReviewBoard;
                varOffToUpdate.RiceQa = vm.variety.VarietyOfficial.RiceQa;
                varOffToUpdate.HistoricalName = vm.variety.VarietyOfficial.HistoricalName;
                varOffToUpdate.PrivateCode = vm.variety.VarietyOfficial.PrivateCode;
                varOffToUpdate.Confidential = vm.variety.VarietyOfficial.Confidential;
                varOffToUpdate.CCIACertifiedDate = vm.variety.VarietyOfficial.CCIACertifiedDate;
                varOffToUpdate.Active = vm.variety.VarietyOfficial.Active;
                varOffToUpdate.CtcApproved = vm.variety.VarietyOfficial.CtcApproved;
                varOffToUpdate.PlantPatentNum = vm.variety.VarietyOfficial.PlantPatentNum;
                varOffToUpdate.PvpDate = vm.variety.VarietyOfficial.PvpDate;
                varOffToUpdate.PvpYears = vm.variety.VarietyOfficial.PvpYears;
                varOffToUpdate.VarReviewBoardDate = vm.variety.VarietyOfficial.VarReviewBoardDate;
                varOffToUpdate.RiceQaColor = vm.variety.VarietyOfficial.RiceQaColor;
                varOffToUpdate.Experimental = vm.variety.VarietyOfficial.Experimental;
                varOffToUpdate.VarStatus = vm.variety.VarietyOfficial.VarStatus;
                varOffToUpdate.CCIACertifier = vm.variety.VarietyOfficial.CCIACertifier;
                varOffToUpdate.DescriptionOnFile = vm.variety.VarietyOfficial.DescriptionOnFile;
                varOffToUpdate.CtcDateApproved = vm.variety.VarietyOfficial.CtcDateApproved;
                varOffToUpdate.PlantPatentDate = vm.variety.VarietyOfficial.PlantPatentDate;
                varOffToUpdate.PvpNumber = vm.variety.VarietyOfficial.PvpNumber;
                varOffToUpdate.TitleV = vm.variety.VarietyOfficial.TitleV;
                varOffToUpdate.OtherStateCert = vm.variety.VarietyOfficial.OtherStateCert;
                varOffToUpdate.Turfgrass = vm.variety.VarietyOfficial.Turfgrass;
                varOffToUpdate.OwnerId = vm.variety.VarietyOfficial.OwnerId;
                varOffToUpdate.Ecoregion = vm.variety.VarietyOfficial.Ecoregion;
                varOffToUpdate.Elevation = vm.variety.VarietyOfficial.Elevation;
                varOffToUpdate.HarvestCountyId =  vm.variety.VarietyOfficial.HarvestCountyId;
                varOffToUpdate.StateHarvestedId = vm.variety.VarietyOfficial.StateHarvestedId;
                varOffToUpdate.DescHyperlink  = vm.variety.VarietyOfficial.DescHyperlink;
                varOffToUpdate.BriefDescription  = vm.variety.VarietyOfficial.BriefDescription;
                varOffToUpdate.Comments = vm.variety.VarietyOfficial.Comments;
            } else
            {
                var varFamToUpdate = await _dbContext.VarFamily.Where(v => v.VarFamId == id).FirstOrDefaultAsync();
                varFamToUpdate.VarFamName = vm.variety.Name;
                varFamToUpdate.VarOffId = vm.variety.VarietyFamily.VarOffId;
                varFamToUpdate.OECD = vm.variety.VarietyFamily.OECD;
                varFamToUpdate.PrivateCode = vm.variety.VarietyFamily.PrivateCode;
                varFamToUpdate.Experimental = vm.variety.VarietyFamily.Experimental;
                varFamToUpdate.Confidential = vm.variety.VarietyFamily.Confidential;
                varFamToUpdate.InUse = vm.variety.VarietyFamily.InUse;
                varFamToUpdate.Alias = vm.variety.VarietyFamily.Alias;
                varFamToUpdate.VarietyType = vm.variety.VarietyFamily.VarietyType;
                varFamToUpdate.DescHyperlink  = vm.variety.VarietyFamily.DescHyperlink;
                varFamToUpdate.VarComments = vm.variety.VarietyFamily.VarComments;
                varFamToUpdate.UpdateComments = vm.variety.VarietyFamily.UpdateComments;
            }

            if(ModelState.IsValid){                   
                await _dbContext.SaveChangesAsync();
                Message = "Variety updated";
            } else {
                ErrorMessage = "Something went wrong";                 
                var model = await AdminVarietyDetailsViewModel.Edit(_dbContext,_helper, id);                        
                return View(model);
            }

            return RedirectToAction(nameof(Details), new {id = varFullItem.Id});
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
