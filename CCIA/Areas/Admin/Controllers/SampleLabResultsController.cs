using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models.SampleLabResultsViewModel;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Controllers.Admin
{

    public class SampleLabResultsController : AdminController
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
            var labs = results.Labs;
            labs.PurityPercent = labs.PurityPercent / 100;
            labs.InertPercent = labs.InertPercent / 100;
            labs.OtherCropPercent = labs.OtherCropPercent / 100;
            labs.OtherVarietyPercent = labs.OtherVarietyPercent / 100;
            labs.WeedSeedPercent = labs.WeedSeedPercent / 100;
            labs.GermPercent = labs.GermPercent / 100;
            labs.HardSeedPercent = labs.HardSeedPercent / 100;
            labs.BadlyDiscoloredPercent = labs.BadlyDiscoloredPercent / 100;
            labs.ForeignMaterialPercent = labs.ForeignMaterialPercent / 100;
            labs.SplitsAndCracksPercent = labs.SplitsAndCracksPercent / 100;
            labs.ChewingInsectDamagePercent = labs.ChewingInsectDamagePercent / 100;
            labs.DormantSeedPercent = labs.DormantSeedPercent / 100;

            var errorList = await LabResultsCheckStandards.CheckStandardsFromLabs(_dbContext, labs);

            if (errorList.HasWarnings)
            {
                if (errorList.PurityError != null)
                {
                    ModelState.AddModelError("Labs.PurityPercent", errorList.PurityError);
                }
                if (errorList.OtherKindError != null)
                {
                    ModelState.AddModelError("Labs.OtherKindPercent", errorList.OtherKindError);
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
                if (errorList.DodderError != null)
                {
                    ModelState.AddModelError("Labs.DodderGrams", errorList.DodderError);
                }
                if (errorList.GeneralError != null)
                {
                    ModelState.AddModelError(string.Empty, errorList.GeneralError);
                }              

                ModelState.AddModelError(string.Empty, "Double check value or mark that section as not passing");

                var errorModel = await SampleLabResultsViewModel.ReUse(_dbContext, labs);
                return View(errorModel);
            }

            // Passed verification, save and redirect
            var labsToUpdate = await _dbContext.SampleLabResults.Where(l => l.SeedsId == results.Labs.SeedsId).FirstOrDefaultAsync();
            if(errorList.AssayNeeded)
            {
                labsToUpdate.AssayResults = labs.AssayResults;
                labsToUpdate.AssayTest = labs.AssayTest;
            } else {
                labsToUpdate.AssayResults = "N";
            }
            
            labsToUpdate.BadlyDiscoloredPercent = labs.BadlyDiscoloredPercent;
            labsToUpdate.BushelWeight = labs.BushelWeight;
            if(!labsToUpdate.CciaConfirmed && labs.CciaConfirmed){
                labsToUpdate.ConfirmDate = DateTime.Now;
                labsToUpdate.ConfirmUser =  User.FindFirstValue(ClaimTypes.Name);
            }
            labsToUpdate.CciaConfirmed = labs.CciaConfirmed;
            
            labsToUpdate.ChewingInsectDamagePercent = labs.ChewingInsectDamagePercent;
            labsToUpdate.Comments = labs.Comments;
            labsToUpdate.PrivateLabDate = labs.PrivateLabDate;
            labsToUpdate.DodderGrams = labs.DodderGrams;
            labsToUpdate.ForeignMaterialPercent = labs.ForeignMaterialPercent;
            labsToUpdate.ForeignMaterialsComments = labs.ForeignMaterialsComments;
            labsToUpdate.GermPercent = labs.GermPercent;
            labsToUpdate.GermResults = labs.GermResults;
            labsToUpdate.HardSeedPercent = labs.HardSeedPercent;
            labsToUpdate.DormantSeedPercent = labs.DormantSeedPercent;
            labsToUpdate.InertComments = labs.InertComments;
            labsToUpdate.InertPercent = labs.InertPercent;
            labsToUpdate.NoxiousComments = labs.NoxiousComments;
            labsToUpdate.NoxiousCount = labs.NoxiousCount;
            labsToUpdate.NoxiousGrams = labs.NoxiousGrams;
            labsToUpdate.OtherCropComments = labs.OtherCropComments;
            labsToUpdate.OtherCropCount = labs.OtherCropCount;
            labsToUpdate.OtherCropPercent = labs.OtherCropPercent;
            labsToUpdate.OtherKindComments = labs.OtherKindComments;
            labsToUpdate.OtherKindPercent = labs.OtherKindPercent;
            labsToUpdate.OtherVarietyComments = labs.OtherVarietyComments;
            labsToUpdate.OtherVarietyCount = labs.OtherVarietyCount;
            labsToUpdate.OtherVarietyPercent = labs.OtherVarietyPercent;
            labsToUpdate.PrivateLabDate =labs.PrivateLabDate;
            labsToUpdate.PrivateLabId = labs.PrivateLabId;
            labsToUpdate.PrivateLabNumber = labs.PrivateLabNumber;
            labsToUpdate.PurityComments = labs.PurityComments;
            labsToUpdate.PurityGrams = labs.PurityGrams;
            labsToUpdate.PurityPercent = labs.PurityPercent;
            labsToUpdate.PurityResults = labs.PurityResults;
            labsToUpdate.SplitsAndCracksPercent = labs.SplitsAndCracksPercent;
            labsToUpdate.WeedSeedComments = labs.WeedSeedComments;
            labsToUpdate.WeedSeedCount = labs.WeedSeedCount;
            labsToUpdate.WeedSeedPercent = labs.WeedSeedPercent;
            labsToUpdate.UpdateUser = User.FindFirstValue(ClaimTypes.Name);

            

             if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Lab Results Updated";
                return RedirectToAction("Details", "Seeds", new { id = labs.SeedsId }); 
            } else {
                ErrorMessage = "Something went wrong";                         
            }

            var model = await SampleLabResultsViewModel.ReUse(_dbContext, results.Labs);
            return View(model);
        }

       
    }
}