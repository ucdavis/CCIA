using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;
using CCIA.Models.AgComm;
using CCIA.Models.SeedsViewModels;

namespace CCIA.Controllers.AgComm
{


    public class SeedsController : AgCommController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;

        public SeedsController(CCIAContext dbContext, IFullCallService helper,IFileIOService fileIOService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileIOService;

        }

        public async Task<IActionResult> Index(int id, AgCommSeedSearchViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(!vm.Search){
                var freshmodel = await AgCommSeedSearchViewModel.Create(_dbContext, null, countyId.Value);
                return View(freshmodel);  
            }
                var model = await AgCommSeedSearchViewModel.Create(_dbContext, vm, countyId.Value);                
                return View(model);            
        }

        public async Task<IActionResult> Details(int id)
        {            
            var model = await AgCommSeedsViewModel.Create(_dbContext, id);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }    
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.seed.ApplicantOrganization.CountyId != countyId.Value )
            {
                ErrorMessage = "Applicant not in your county.";
                return RedirectToAction(nameof(Index));

            }        
            return View(model);
        }    

        public async Task<IActionResult> SIR(int id)
        {            
            var model = await ClientSeedsViewModel.CreateSIR(_dbContext, _helper, id);
            if(model.seed == null)
            {
                ErrorMessage = "Seed lot not found.";
                return RedirectToAction(nameof(Index));
            }            
            return View("~/Areas/Client/Views/Seeds/SIR.cshtml",model);
        }      

    }
}