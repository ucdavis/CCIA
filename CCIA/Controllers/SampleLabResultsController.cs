using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.SampleLabResultsViewModel;


namespace CCIA.Controllers
{
    public class SampleLabResultsController : SuperController
    {

        private readonly CCIAContext _dbContext;

        public SampleLabResultsController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await SampleLabResultsViewModel.Create(_dbContext, id);
            return View(model);
        }       

    }
}