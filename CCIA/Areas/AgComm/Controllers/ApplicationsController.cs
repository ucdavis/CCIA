using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;

namespace CCIA.Controllers.AgComm
{


    public class ApplicationsController : AgCommController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;

        public ApplicationsController(CCIAContext dbContext, IFullCallService helper,IFileIOService fileIOService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileIOService;

        }

        public async Task<IActionResult> Index(int id, AgCommSearchViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.CountyId).FirstAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(!vm.Search){
                var freshmodel = await AgCommSearchViewModel.Create(_dbContext, null, _helper, countyId.Value);
                return View(freshmodel);  
            }
                var model = await AgCommSearchViewModel.Create(_dbContext, vm, _helper, countyId.Value);
                
                return View(model);            
        }  

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.CountyId).FirstAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            var model = await _dbContext.Applications.Where(a => a.Id == id && a.FarmCounty == countyId)
                .Include(a => a.GrowerOrganization)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.Certificates)
                .Include(a => a.PlantingStocks)
                .ThenInclude(p => p.PsClassNavigation)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownCountry)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedCountry)
                .Include(a => a.FieldHistories).ThenInclude(fh => fh.FHCrops)
                .FirstOrDefaultAsync();

            if(model == null)
            {
                ErrorMessage = "That app was either not found or does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }    
            return View(model);
        }

        
        public async Task<IActionResult> FieldMap(int id)
        {
            var model = await AdminMapFieldsViewModel.SingleMap(_dbContext, id, _helper);
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.CountyId).FirstAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.details.application.FarmCounty != countyId)
            {
                ErrorMessage = "That app does not belong in your county.";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);  
        }

        
        public async Task<IActionResult> GetCertificateFile(int id, string link)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            
            if(app == null)
            {
                ErrorMessage = "App not found";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.CountyId).FirstAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(app.FarmCounty != countyId)
            {
            
                ErrorMessage = "That app does not belong in your county.";
                return  RedirectToAction(nameof(Index));
            }
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadCertificateFile(app, link), contentType, link);
        }
    }
}