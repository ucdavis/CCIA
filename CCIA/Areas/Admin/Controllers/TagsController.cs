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

namespace CCIA.Controllers.Admin
{

    public class TagsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public TagsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        public ActionResult Index ()
        {
            return View();
        }

        public ActionResult Lookup()
        {
            return View();
        }

        public async Task<IActionResult> Edit(int id)
        {
            var model = await TagCreateEditViewModel.Create(_dbContext, _helper, id);         
            return View(model);
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
                .Where(t => t.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Tag ID not found!";
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> RecordSeries(int id, string Letter, int Start, int End, bool Void)
        {
            var test = await _dbContext.TagSeries.Include(s => s.Tag).Where(s => s.Letter == Letter && !s.Tag.Bulk && ((End >= s.Start && Start <= s.End) || (Start >= s.Start && End <= s.End))).AnyAsync();
            if(test)
            {
                ErrorMessage = "Tag Series already exists with that Letter & range";
                return RedirectToAction(nameof(Details), new {id = id});
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
        public async Task<IActionResult> AcceptTag(int id)
        {
            var tag = await _dbContext.Tags
                .Include(t => t.Seeds)
                .ThenInclude(s => s.StateOfOrigin)
                .Include(t => t.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Where(t => t.Id == id).FirstOrDefaultAsync();
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
                oecd.VarietyId = tag.Seeds.OfficialVarietyId;
                oecd.Pounds = Convert.ToInt32(tag.LotWeightRequested.Value);
                oecd.CertNumber = tag.Seeds.CertNumber;
                oecd.ClassId = tag.OECDTagType;
                oecd.CloseDate = tag.DateSealed.Value;
                oecd.ConditionerId = tag.Seeds.ConditionerId;
                oecd.CountryId = tag.OECDCountryId;
                oecd.LotNumber = tag.Seeds.LotNumber;
                oecd.ShipperId = tag.TaggingOrg;
                oecd.DateRequested = tag.DateEntered;
                oecd.NotCertified = tag.OECDTagType == 5;
                oecd.DataEntryDate = DateTime.Now;
                oecd.DataEntryUser = User.FindFirstValue(ClaimTypes.Name);
                oecd.DomesticOrigin = tag.Seeds.OriginCountry == 58;
                oecd.ReferenceNumber = tag.PlantingStockNumber;
                oecd.Canceled = false;
                oecd.TagsRequested = tag.CountRequested.Value;
                oecd.AdminComments = tag.AdminComments;
                oecd.OECDNumber = tag.Seeds.OriginCountry == 102 ? $"USA-CA-{tag.Seeds.CertNumber}" : $"USA-{tag.Seeds.StateOfOrigin.StateProvinceCode}/CA-{tag.Seeds.CertNumber}-{tag.Seeds.LotNumber}";
                oecd.TagId = tag.Id;

                _dbContext.Add(oecd);

            }

            await _dbContext.SaveChangesAsync();

            if(tag.OECD)
            {
                var msg = $"; OECD File {oecd.Id}";
                tag.AdminComments = $"{tag.AdminComments}{msg}";
                tag.OECDId = oecd.Id;
                tag.Seeds.Remarks = $"{tag.Seeds.Remarks}{msg}";
                await _dbContext.SaveChangesAsync(); 
                Message = "Tag Accepted & OECD File saved";               
            }

            return RedirectToAction(nameof(Details), new {id = id});            
        }
    }
}