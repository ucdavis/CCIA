using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;
using CCIA.Models.AgComm;

namespace CCIA.Controllers.AgComm
{


    public class BulkSalesController : AgCommController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;

        public BulkSalesController(CCIAContext dbContext, IFullCallService helper,IFileIOService fileIOService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileIOService;

        }

        public async Task<IActionResult> Index()
        {
            var model = await AgCommBulkSalesCertificateSearchViewModel.Create(_dbContext, null, _helper, 0);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Index(AgCommBulkSalesCertificateSearchViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            var model = await AgCommBulkSalesCertificateSearchViewModel.Create(_dbContext, vm, _helper, countyId.Value);
            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await _helper.FullBulkSalesCertificates().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Bulk Sales Certificate not found!";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var countyId = await _dbContext.Organizations.Where( o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            if(!countyId.HasValue)
            {
                countyId = 0;
            }
            if(model.ConditionerOrganization.CountyId != countyId.Value)
            {
                ErrorMessage = "Bulk Sales Certificate not from your county.";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }
    }
}