using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Services;

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
            if(!vm.Search){
                var freshmodel = await AgCommSeedSearchViewModel.Create(_dbContext, null);
                return View(freshmodel);  
            }
                var model = await AgCommSeedSearchViewModel.Create(_dbContext, vm);                
                return View(model);            
        }

    }
}