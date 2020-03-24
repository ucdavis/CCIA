using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models.SampleLabResultsViewModel;
using CCIA.Helpers;


namespace CCIA.Controllers
{
    public class SampleLabResultsController : ClientController
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

            var errorList = await LabResultsCheckStandards.CheckStandardsFromLabs(_dbContext, results.Labs);

            if (errorList.HasWarnings)
            {
                if (errorList.PurityError != null)
                {
                    ModelState.AddModelError("Labs.PurityPercent", errorList.PurityError);
                }
                if (errorList.InertError != null)
                {
                    ModelState.AddModelError("Labs.InertPercent", errorList.InertError);
                }
                if (errorList.OtherCropError != null)
                {
                    ModelState.AddModelError("Labs.OtherCropPercent", errorList.OtherCropError);
                }
                if (errorList.WeedSeedError != null)
                {
                    ModelState.AddModelError("Labs.WeedSeedPercent", errorList.WeedSeedError);
                }
                if (errorList.OtherVarietyError != null)
                {
                    ModelState.AddModelError("Labs.OtherVarietyPercent", errorList.OtherVarietyError);
                }
                if (errorList.ForeignMaterialError != null)
                {
                    ModelState.AddModelError("Labs.ForeignMaterialPercent", errorList.ForeignMaterialError);
                }
                if (errorList.SplitsAndCracksError != null)
                {
                    ModelState.AddModelError("Labs.SplitsAndCracksPercent", errorList.SplitsAndCracksError);
                }
                if (errorList.BadlyDiscoloredError != null)
                {
                    ModelState.AddModelError("Labs.BadlyDiscoloredPercent", errorList.BadlyDiscoloredError);
                }
                if (errorList.ChewingInsectDamageError != null)
                {
                    ModelState.AddModelError("Labs.ChewingInsectDamagePercent", errorList.ChewingInsectDamageError);
                }
                if (errorList.NoxiousSeedError != null)
                {
                    ModelState.AddModelError("Labs.NoxiousCount", errorList.NoxiousSeedError);
                }
                if (errorList.PurityGramsError != null)
                {
                    ModelState.AddModelError("Labs.PurityGrams", errorList.PurityGramsError);
                }
                if (errorList.NoxiousGramsError != null)
                {
                    ModelState.AddModelError("Labs.NoxiousGrams", errorList.NoxiousGramsError);
                }
                if (errorList.BushelWeightError != null)
                {
                    ModelState.AddModelError("Labs.BushelWeight", errorList.BushelWeightError);
                }
                if (errorList.GermError != null)
                {
                    ModelState.AddModelError("Labs.GermPercent", errorList.GermError);
                }
                if (errorList.Assay1Error != null)
                {
                    ModelState.AddModelError("Labs.AssayTest", errorList.Assay1Error);
                }
                // if (errorList.Assay2Error != null)
                // {
                //     ModelState.AddModelError("Labs.AssayTest2", errorList.Assay2Error);
                // }

                ModelState.AddModelError(string.Empty, "Double check value or select continue as rejected lot");

                var errorModel = await SampleLabResultsViewModel.ReUse(_dbContext, results.Labs);
                return View(errorModel);
            }

            //var resultsToUpdate = await _dbContext.SampleLabResults.Where(s => s.SeedsId == id).FirstOrDefaultAsync();


            var model = await SampleLabResultsViewModel.ReUse(_dbContext, results.Labs);
            return View(model);
        }

    }
}