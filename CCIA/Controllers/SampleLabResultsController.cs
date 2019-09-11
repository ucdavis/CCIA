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

        [HttpPost]
        public async Task<IActionResult> Edit(int id, SampleLabResultsViewModel results)
        {
            results.Labs.PurityPercent = results.Labs.PurityPercent / 100;
            results.Labs.InertPercent = results.Labs.InertPercent / 100;
            results.Labs.OtherCropPercent = results.Labs.OtherCropPercent / 100;
            results.Labs.OtherVarietyPercent = results.Labs.OtherVarietyPercent / 100;
            results.Labs.WeedSeedPercent = results.Labs.WeedSeedPercent / 100;
            results.Labs.GermPercent = results.Labs.GermPercent / 100;
            results.Labs.HardSeedPercent = results.Labs.HardSeedPercent / 100;
            results.Labs.BadlyDiscoloredPercent = results.Labs.BadlyDiscoloredPercent / 100;
            results.Labs.ForeignMaterialPercent = results.Labs.ForeignMaterialPercent / 100;
            results.Labs.SplitsAndCracksPercent = results.Labs.SplitsAndCracksPercent / 100;
            results.Labs.ChewingInsectDamagePercent = results.Labs.ChewingInsectDamagePercent / 100;

            if(results.Labs.PurityPercent < 85)
            {
                 ModelState.AddModelError("results.Labs.PurityPercent", "Purity does not fall withing crop standard");
                 var errorModel = await SampleLabResultsViewModel.ReUse(_dbContext, results.Labs);
                 return View(errorModel);
            }

            //var resultsToUpdate = await _dbContext.SampleLabResults.Where(s => s.SeedsId == id).FirstOrDefaultAsync();

            
            var model = await SampleLabResultsViewModel.ReUse(_dbContext, results.Labs);
            return View(model);
        }      

    }
}