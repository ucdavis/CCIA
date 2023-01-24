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
using CCIA.Models.SeedsCreateViewModel;

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
            var model = await TagIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value, _helper);            
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

        private async Task<bool> checkBulkPermission(int orgId)
        {
            return await _dbContext.CondStatus.Where(s => s.OrgId == orgId && s.Year == CertYearFinder.ConditionerYear && s.Status != "O" && s.PrintSeries).AnyAsync();
        }

        public async Task<IActionResult> InitiateBulk()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }      
            if(!(await checkBulkPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request bulk tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var model = await _dbContext.Crops.Where(c => c.CertifiedCrop).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> CreateBulk(int cropId, string variety)
        {
            if(!(await checkBulkPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request bulk tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await ClientTagRequestViewModel.CreateBulk(_dbContext, _helper, cropId, variety, orgId);
            if(model.variety == null || model.crop == null)
            {
                ErrorMessage = "Crop/variety not found";
                return RedirectToAction(nameof(InitiateBulk));
            }
            return View(model);
        }

        public async Task<IActionResult> SubmitBulk(int id, ClientTagRequestViewModel model)
        {
            if(!(await checkBulkPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request bulk tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }

            var newTag = new Tags();
            var submittedTag = model.request;
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);            
           
           
            newTag.TagClass = 4;
            newTag.DateRequested = DateTime.Now;
            newTag.DateNeeded = submittedTag.DateNeeded;
            newTag.SeriesRequest = true;
            newTag.WeightUnit = "0";
            newTag.CountRequested = submittedTag.CountRequested;
            newTag.ExtrasOverrun = 0;            
            newTag.TagType = submittedTag.TagType;
            newTag.Comments = submittedTag.Comments;
            newTag.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            // TODO These are the same column. Get rid of one?
            newTag.Contact = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            // TODO DateEntered & DateRequested are duplicates. Get rid of one
            newTag.DateEntered = DateTime.Now;
            newTag.TaggingOrg = orgId;
            newTag.Bulk = true;
            newTag.HowDeliver = submittedTag.HowDeliver;
            newTag.Stage = TagStages.Requested.GetDisplayName();
            newTag.Alias = submittedTag.Alias;
            newTag.BulkCropId = model.crop.CropId;
            newTag.BulkVarietyId = model.variety.Id;
            newTag.OECD = false;
            newTag.BagSize = null;
            newTag.DateSealed = null;
            newTag.OECDCountryId = null;

            ModelState.Clear();    
            if(submittedTag.DateNeeded < DateTime.Now.AddDays(1))       
            {
                ModelState.AddModelError("request.DateNeeded","Date needed must be greater than 2 days out");
            }
            if (TryValidateModel(newTag))
            {   
                _dbContext.Add(newTag);                
                await _dbContext.SaveChangesAsync();  
                await _notificationService.TagSubmitted(newTag);
                await _dbContext.SaveChangesAsync();             
                Message = "Tag submitted!";
                return RedirectToAction("Details", new { id = newTag.Id });
            }
            
            var retryModel = await ClientTagRequestViewModel.CreateBulkRetry(_dbContext, _helper, submittedTag.Id, submittedTag.Target, orgId, submittedTag);
            return View("CreateBulk", retryModel);

        }

        public async Task<IActionResult> Initiate()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }      
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
            if (TryValidateModel(newTag))
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

        public async Task<IActionResult> SelectOrigin()
        {     
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }       
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SelectOrigin(string origin)
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            if(origin == "Ca")
            {
                return RedirectToAction("SelectApp");
            }
            if(origin == "OOS")
            {
                return RedirectToAction("NewOOSSeedLot");
            }
            return View();
        }

        public async Task<ActionResult> NewOOSSeedLot()
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var model = await ClientTagRequestViewModel.CreateOOSGrayTag(_dbContext);
            return View(model);
        }

         [HttpPost]
        public async Task<IActionResult> NewOOSSeedLot(ClientTagRequestViewModel model)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var seed = model.OOSSeed;  
            var tag = model.request;
            seed.LotNumber = seed.LotNumber.Trim();
            seed.SampleFormCertNumber = seed.SampleFormCertNumber.Trim();   
           
            bool error = false;                        
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.SampleFormCertNumber == seed.SampleFormCertNumber))
            {
                ErrorMessage = "SID with same Lot, Cert Year, and Cert Number found. Duplicates are not allowed.";
                error = true;              
            }        
            if(error)
            {
                var returnErrorModel = await ClientTagRequestViewModel.CreateOOSGrayTagRetry(_dbContext, model);
                return View(returnErrorModel); 
            }  
            
            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.SampleFormCertNumber = seed.SampleFormCertNumber;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = seed.ApplicantId;
            newSeed.ConditionerId = orgId;
            newSeed.SampleFormVarietyId = seed.SampleFormVarietyId;
            newSeed.OfficialVarietyId = seed.SampleFormVarietyId.HasValue ? seed.SampleFormVarietyId.Value : 0;
            newSeed.ConditionerId = orgId;
            newSeed.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);           
            newSeed.LotNumber = seed.LotNumber;
            newSeed.PoundsLot = seed.PoundsLot;
            newSeed.Class = 6;
            newSeed.CountyDrawn = 0;
            newSeed.OriginState = seed.OriginState;
            newSeed.OriginCountry = seed.OriginCountry;
            newSeed.OriginalRun = seed.Type == "Original Run" ? true : false;
            newSeed.Remill = seed.Type == "Remill" ? true : false;            
            newSeed.Remarks = tag.Comments;
            newSeed.SampleDrawnBy = "NFC";
            newSeed.OECDLot = true;
            newSeed.Confirmed = false;
            newSeed.CertProgram = "SD";
            newSeed.Status = SeedsStatus.PendingAcceptance.GetDisplayName();
            newSeed.NotFinallyCertified = true;

            ModelState.Clear();                
            if (TryValidateModel(newSeed))            
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                var newTag = new Tags();
                newTag.SeedsID = newSeed.Id;
                newTag.LotWeightBagged = seed.PoundsLot;
                newTag.CountRequested = tag.CountRequested;
                newTag.BagSize = tag.BagSize;
                newTag.WeightUnit = tag.WeightUnit;
                newTag.TagClass = seed.Class;
                newTag.TagType = 7;
                newTag.DateNeeded = tag.DateNeeded;
                newTag.HowDeliver = tag.HowDeliver;
                newTag.Comments = tag.Comments;
                newTag.DateRequested = DateTime.Now;
                newTag.Contact = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                newTag.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                newTag.DateEntered = DateTime.Now;
                newTag.TaggingOrg = orgId;
                newTag.Stage = TagStages.Requested.GetDisplayName();
                newTag.OECD = true;
                newTag.PlantingStockNumber = tag.PlantingStockLotNumber;
                newTag.OECDTagType = 5;
                newTag.DateSealed = tag.DateSealed;
                newTag.OECDCountryId = tag.OECDCountryId;
                newTag.Alias = tag.Alias;
                newTag.CoatingPercent = tag.CoatingPercent;

                _dbContext.Add(newTag);
                await _dbContext.SaveChangesAsync();

                var adminComments = $"NFC request: SID={newSeed.Id} Tag ID={newTag.Id}";

                newSeed.Remarks = string.IsNullOrWhiteSpace(seed.Remarks) ? adminComments : $"{seed.Remarks}; {adminComments}";
                newTag.AdminComments = adminComments;
                await _notificationService.NFCSubmitted(newTag);

                await _dbContext.SaveChangesAsync();                
                Message = $"NFC Seed/Tag request submitted for review. SID: {newSeed.Id}, TagID: {newTag.Id}";
                return RedirectToAction(nameof(Index));
            }

            ErrorMessage = "Error encountered saving seed lot";
            var returnModel = await ClientTagRequestViewModel.CreateOOSGrayTagRetry(_dbContext, model);
            return View(returnModel);                
        
        }

        public async Task<IActionResult> SelectApp()
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            int[] years = CertYearFinder.certYearListReverse.ToArray();
            return View(years);
        }

        [HttpPost]
        public async Task<ActionResult> CreateInState(int[] appId, int certYear, int certNum, int certRad)
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            if(appId == null || appId.Count() == 0)
            {
                ErrorMessage = "No apps selected";
                return RedirectToAction(nameof(SelectApp));

            }
            var model = await ClientTagRequestViewModel.CreateGrayTag(_dbContext, appId, certYear, certNum, certRad);
           
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> SubmitInState(ClientTagRequestViewModel model)
        {
            if(!(await checkTagPermission(int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))))
            {
                ErrorMessage = "You do not have current permission to request tags. Please contact CCIA staff to correct.";
                return RedirectToAction(nameof(Index));
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var seed = model.Seed;
            var tag = model.request;
            
            bool error = false;
            
            
            
            if(await _dbContext.Seeds.AnyAsync(s => s.LotNumber == seed.LotNumber && s.CertYear == seed.CertYear && s.SampleFormCertNumber == seed.SampleFormCertNumber.ToString() && s.SampleFormRad == seed.SampleFormRad))
            {
                ErrorMessage = "SID with same Lot, Cert Year, Cert Number, and Rad found. Duplicates are not allowed.";
                error = true;                
            }   

            if(error)
            {
                var returnModel = await ClientTagRequestViewModel.CreateGrayTagRetry(_dbContext, model);
                return View("CreateInState", returnModel);
            }

             
            
            var app = await _dbContext.Applications.Where(a => a.Id == seed.AppId.First())
                .Include(a => a.Variety)
                .FirstAsync();
           
            var newSeed = new Seeds();
            newSeed.SampleFormDate = DateTime.Now;
            newSeed.SampleFormCertNumber = seed.SampleFormCertNumber.ToString();
            newSeed.SampleFormRad = seed.SampleFormRad;
            newSeed.CertYear = seed.CertYear;
            newSeed.ApplicantId = app.ApplicantId;
            newSeed.ConditionerId = orgId;
            newSeed.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            newSeed.SampleFormVarietyId = app.SelectedVarietyId;
            newSeed.OfficialVarietyId = app.Variety.ParentId;
            newSeed.LotNumber = seed.LotNumber;
            newSeed.PoundsLot = seed.PoundsLot;
            newSeed.Class = app.ClassProducedId;
            newSeed.CountyDrawn = seed.CountyDrawn;
            newSeed.OriginState = 102;
            newSeed.OriginCountry = 58;
            if(seed.Type == "Original Run")
            {
                newSeed.OriginalRun = true;
            } else
            {
                newSeed.OriginalRun = false;
            }
            if(seed.Type == "Remill")
            {
                newSeed.Remill = true;
            } else
            {
                newSeed.Remill = false;
            }
            newSeed.Remarks = seed.Remarks;
            newSeed.SampleDrawnBy = seed.SampleDrawnBy + " - " + seed.SamplerName;
            newSeed.SamplerID = seed.SamplerId;
            newSeed.OECDLot = true;
            newSeed.Confirmed = false;
            newSeed.Status = SeedsStatus.PendingAcceptance.GetDisplayName();
            newSeed.CertProgram = app.AppType;
            newSeed.NotFinallyCertified = true;
            
            
            if(ModelState.IsValid)
            {
                await _dbContext.Seeds.AddAsync(newSeed);
                await _dbContext.SaveChangesAsync();

                var seedapps = new List<SeedsApplications>();
                foreach(var sa in seed.AppId)
                {
                    seedapps.Add(new SeedsApplications { AppId = sa});
                }
                newSeed.SeedsApplications = seedapps;

                var newTag = new Tags();
                newTag.SeedsID = newSeed.Id;
                newTag.LotWeightBagged = seed.PoundsLot;
                newTag.CountRequested = tag.CountRequested;
                newTag.BagSize = tag.BagSize;
                newTag.WeightUnit = tag.WeightUnit;
                newTag.TagClass = seed.Class;
                newTag.TagType = 7;
                newTag.DateNeeded = tag.DateNeeded;
                newTag.HowDeliver = tag.HowDeliver;
                newTag.Comments = tag.Comments;
                newTag.DateRequested = DateTime.Now;
                newTag.Contact = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                newTag.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                newTag.DateEntered = DateTime.Now;
                newTag.TaggingOrg = orgId;
                newTag.Stage = TagStages.Requested.GetDisplayName();
                newTag.OECD = true;
                newTag.PlantingStockNumber = tag.PlantingStockLotNumber;
                newTag.OECDTagType = 5;
                newTag.DateSealed = tag.DateSealed;
                newTag.OECDCountryId = tag.OECDCountryId;
                newTag.Alias = tag.Alias;
                newTag.CoatingPercent = tag.CoatingPercent;

                _dbContext.Add(newTag);
                await _dbContext.SaveChangesAsync();

                var adminComments = $"NFC request: SID={newSeed.Id} Tag ID={newTag.Id}";

                newSeed.Remarks = string.IsNullOrWhiteSpace(seed.Remarks) ? adminComments : $"{seed.Remarks}; {adminComments}";
                newTag.AdminComments = adminComments;
                await _notificationService.NFCSubmitted(newTag);

                await _dbContext.SaveChangesAsync();                
                Message = $"NFC Seed/Tag request submitted for review. SID: {newSeed.Id}, TagID: {newTag.Id}";
                return RedirectToAction(nameof(Index));
            }
            
                ErrorMessage = "Error encountered saving seed lot or tag";
                var retryModel =  await ClientTagRequestViewModel.CreateGrayTagRetry(_dbContext, model);
                return View("CreateInState", retryModel);
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

        [HttpPost]
        public async Task<IActionResult> RecordSeries(int id, string Letter, int Start, int End, bool Void)
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

    }
}