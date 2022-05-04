using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
           var lotToUpdate = await _dbContext.LotBlends.Where(lb => blend.CompId == id).FirstOrDefaultAsync();
           if(lotToUpdate == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Process));
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
       
    }
}
