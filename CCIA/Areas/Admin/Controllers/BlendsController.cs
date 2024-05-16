using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Models.SampleLabResultsViewModel;
using CCIA.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using NetTopologySuite.Geometries;
using Newtonsoft.Json;

namespace CCIA.Controllers.Admin
{
   
    public class BlendsController : AdminController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;
        private readonly INotificationService _notificationService;

        public BlendsController(CCIAContext dbContext, IFullCallService helper, IFileIOService fileIOService, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileIOService;
            _notificationService = notificationService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Lookup()
        {
            return View();
        }   

        public async Task<IActionResult> ToggleFollowup(int id)
        {
            var blendToToggle = await _dbContext.BlendRequests.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(blendToToggle == null)
            {
                ErrorMessage = "Blend not found";
                return RedirectToAction(nameof(Index));
            }
            blendToToggle.FollowUp = !blendToToggle.FollowUp;
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new {id = id});
        }    

        public async Task<IActionResult> ByStatus()
        {
            var model = await AdminBlendsSearchViewModel.ByStatus(_dbContext, null, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> ByStatus(AdminBlendsSearchViewModel vm)
        {
            var model = await AdminBlendsSearchViewModel.ByStatus(_dbContext, vm, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Sublot(int id, int pounds, string number )
        {
            if(pounds == 0 || string.IsNullOrWhiteSpace(number))
            {
                ErrorMessage = "Pounds and Sublot Number must be provided";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if(await _dbContext.BlendRequests.Where(b => b.ParentId == id && b.SublotNumber == number).AnyAsync())
            {
                ErrorMessage = "Sublot Number must be unique for the parent ID";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            var blendToSublot = await _dbContext.BlendRequests.Include(b => b.LotBlends).ThenInclude(l => l.Seeds).Where(b => b.Id == id).FirstOrDefaultAsync();
            if(blendToSublot == null)
            {
                ErrorMessage = "Blend not found!";
                return RedirectToAction(nameof(Index));
            }
            if(blendToSublot.BlendType != "Lot")
            {
                ErrorMessage = "Blend must be a lot blend to sublot!";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if (blendToSublot.Sublot)
            {
                ErrorMessage = "Cannot sublot a sublot";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            if (blendToSublot.LotBlends == null || blendToSublot.LotBlends.First().Seeds == null)
            {
                ErrorMessage = "SID not found on first component; can't sublot";
                return RedirectToAction(nameof(Details), new { id = id });
            }

            var sublot = new BlendRequests()
            {
                BlendType = "Lot",
                RequestStarted = blendToSublot.RequestStarted,
                Sublot = true,
                ParentId = blendToSublot.Id,
                ConditionerId = blendToSublot.ConditionerId,
                UserEntered = blendToSublot.UserEntered,
                LbsLot = pounds,
                SublotNumber = number,
                Status = BlendStatus.Approved.GetDisplayName(),
                VarietyId = blendToSublot.LotBlends.First().Seeds.OfficialVarietyId,
                Comments = $"Sublot of BID {blendToSublot.Id}",
                DateSubmitted = DateTime.Now,
                Submitted = true,
                Approved = true,
                ApproveDate = DateTime.Now,
                ApprovedBy = User.FindFirstValue(ClaimTypes.Name)
            };

            _dbContext.Add(sublot);
            await _dbContext.SaveChangesAsync();
            Message = "Sublot created";
            return RedirectToAction(nameof(Details), new { id = sublot.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var p0 = new SqlParameter("@id", System.Data.SqlDbType.Int);
            p0.Value = id;
            var check = await _dbContext.SeedCancelChecks.FromSqlRaw($"EXEC mvc_blend_cancel_check @id", p0).ToListAsync();
            if (check.Any())
            {
                var sbCheck = new StringBuilder();
                foreach (var row in check)
                {
                    sbCheck.Append(row.Id + " " + row.Source + " || ");
                }
                ErrorMessage = "That Blend has existing tags, OECD, Blends, Bulk Sales, or Seed Transfers on file: " + sbCheck.ToString();
                return RedirectToAction(nameof(Details), new { id = id });
            }
            var blend = await _dbContext.BlendRequests.Where(a => a.Id == id).FirstOrDefaultAsync();
            if (blend == null)
            {
                ErrorMessage = "Blend not found";
                return RedirectToAction(nameof(Index));
            }
            blend.Status = BlendStatus.Cancelled.GetDisplayName();
            blend.Comments = blend.Comments + $"; cancelled by CCIA {DateTime.Now}";
            await _notificationService.BlendCancelled(blend);
            await _dbContext.SaveChangesAsync();
            Message = "Blend cancelled.";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        public async Task<IActionResult> GetBlendFile(int id)
        {
            var blendDoc = await _dbContext.BlendDocuments.Where(a => a.Id == id).FirstOrDefaultAsync();
            var certYear = await _dbContext.BlendRequests.Where(b => b.Id == blendDoc.BlendId).Select(b => b.CertYear).FirstOrDefaultAsync();
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadBlendFile(blendDoc, certYear), contentType, blendDoc.Link);
        }


        [HttpPost]
        public async Task<IActionResult> UploadBlendDocument(int id, string docName, IFormFile file)
        {
           docName = docName?.Trim();
            if (string.IsNullOrWhiteSpace(docName))
            {
                ErrorMessage = "Must provide a document name for this file!";
                return RedirectToAction(nameof(Details), new { id = id });
            }
            var blend = await _dbContext.BlendRequests.Where(a => a.Id == id).FirstOrDefaultAsync();
           if(blend == null)
           {
               ErrorMessage = "Blend not found";
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
               await _fileService.SaveBlendFile(blend, file);               
                var doc = new BlendDocuments();
                doc.BlendId = blend.Id;
                doc.Name = docName;
                doc.Link = file.FileName;
                _dbContext.Add(doc);
                await _dbContext.SaveChangesAsync();
                Message = "Document saved";
           }
           return RedirectToAction(nameof(Details), new { id = id }); 
        }


        public async Task<IActionResult> Search()
        {
            var model = await AdminBlendsSearchViewModel.Create(_dbContext, null, _helper);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Search(AdminBlendsSearchViewModel vm)
        {
            vm.conditionerSearchTerm = vm.conditionerSearchTerm?.Trim();
            var model = await AdminBlendsSearchViewModel.Create(_dbContext, vm, _helper);
            return View(model);
        }


       public async Task<IActionResult> Process()
       {
           var model = await _helper.FullBlendRequest().Where(b => b.Status == BlendStatus.PendingAcceptance.GetDisplayName()).ToListAsync();
           return View(model);
       }  

       public async Task<IActionResult> Details(int id)
       {
           var model = await AdminBlendsDetailsViewModel.Create(_dbContext, _helper, id);           
           if(model.blend == null)
           {
               ErrorMessage = "Blend Request not found";
                return RedirectToAction(nameof(Process));
           }
           return View(model);
       }

		[HttpPost]
		public async Task<IActionResult> ReturnBlend(int id, string reason)
		{
			var blendToReturn = await _dbContext.BlendRequests.Where(b => b.Id == id).FirstOrDefaultAsync();
			if (blendToReturn == null)
			{
				ErrorMessage = "Blend not found";
				return RedirectToAction(nameof(Index));
			}
			if (string.IsNullOrWhiteSpace(reason))
			{
				ErrorMessage = "Return reason cannot be blank";
				return RedirectToAction(nameof(Details), new { id = blendToReturn.Id });
			}
			var reasonWithDate = DateTime.Now.ToShortDateString() + ": " + reason;
			blendToReturn.Status = BlendStatus.ReturnedToClient.GetDisplayName();
			blendToReturn.ReturnReason = string.IsNullOrWhiteSpace(blendToReturn.ReturnReason) ? reasonWithDate : blendToReturn.ReturnReason + "; " + reasonWithDate;
			await _notificationService.BlendReturnedForReview(blendToReturn);
			await _dbContext.SaveChangesAsync();
			Message = "Blend returned to client.";
			return RedirectToAction(nameof(Details), new { id = blendToReturn.Id });
		}

		public async Task<IActionResult> Previous(int id)
        {
            var previousId = await _dbContext.BlendRequests.Where(x => x.Id < id).OrderBy(x => x.Id).Select(x => x.Id).LastOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

        public async Task<IActionResult> Next(int id)
        {
            var previousId = await _dbContext.BlendRequests.Where(x => x.Id > id).OrderBy(x => x.Id).Select(x => x.Id).FirstOrDefaultAsync();
            return RedirectToAction(nameof(Details), new {id = previousId});
        }

       public async Task<IActionResult> EditVariety(int id)
       {
           var model = await _dbContext.BlendRequests.Where(b => b.Id == id).FirstOrDefaultAsync();
           if(model.BlendType != "Varietal")
           {
               ErrorMessage = "Can not change variety on In-Dirt or Lot blends";
               return RedirectToAction(nameof(Details), new {id = id});
           }
           return View(model);
       }

       public async Task<IActionResult> LookupVariety (string lookup) 
        {  
            lookup = lookup?.Trim();          
            var varieties = await _dbContext.VarFull.Where(v => (v.Name.Contains(lookup) || EF.Functions.Like(v.Id.ToString(), "%" + lookup + "%")) && v.Blend).ToListAsync();
            return PartialView("_LookupVariety", varieties);
        }

        [HttpPost]
        public async Task<IActionResult> EditVariety(int id, int VarietyId)
        {
            var blend = await _dbContext.BlendRequests.Where(b => b.Id == id).FirstOrDefaultAsync();
            blend.VarietyId = VarietyId;
            await _dbContext.SaveChangesAsync();
            Message = "Variety updated";
            return RedirectToAction(nameof(Details), new {id = id});
        }

       public IActionResult NewLot(int id)
       {           
           var model = new LotBlends();
           model.BlendId = id;
           return View(model);
       }
       
       [HttpPost]
       public async Task<IActionResult> NewLot(int id, LotBlends lot)
       {
           var blend = await _dbContext.BlendRequests.Where(b => b.Id == id).AnyAsync();
           if(lot.BlendId != id || !blend)
           {
               ErrorMessage = "Something went wrong!";
               return RedirectToAction(nameof(Process));
           }
           if(lot.Weight == 0 || lot.Weight < 0.1M)
           {
                ErrorMessage = "Component weight cannot be zero or less than 0.1 ";
                return View(lot);
           }
           var newLot = new LotBlends();
           newLot.BlendId = id;
           newLot.Sid = lot.Sid;
           newLot.Weight = lot.Weight;

           var change = new BlendComponentChanges();
           change.BlendId = id;
           change.ComponentId = newLot.CompId;
            change.ColumnChange = "Component Added";
            change.UserChange = User.FindFirstValue(ClaimTypes.Name);
            change.DateChanged = DateTime.Now;
            change.OldValue = "";
            change.NewValue = $"SID: {lot.Sid} Weight: {lot.Weight}";
            

           if(ModelState.IsValid){
                _dbContext.Add(newLot);  
                _dbContext.Add(change);              
                await _dbContext.SaveChangesAsync();
                Message = "Component added";
            } else {
                ErrorMessage = "Something went wrong";                         
                return View(lot);
            }
            return RedirectToAction(nameof(Details), new {id = id});
       } 
       public async Task<IActionResult> NewDirtLot(int id)
       {  
           var comp = await AdminBlendsInDirtEditViewModel.Create(_dbContext, 0);          
            comp.comp.BlendId = id;                     
           return View(comp);
       }
       
       [HttpPost]
       public async Task<IActionResult> NewDirtLot(int id, AdminBlendsInDirtEditViewModel vm)
       {
           var blend = await _dbContext.BlendRequests.Where(b => b.Id == id).AnyAsync();
           var lot = vm.comp;
           if(lot.BlendId != id || !blend)
           {
               ErrorMessage = "Something went wrong!";
               return RedirectToAction(nameof(Process));
           }
           if(lot.Weight == 0 || lot.Weight < 0.1M)
           {
                ErrorMessage = "Component weight cannot be zero or less than 0.1 ";
                return View(lot);
           }
           var newLot = new BlendInDirtComponents();
           newLot.BlendId = id;

           var change = new BlendComponentChanges();
           change.BlendId = id;
           change.ComponentId = newLot.Id;
            change.ColumnChange = "Component Added";
            change.UserChange = User.FindFirstValue(ClaimTypes.Name);
            change.DateChanged = DateTime.Now;
            change.OldValue = "";  

           if(lot.AppId == null)
           {
               newLot.ApplicantId = lot.ApplicantId;
               newLot.CropId = lot.CropId;
               newLot.OfficialVarietyId = lot.OfficialVarietyId;
               newLot.CertYear = lot.CertYear;
               newLot.CountryOfOrigin = lot.CountryOfOrigin;
               newLot.StateOfOrigin = lot.StateOfOrigin;
               newLot.CertNumber = lot.CertNumber;
               newLot.LotNumber = lot.LotNumber;
               newLot.Class = lot.Class;
               change.NewValue = $"Applicant Id: {lot.ApplicantId} Weight: {lot.Weight} Crop Id: {lot.CropId} Var Id: {lot.OfficialVarietyId} Cert Year: {lot.CertYear} Country Id: {lot.CountryOfOrigin} State id: {lot.StateOfOrigin} Cert#: {lot.CertYear} Lot#: {lot.CertYear} Class ID: {lot.Class}";

           } else
           {
               newLot.AppId = lot.AppId;
               change.NewValue  = $"AppId: {lot.AppId} Weight: {lot.Weight}";
           }
           
           newLot.Weight = lot.Weight;

           if(ModelState.IsValid){
                _dbContext.Add(newLot);  
                _dbContext.Add(change);              
                await _dbContext.SaveChangesAsync();
                Message = "Component added";
            } else {
                ErrorMessage = "Something went wrong";                         
                return View(lot);
            }
            return RedirectToAction(nameof(Details), new {id = id});
       } 

       public async Task<IActionResult> Accept(int id)
       {
           var blend = await _dbContext.BlendRequests.Where(b => b.Id == id).FirstOrDefaultAsync();
           if(blend == null)
           {
               ErrorMessage = "Blend not found!!";
               return RedirectToAction(nameof(Process));
           }
           blend.Approved = true;
           blend.Status = BlendStatus.Approved.GetDisplayName();
           blend.ApproveDate = DateTime.Now;
           blend.ApprovedBy = User.FindFirstValue(ClaimTypes.Name);

           await _notificationService.BlendRequestApproved(blend);

           await _dbContext.SaveChangesAsync();
           Message = "Blend Approved";
           return RedirectToAction(nameof(Details), new {id = blend.Id});

       }

       public async Task<IActionResult> EditLot(int id)
       {
           var comp = await _dbContext.LotBlends.Where(lb => lb.CompId == id).FirstOrDefaultAsync();
           if(comp == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           return View(comp);
       } 
       
       [HttpPost]      
       public async Task<IActionResult> EditLot(int id, LotBlends blend)
       {
           var lotToUpdate = await _dbContext.LotBlends.Where(lb => lb.CompId == id).FirstOrDefaultAsync();
           if(lotToUpdate == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           if(lotToUpdate.Weight == 0 || lotToUpdate.Weight < 0.1M)
           {
                ErrorMessage = "Component weight cannot be zero or less than 0.1 ";
                return View(lotToUpdate);
           }

           if(lotToUpdate.Sid != blend.Sid)
           {
               var sidUpdate = new BlendComponentChanges();
               sidUpdate.BlendId = lotToUpdate.BlendId;
               sidUpdate.ComponentId = lotToUpdate.CompId;
               sidUpdate.ColumnChange = "SID";
               sidUpdate.UserChange = User.FindFirstValue(ClaimTypes.Name);
               sidUpdate.DateChanged = DateTime.Now;
               sidUpdate.OldValue = lotToUpdate.Sid.ToString();
               sidUpdate.NewValue = blend.Sid.ToString();
               _dbContext.Add(sidUpdate);
           }
           if(lotToUpdate.Weight != blend.Weight)
           {
               var weightUpdate = new BlendComponentChanges();
               weightUpdate.BlendId = lotToUpdate.BlendId;
               weightUpdate.ComponentId = lotToUpdate.CompId;
               weightUpdate.ColumnChange = "Weight";
               weightUpdate.UserChange = User.FindFirstValue(ClaimTypes.Name);
               weightUpdate.DateChanged = DateTime.Now;
               weightUpdate.OldValue = lotToUpdate.Weight.ToString();
               weightUpdate.NewValue = blend.Weight.ToString();
               _dbContext.Add(weightUpdate);
           }
           lotToUpdate.Sid = blend.Sid;
           lotToUpdate.Weight = blend.Weight;

            if(ModelState.IsValid){                
                await _dbContext.SaveChangesAsync();
                Message = "Component updated";
            } else {
                ErrorMessage = "Something went wrong";                         
                return View(lotToUpdate);
            }

            return RedirectToAction(nameof(Details), new { id = lotToUpdate.BlendId }); 

       }

       [HttpPost]
       public async Task<IActionResult> DeleteLot(int id)
       {
           var compToDelete = await _dbContext.LotBlends.Where(lb => lb.CompId == id).FirstOrDefaultAsync();           
           if(compToDelete == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           var blendId = compToDelete.BlendId;
            var lotDeleted = new BlendComponentChanges();
            lotDeleted.BlendId = compToDelete.BlendId;
            lotDeleted.ComponentId = compToDelete.CompId;
            lotDeleted.ColumnChange = "Deleted Component";
            lotDeleted.UserChange = User.FindFirstValue(ClaimTypes.Name);
            lotDeleted.DateChanged = DateTime.Now;
            lotDeleted.OldValue = $"SID: {compToDelete.Sid} Weight: {compToDelete.Weight}";
            lotDeleted.NewValue = "";
            _dbContext.Add(lotDeleted);
           
           _dbContext.Remove(compToDelete);
           await _dbContext.SaveChangesAsync();

           Message = "Component deleted";

          return RedirectToAction(nameof(Details), new { id = blendId });  

       }

        public async Task<IActionResult> EditDirtLot(int id)
       {
           var comp = await AdminBlendsInDirtEditViewModel.Create(_dbContext, id);          
           
           if(comp.comp == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           return View(comp);
       }  

       [HttpPost]      
       public async Task<IActionResult> EditDirtLot(int id, BlendInDirtComponents comp)
       {
           var compToUpdate = await _dbContext.BlendInDirtComponents.Where(b => b.Id == id).FirstOrDefaultAsync();
           if(compToUpdate == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           if(compToUpdate.Weight == 0 || compToUpdate.Weight < 0.1M)
           {
                ErrorMessage = "Component weight cannot be zero or less than 0.1 ";
                return View(compToUpdate);
           }
           
           compToUpdate.AppId = comp.AppId;
           compToUpdate.Weight = comp.Weight;
           if((compToUpdate.CropId == null & comp.CropId != 0) || (compToUpdate.CropId != null && comp.CropId == 0))
           {
               compToUpdate.CropId = comp.CropId;
           }
           
           compToUpdate.ApplicantId = comp.ApplicantId;
           compToUpdate.OfficialVarietyId = comp.OfficialVarietyId;
           compToUpdate.CertYear = comp.CertYear;
           if((compToUpdate.CountryOfOrigin == null & comp.CountryOfOrigin != 0) || (compToUpdate.CountryOfOrigin != null && comp.CountryOfOrigin == 0))
           {
               compToUpdate.CountryOfOrigin = comp.CountryOfOrigin;
           }
           if((compToUpdate.StateOfOrigin == null & comp.StateOfOrigin != 0) || (compToUpdate.StateOfOrigin != null && comp.StateOfOrigin == 0))
           {
               compToUpdate.StateOfOrigin = comp.StateOfOrigin;
           }
           compToUpdate.CertNumber = comp.CertNumber;
           compToUpdate.LotNumber = comp.LotNumber;
           if((compToUpdate.Class == null & comp.Class != 0) || (compToUpdate.Class != null && comp.Class == 0))
           {
               compToUpdate.Class = comp.Class;
           }

            if(ModelState.IsValid){                
                await _dbContext.SaveChangesAsync();
                Message = "Component updated";
            } else {
                ErrorMessage = "Something went wrong";                         
                return View(compToUpdate);
            }
            return RedirectToAction(nameof(Details), new { id = compToUpdate.BlendId }); 

       }

       [HttpPost]
       public async Task<IActionResult> DeleteDirtLot(int id)
       {
           var compToDelete = await _dbContext.BlendInDirtComponents.Where(b => b.Id == id).FirstOrDefaultAsync();           
           if(compToDelete == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
           }
           var blendId = compToDelete.BlendId;
            var lotDeleted = new BlendComponentChanges();
            lotDeleted.BlendId = compToDelete.BlendId;
            lotDeleted.ComponentId = compToDelete.Id;
            lotDeleted.ColumnChange = "Deleted Component";
            lotDeleted.UserChange = User.FindFirstValue(ClaimTypes.Name);
            lotDeleted.DateChanged = DateTime.Now;
            lotDeleted.OldValue = $"AppID: {compToDelete.AppId} Weight: {compToDelete.Weight} Applicant ID: {compToDelete.ApplicantId} Crop ID: {compToDelete.CropId} Variety: {compToDelete.OfficialVarietyId} Class: {compToDelete.Class}";
            
            lotDeleted.NewValue = "";
            _dbContext.Add(lotDeleted);
           
           _dbContext.Remove(compToDelete);
           await _dbContext.SaveChangesAsync();

           Message = "Component deleted";

          return RedirectToAction(nameof(Details), new { id = blendId });  

       }

        
        public async Task<IActionResult> EditLabs(int Id)
        {
            var labs = await _dbContext.BlendLabResults.Where(b => b.BlendId == Id).FirstOrDefaultAsync();
            if (labs == null)
            {
                BlendLabResults newlab = new BlendLabResults
                {
                    BlendId = Id
                };
                _dbContext.Add(newlab);
                await _dbContext.SaveChangesAsync();
            }
            var model = await SampleLabResultsViewModel.CreateBlend(_dbContext, Id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditLabs(int id, SampleLabResultsViewModel results)
        {
            var labs = results.BlendLabs;
            labs.PurityPercent = labs.PurityPercent / 100;
            labs.InertPercent = labs.InertPercent / 100;
            labs.OtherCropPercent = labs.OtherCropPercent / 100;
            labs.OtherVarietyPercent = labs.OtherVarietyPercent / 100;
            labs.WeedSeedPercent = labs.WeedSeedPercent / 100;
            labs.GermPercent = labs.GermPercent / 100;
            labs.HardSeedPercent = labs.HardSeedPercent / 100;
          

            var errorList = await LabResultsCheckStandards.CheckStandardsFromBlendLabs(_dbContext, labs);

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

                var errorModel = await SampleLabResultsViewModel.ReUseBlend(_dbContext, labs);
                return View(errorModel);
            }

            // Passed verification, save and redirect
            var labsToUpdate = await _dbContext.BlendLabResults.Where(l => l.BlendId == results.BlendLabs.BlendId).FirstOrDefaultAsync();
            if (errorList.AssayNeeded)
            {
                labsToUpdate.AssayResults = labs.AssayResults;
                labsToUpdate.AssayTest = labs.AssayTest;
            }
            else
            {
                labsToUpdate.AssayResults = "N";
            }
           
            if (!labsToUpdate.CciaConfirmed && labs.CciaConfirmed)
            {
                labsToUpdate.ConfirmDate = DateTime.Now;
                labsToUpdate.ConfirmUser = User.FindFirstValue(ClaimTypes.Name);
            }
            labsToUpdate.CciaConfirmed = labs.CciaConfirmed;

           
            labsToUpdate.Comments = labs.Comments;
            labsToUpdate.PrivateLabDate = labs.PrivateLabDate;
            labsToUpdate.DodderGrams = labs.DodderGrams;
           
            labsToUpdate.GermPercent = labs.GermPercent;
            labsToUpdate.GermResults = labs.GermResults;
            labsToUpdate.HardSeedPercent = labs.HardSeedPercent;
           
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
            labsToUpdate.PrivateLabDate = labs.PrivateLabDate;
            labsToUpdate.PrivateLabId = labs.PrivateLabId;
            labsToUpdate.PrivateLabNumber = labs.PrivateLabNumber;
            labsToUpdate.PurityComments = labs.PurityComments;
            labsToUpdate.PurityGrams = labs.PurityGrams;
            labsToUpdate.PurityPercent = labs.PurityPercent;
            labsToUpdate.PurityResults = labs.PurityResults;
            
            labsToUpdate.WeedSeedComments = labs.WeedSeedComments;
            labsToUpdate.WeedSeedCount = labs.WeedSeedCount;
            labsToUpdate.WeedSeedPercent = labs.WeedSeedPercent;
            labsToUpdate.LastUpdateAdmin = true;
            labsToUpdate.UpdateAdminId = User.FindFirstValue(ClaimTypes.Name);



            if (ModelState.IsValid)
            {
                await _dbContext.SaveChangesAsync();
                Message = "Lab Results Updated";
                return RedirectToAction("Details", new { id = labs.BlendId });
            }
            else
            {
                ErrorMessage = "Something went wrong";
            }

            var model = await SampleLabResultsViewModel.ReUseBlend(_dbContext, results.BlendLabs);
            return View(model);
        }

    }
}
