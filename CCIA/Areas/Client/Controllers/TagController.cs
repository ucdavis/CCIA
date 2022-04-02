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

        public ActionResult Initiate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, string TagTarget)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await ClientTagRequestViewModel.Create(_dbContext, _helper, id, TagTarget, orgId);
            return View(model);
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