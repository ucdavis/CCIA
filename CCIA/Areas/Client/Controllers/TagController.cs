using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;
using CCIA.Services;
using System.IO;

namespace CCIA.Controllers.Client
{
    public class TagController : ClientController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;
        private readonly INotificationService _notificationService;

        public TagController(CCIAContext dbContext, IFullCallService helper, IFileIOService fileService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileService;
            _notificationService = notificationService;
        }
       

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {      
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.Tags.Where(o => o.TaggingOrg == orgId).MaxAsync(x => (int?)x.DateRequested.Value.Year);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }           
            var model = await TagIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);            
            return View(model);
        }

        public async Task<IActionResult> Details(int id)    
        {  
            var model = await TagCreateEditViewModel.Create(_dbContext, _helper, id); 
            if(model.tag == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }
            
            if(model.tag.TaggingOrg != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the company that requested that tag";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        private async Task<bool> checkTagPermission(int orgId)
        {
            return await _dbContext.CondStatus.Where(s => s.OrgId == orgId && s.Year == CertYearFinder.ConditionerYear && s.Status != "O").AnyAsync();
        }

        public async Task<IActionResult> InitiateBulk()
        {
            var model = await _dbContext.Crops.Where(c => c.CertifiedCrop.Value).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Initiate()
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, string TagTarget)
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await ClientTagRequestViewModel.Create(_dbContext, _helper, id, TagTarget, orgId);
            if(model.request == null)
            {
                ErrorMessage = "Tag request could not be started. Please double check ID & Tag type.";
                return RedirectToAction(nameof(Index));
            }
        
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTag(int id, ClientTagRequestViewModel model)
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var newTag = new Tags();
            var submittedTag = model.request;
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            switch(submittedTag.Target)
            {
                case "SID":
                    newTag.SeedsID = submittedTag.Id;
                    break;
                case "BID":
                    newTag.BlendId = submittedTag.Id;
                    break;
                default:
                    newTag.AppId = submittedTag.Id;
                    break;
            }
            newTag.TagClass = submittedTag.TagClass;
            newTag.DateRequested = DateTime.Now;
            newTag.DateNeeded = submittedTag.DateNeeded;
            newTag.CoatingPercent = submittedTag.CoatingPercent;
            newTag.WeightUnit = submittedTag.WeightUnit;
            newTag.CountRequested = submittedTag.CountRequested;
            newTag.ExtrasOverrun = 0;
            newTag.BagSize = submittedTag.BagSize;
            newTag.TagType = submittedTag.TagType;
            newTag.Comments = submittedTag.Comments;
            newTag.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            // TODO These are the same column. Get rid of one?
            newTag.Contact = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            // TODO DateEntered & DateRequested are duplicates. Get rid of one
            newTag.DateEntered = DateTime.Now;
            newTag.TaggingOrg = orgId;
            newTag.Bulk = false;
            newTag.Pretagging = submittedTag.Pretagging;
            newTag.AnalysisRequested = submittedTag.AnalysisRequested;
            newTag.SeriesRequest = submittedTag.SeriesRequest;
            newTag.HowDeliver = submittedTag.HowDeliver;
            newTag.Stage = TagStages.Requested.GetDisplayName();
            newTag.Alias = submittedTag.Alias;
            if(submittedTag.Target == "SID")
            {
                newTag.OECD = submittedTag.OECD;
                if(submittedTag.OECD)
                {
                    newTag.PlantingStockNumber = submittedTag.PlantingStockLotNumber;
                    newTag.OECDTagType = submittedTag.OECDTagType;
                    newTag.DateSealed = submittedTag.DateSealed;
                    newTag.OECDCountryId = submittedTag.OECDCountryId;
                }

            } else {
                newTag.OECD = false;
            }

            ModelState.Clear();    
            if(submittedTag.DateNeeded < DateTime.Now.AddDays(1))       
            {
                ModelState.AddModelError("request.DateNeeded","Date needed must be greater than 2 days out");
            }
            if (TryValidateModel(model))
            {   
                _dbContext.Add(newTag);                
                await _dbContext.SaveChangesAsync();  
                await _notificationService.TagSubmitted(newTag);
                await _dbContext.SaveChangesAsync();             
                Message = "Tag submitted!";
                return RedirectToAction("Details", new { id = newTag.Id });
            }
            
            var retryModel = await ClientTagRequestViewModel.Edit(_dbContext, _helper, submittedTag.Id, submittedTag.Target, orgId, submittedTag);
            return View("Create", retryModel);
        }


        [HttpPost]
        public async Task<IActionResult> UpdateOECD(TagCreateEditViewModel vm)
        {
            var oecd = await _dbContext.OECD.Where(o => o.Id == vm.tag.OECDFile.Id).FirstOrDefaultAsync();
            var tag = await _dbContext.Tags.Where(t => t.Id == vm.tag.Id).FirstOrDefaultAsync();
            if(oecd == null || tag == null || tag.OECDId != oecd.Id || oecd.DatePrinted != null)
            {
                ErrorMessage = "Unable to save those changes. Please contact CCIA staff to update.";
                return RedirectToAction(nameof(Index));
            }
            if(tag.TaggingOrg != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "You are not the company that requested that tag";
                return RedirectToAction(nameof(Index));
            }
            
            bool anyChanges = false;

            if(oecd.CountryId != vm.tag.OECDFile.CountryId)
            {   
                anyChanges = true;
                var countryChange = new OECDChanges();
                countryChange.OECDId = oecd.Id;
                countryChange.ColumnChange = "Country";
                countryChange.OldValue = oecd.CountryId.ToString();
                countryChange.NewValue = vm.tag.OECDFile.CountryId.ToString();
                countryChange.UserChange = "Client";
                countryChange.DateChanged = DateTime.Now;
                oecd.CountryId = vm.tag.OECDFile.CountryId;
                tag.OECDCountryId = vm.tag.OECDFile.CountryId;
                _dbContext.Add(countryChange);
            }
            if(oecd.Pounds != vm.tag.OECDFile.Pounds)
            {   
                anyChanges = true;
                var poundChange = new OECDChanges();
                poundChange.OECDId = oecd.Id;
                poundChange.ColumnChange = "LBS OECD";
                poundChange.OldValue = oecd.Pounds.ToString();
                poundChange.NewValue = vm.tag.OECDFile.Pounds.ToString();
                poundChange.UserChange = "Client";
                poundChange.DateChanged = DateTime.Now;
                oecd.Pounds = vm.tag.OECDFile.Pounds;
                _dbContext.Add(poundChange);
            }
            if(anyChanges)
            {
                await _dbContext.SaveChangesAsync();
                Message = "OECD values updated";
            }

            return RedirectToAction(nameof(Details), new { id = vm.tag.Id });
            
        }



        public async Task<IActionResult> GetTagFile(int id, int certYear)
        {
            var tagDoc = await _dbContext.TagDocuments.Where(d => d.Id == id).FirstOrDefaultAsync(); 
            var tag = await _dbContext.Tags.Where(t => t.Id == tagDoc.TagId).FirstOrDefaultAsync();
            if(tagDoc == null || tag == null)
            {
                ErrorMessage = "Document not found";
                return RedirectToAction(nameof(Index));
            }
            if(tag.TaggingOrg != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
             {
                ErrorMessage = "You are not the company that requested that tag";
                return RedirectToAction(nameof(Index));
            }
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
           if(tag.TaggingOrg != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
           {
                ErrorMessage = "You are not the company that requested that tag";
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
               Message = "File uploaded and associated to this tag request";
           }
           return RedirectToAction(nameof(Details), new { id = id }); 
        }
    }
}