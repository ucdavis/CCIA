using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Services;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.ViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;
using CCIA.Helpers;
using System.IO;

namespace CCIA.Controllers.Admin
{

    public class TagsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;
        private readonly INotificationService _notificationService;

        public TagsController(CCIAContext dbContext, IFullCallService helper, IFileIOService fileService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileService;
            _notificationService = notificationService;
        }

        // TODO: Add app warnings for apps from Seed_apps for SID or App for Potatoes        
        // TODO: Check weight from PO Pounds Harvested for new tags/overage check

        public ActionResult Index ()
        {
            return View();
        }

        public ActionResult Lookup()
        {
            return View();
        }

        public async Task<IActionResult> ToggleFollowup(int id)
        {
            var tagToToggle = await _dbContext.Tags.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(tagToToggle == null)
            {
                ErrorMessage = "Tag not found";
                return RedirectToAction(nameof(Index));
            }
            tagToToggle.FollowUp = !tagToToggle.FollowUp;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = id});
        }

        public async Task<IActionResult> Create()
        {
            var model = await TagCreateEditViewModel.Create(_dbContext, _helper, 0);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int id, TagCreateEditViewModel vm)
        {   
            var tagToCreate = new Tags();
            var newTag = vm.tag;

            tagToCreate.SeedsID = newTag.SeedsID;
            tagToCreate.BlendId = newTag.BlendId;
            tagToCreate.AppId = newTag.AppId;
            tagToCreate.TagType = newTag.TagType;
            tagToCreate.BagSize = newTag.BagSize;
            tagToCreate.WeightUnit = newTag.WeightUnit;
            tagToCreate.TagClass = newTag.TagClass;
            tagToCreate.Alias = newTag.Alias;
            tagToCreate.TaggingOrg = newTag.TaggingOrg;            
            tagToCreate.CountRequested = newTag.CountRequested;
            tagToCreate.CountUsed = newTag.CountUsed;
            tagToCreate.ExtrasOverrun = newTag.ExtrasOverrun;
            tagToCreate.CoatingPercent = newTag.CoatingPercent;
            tagToCreate.SeriesRequest = newTag.SeriesRequest;
            tagToCreate.Bulk = newTag.Bulk;
            tagToCreate.AnalysisRequested = newTag.AnalysisRequested;
            tagToCreate.Statement = newTag.Statement;
            tagToCreate.HowDeliver = newTag.HowDeliver;
            tagToCreate.DateNeeded = newTag.DateNeeded;
            tagToCreate.Comments =newTag.Comments;
            tagToCreate.DateRequested = DateTime.Now;
            tagToCreate.TagBagging = null;
            tagToCreate.Stage = TagStages.Printing.GetDisplayName();
            
            if(ModelState.IsValid){
                _dbContext.Add(tagToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Tag Created";
                
                if(!string.IsNullOrWhiteSpace(newTag.PlantingStockNumber))
                {
                    var oecd = new OECD();
                    var seed = await _helper.FullSeeds().Where(s => s.Id == newTag.SeedsID).FirstAsync();
                     oecd.SeedsId = tagToCreate.SeedsID;
                    oecd.VarietyId = seed.OfficialVarietyId;
                    oecd.Pounds = Convert.ToInt32(tagToCreate.LotWeightRequested.Value);
                    oecd.CertNumber = seed.CertNumber;
                    tagToCreate.OECDTagType = newTag.OECDTagType;
                    oecd.ClassId = newTag.OECDTagType;
                    oecd.CloseDate = newTag.DateSealed.Value;
                    oecd.ConditionerId = seed.ConditionerId;
                    oecd.CountryId = newTag.OECDCountryId;
                    oecd.LotNumber = seed.LotNumber;
                    oecd.ShipperId = newTag.TaggingOrg;
                    oecd.DateRequested = DateTime.Now;
                    oecd.NotCertified = newTag.OECDTagType == 5;
                    oecd.DataEntryDate = DateTime.Now;
                    oecd.DataEntryUser = User.FindFirstValue(ClaimTypes.Name);
                    oecd.DomesticOrigin = seed.OriginCountry == 58;
                    oecd.ReferenceNumber = newTag.PlantingStockNumber;
                    oecd.Canceled = false;
                    oecd.TagsRequested = newTag.CountRequested.Value;
                    oecd.AdminComments = newTag.AdminComments;
                    oecd.OECDNumber = seed.OriginCountry == 102 ? $"USA-CA-{seed.CertNumber}" : $"USA-{seed.StateOfOrigin.StateProvinceCode}/CA-{seed.CertNumber}-{seed.LotNumber}";
                    oecd.TagId = tagToCreate.Id;

                    _dbContext.Add(oecd);
                    await _dbContext.SaveChangesAsync();
                    Message = "Tag & OECD created";     

                    var msg = "; OECD ID:  " + oecd.Id.ToString() + " Tag ID: " + tagToCreate.Id;
                    tagToCreate.AdminComments += msg;
                    oecd.AdminComments += msg;
                    seed.Remarks += msg;
                    await _dbContext.SaveChangesAsync();
                }
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await TagCreateEditViewModel.Create(_dbContext, _helper, id);
                return View(vm); 
            }
            return RedirectToAction(nameof(Details), new { id = tagToCreate.Id });  

        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await TagCreateEditViewModel.Create(_dbContext, _helper, id);         
            return View(model);
        } 

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TagCreateEditViewModel vm)
        {   
            var tagToUpdate = await _dbContext.Tags.Where(a => a.Id == id).FirstAsync();
            var edit = vm.tag;
            tagToUpdate.SeedsID = edit.SeedsID;
            tagToUpdate.BlendId = edit.BlendId;
            tagToUpdate.AppId = edit.AppId;
            tagToUpdate.TagType = edit.TagType;
            tagToUpdate.TagClass = edit.TagClass;
            tagToUpdate.Alias = edit.Alias;
            tagToUpdate.BagSize = edit.BagSize;
            tagToUpdate.WeightUnit = edit.WeightUnit;
            tagToUpdate.CountRequested = edit.CountRequested;
            tagToUpdate.CountUsed = edit.CountUsed;
            tagToUpdate.ExtrasOverrun = edit.ExtrasOverrun;
            tagToUpdate.CoatingPercent = edit.CoatingPercent;
            tagToUpdate.SeriesRequest = edit.SeriesRequest;
            tagToUpdate.Bulk = edit.Bulk;
            tagToUpdate.AnalysisRequested = edit.AnalysisRequested;
            tagToUpdate.Statement = edit.Statement;
            tagToUpdate.HowDeliver = edit.HowDeliver;
            tagToUpdate.UserModified = User.FindFirstValue(ClaimTypes.Name);
            tagToUpdate.DateModified = DateTime.Now;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Tag Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await TagCreateEditViewModel.Create(_dbContext, _helper, id);
                return View(vm); 
            }

            return RedirectToAction(nameof(Details), new { id = tagToUpdate.Id });  

        }  

        public async Task<IActionResult> GetTagFile(int id, int certYear)
        {
            var tagDoc = await _dbContext.TagDocuments.Where(d => d.Id == id).FirstOrDefaultAsync();         
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadTagFile(tagDoc, certYear), contentType, tagDoc.Link);
        }

        [HttpPost]
        public async Task<IActionResult> UploadTagDocument(int id, int certYear, IFormFile file)
        {
           var tag = await _dbContext.Tags.Where(t => t.Id==id).FirstOrDefaultAsync();
           if(tag == null)
           {
               ErrorMessage = "Tag not found";
               return RedirectToAction(nameof(Index));
           }
           var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

           if(_fileService.CheckDeniedExtension(ext))
           {
               ErrorMessage = "File extension not allowed!";
               return RedirectToAction(nameof(Details), new { id = id });
           }      
           
           if(file.Length >0)
           {
               await _fileService.SaveTagDocument(tag.Id, certYear, file); 
               var tagDoc = new TagDocuments();
               tagDoc.Link = file.FileName;
               tagDoc.TagId = tag.Id;
                _dbContext.Add(tagDoc);
                await _dbContext.SaveChangesAsync();
           }
           return RedirectToAction(nameof(Details), new { id = id }); 
        }




        public async Task<IActionResult> Process(ProcessViewModel vm)
        {
            var model = await ProcessViewModel.Create(_dbContext, vm, _helper);         
            return View(model);
        }   

        public async Task<IActionResult> Details(int id)    
        {            
            var tag = _helper.FullTag();
            var model = await tag
                .Include(t => t.TagBagging)
                .Include(t => t.EmployeePrinted)
                .Include(t => t.Documents)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
            if(model.Stage ==  TagStages.Requested.GetDisplayName() && model.SeriesRequest)
            {
                ViewBag.AllowConditionerPrint = await _dbContext.CondStatus.Where(c => c.OrgId == model.TaggingOrg && c.Year == model.ConditionerYearEntered && c.PrintSeries).AnyAsync();
            }            

            if(model == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        public async Task<IActionResult> Previous(int id)
        {
            var previousId = await _dbContext.Tags.Where(t => t.Id < id).OrderBy(t => t.Id).Select(t => t.Id).LastOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Next(int id)
        {
            var previousId = await _dbContext.Tags.Where(t => t.Id > id).OrderBy(t => t.Id).Select(t => t.Id).FirstOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }




        [HttpPost]
        public async Task<IActionResult> RecordSeries(int id, string Letter, int Start, int End, bool Void)
        {
            if(!Void)
            {
                var test = await _dbContext.TagSeries.Include(s => s.Tag).Where(s => s.Letter == Letter && !s.Tag.Bulk && !s.Void && ((End >= s.Start && Start <= s.End) || (Start >= s.Start && End <= s.End))).AnyAsync();
                if(test)
                {
                    ErrorMessage = "Tag Series already exists with that Letter & range";
                    return RedirectToAction(nameof(Details), new {id = id});
                }
            }            
            var series = new TagSeries();
            series.TagId = id;
            series.Letter = Letter;
            series.Start = Start;
            series.End = End;
            series.Void = Void;

            _dbContext.Add(series);
            await _dbContext.SaveChangesAsync();
            Message = "New Series Recorded";

            return RedirectToAction(nameof(Details), new {id = id});
        }

        [HttpPost]
        public async Task<IActionResult> DeleteSeries(int id)
        {            
            var series = await _dbContext.TagSeries.Where(s => s.Id == id).FirstOrDefaultAsync();
            var tag = series.TagId;
            _dbContext.Remove(series);
            await _dbContext.SaveChangesAsync();
            Message = "Series entry deleted";
            return RedirectToAction(nameof(Details), new {id = tag});
        }

        [HttpPost]
        public async Task<IActionResult> UpdateAdminComments(int Id, string AdminComments)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == Id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.AdminComments = AdminComments;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = Id});
        }

        [HttpPost]
        public async Task<IActionResult> UpdateTrackingNumber(int Id, string TrackingNumber)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == Id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.TrackingNumber = TrackingNumber;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = Id});
        }

        [HttpPost]
        public async Task<IActionResult> PrintedTag(int id)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID Not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.Stage = TagStages.PendingFile.GetDisplayName();
            tag.UserPrinted = User.FindFirstValue(ClaimTypes.Name);
            tag.PrintedDate = DateTime.Now;
            await _notificationService.TagPrinted(tag);
            await _dbContext.SaveChangesAsync();

            Message = "Tag marked printed";
            return RedirectToAction(nameof(Details), new {id = id}); 
        }


        [HttpPost]
        public async Task<IActionResult> FileTag(int id)
        {
            var tag = await _dbContext.Tags.Where(t => t.Id == id).FirstOrDefaultAsync();
            if(tag == null)
            {
                ErrorMessage = "Tag ID Not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.Stage = TagStages.Complete.GetDisplayName();
            await _notificationService.TagFiled(tag);

            await _dbContext.SaveChangesAsync();

            Message = "Tag marked complete";
            return RedirectToAction(nameof(Details), new {id = id}); 
        }

        public async Task<IActionResult> Search()
        {
            var model = await AdminTagsSearchViewModel.Create(_dbContext, null, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(AdminTagsSearchViewModel vm)
        {
            var model = await AdminTagsSearchViewModel.Create(_dbContext, vm, _helper);
            return View(model);
        }

        public async Task<IActionResult> SearchSeries()
        {
            var model = await AdminTagsSeriesSearchViewModel.Create(_dbContext, null);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SearchSeries(AdminTagsSeriesSearchViewModel vm)
        {
            var model = await AdminTagsSeriesSearchViewModel.Create(_dbContext, vm);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AcceptTag(int id, bool conditionerPrint = false)
        {
            var tag = await _helper.FullTag().Where(t => t.Id == id).FirstOrDefaultAsync();            
            if(tag == null)
            {
                ErrorMessage = "Tag ID Not found!";
                return RedirectToAction(nameof(Index));
            }
            tag.Stage = TagStages.Printing.GetDisplayName();
            tag.UserApproved = User.FindFirstValue(ClaimTypes.Name);
            tag.ApprovedDate = DateTime.Now;

            Message = "Tag Accepted";

            var oecd = new OECD();
            if(tag.OECD)
            {
                
                oecd.SeedsId = tag.SeedsID;
                oecd.BID = tag.BlendId;
                oecd.VarietyId = tag.VarietyId;
                oecd.Pounds = Convert.ToInt32(tag.LotWeightRequested.Value);
                oecd.CertNumber = tag.CertNumber;
                oecd.ClassId = tag.OECDTagType;
                oecd.CloseDate = tag.DateSealed.Value;
                oecd.ConditionerId = tag.SeedsID != null ? tag.Seeds.ConditionerId : tag.Blend.ConditionerId;
                oecd.CountryId = tag.OECDCountryId;
                oecd.LotNumber = tag.LotNumber;
                oecd.ShipperId = tag.TaggingOrg;
                oecd.DateRequested = tag.DateEntered;
                oecd.NotCertified = tag.OECDTagType == 5;
                oecd.DataEntryDate = DateTime.Now;
                oecd.DataEntryUser = User.FindFirstValue(ClaimTypes.Name);
                oecd.DomesticOrigin = tag.SeedsID != null ? tag.Seeds.OriginCountry == 58 : true;
                oecd.ReferenceNumber = tag.PlantingStockNumber;
                oecd.Canceled = false;
                oecd.TagsRequested = tag.CountRequested.Value;
                oecd.AdminComments = tag.AdminComments;
                oecd.OECDNumber = tag.BlendId != null || tag.Seeds.OriginCountry == 102  ? $"USA-CA-{tag.CertNumber}" : $"USA-{tag.Seeds.StateOfOrigin.StateProvinceCode}/CA-{tag.Seeds.CertNumber}-{tag.Seeds.LotNumber}";
                oecd.TagId = tag.Id;

                _dbContext.Add(oecd);

            }

            if(conditionerPrint)
            {
                tag.Stage = TagStages.PendingFile.GetDisplayName();
                await _notificationService.TagApprovedForConditionerPrint(tag);
                Message = "Tag Approved for Conditioner Print";
            } else
            {
                await _notificationService.TagApproved(tag);
            }
            
            await _dbContext.SaveChangesAsync();

            if(tag.OECD)
            {
                var msg = $"; OECD File {oecd.Id}";
                tag.AdminComments = $"{tag.AdminComments}{msg}";
                tag.OECDId = oecd.Id;
                if(tag.SeedsID != null)
                {
                    tag.Seeds.Remarks = $"{tag.Seeds.Remarks}{msg}";
                } else 
                {
                    tag.Blend.Comments = $"{tag.Blend.Comments}{msg}";
                }                
                await _dbContext.SaveChangesAsync(); 
                Message = $"{Message} & OECD File saved";               
            }

            return RedirectToAction(nameof(Details), new {id = id});            
        }

        public async Task<IActionResult> EditSeries(int id)
        {
            var model = await _dbContext.TagSeries.Where(ts => ts.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Tag Series not found";
            }            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSeries(int id, TagSeries ts)
        {
            var tagSeriesToUpdate = await _dbContext.TagSeries.Where(ts => ts.Id == id).FirstOrDefaultAsync();
            if(tagSeriesToUpdate == null || tagSeriesToUpdate.Id != ts.Id)
            {
                ErrorMessage = "Tag Series not found";
                return RedirectToAction(nameof(Process));
            }

            tagSeriesToUpdate.Letter = ts.Letter;
            tagSeriesToUpdate.Start = ts.Start;
            tagSeriesToUpdate.End = ts.End;
            tagSeriesToUpdate.Void = ts.Void;

            if(ModelState.IsValid){               
                await _dbContext.SaveChangesAsync();
                Message = "Tag Series updated";
            }
             return RedirectToAction(nameof(Details), new { id = tagSeriesToUpdate.TagId });  

        }
    }
}