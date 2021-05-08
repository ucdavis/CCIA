using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using CCIA.Services;
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

        public BlendsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
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
                RedirectToAction(nameof(Process));
           }
           return View(model);
       } 

       public async Task<IActionResult> EditLot(int id)
       {
           var comp = await _dbContext.LotBlends.Where(lb => lb.CompId == id).FirstOrDefaultAsync();
           if(comp == null)
           {
               ErrorMessage = "Component not found!";
               RedirectToAction(nameof(Process));
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
               RedirectToAction(nameof(Process));
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
               RedirectToAction(nameof(Process));
           }
           var blendId = compToDelete.BlendId;
            var lotDeleted = new BlendComponentChanges();
            lotDeleted.BlendId = compToDelete.BlendId;
            lotDeleted.ComponentId = compToDelete.CompId;
            lotDeleted.ColumnChange = "Deleted Component";
            lotDeleted.UserChange = User.FindFirstValue(ClaimTypes.Name);
            lotDeleted.DateChanged = DateTime.Now;
            lotDeleted.OldValue = "SID: " + compToDelete.Sid.ToString() + " Weight: " + compToDelete.Weight.ToString();
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
               RedirectToAction(nameof(Process));
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
               RedirectToAction(nameof(Process));
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
               RedirectToAction(nameof(Process));
           }
           var blendId = compToDelete.BlendId;
            var lotDeleted = new BlendComponentChanges();
            lotDeleted.BlendId = compToDelete.BlendId;
            lotDeleted.ComponentId = compToDelete.Id;
            lotDeleted.ColumnChange = "Deleted Component";
            lotDeleted.UserChange = User.FindFirstValue(ClaimTypes.Name);
            lotDeleted.DateChanged = DateTime.Now;
            lotDeleted.OldValue = "AppID: " + compToDelete.AppId.ToString() + " Weight: " + compToDelete.Weight.ToString() + " Applicant ID: " + compToDelete.ApplicantId.ToString() + " Crop ID: " + compToDelete.CropId.ToString() + " Variety: " + compToDelete.OfficialVarietyId.ToString() + " Class: " + compToDelete.Class.ToString();
            
            lotDeleted.NewValue = "";
            _dbContext.Add(lotDeleted);
           
           _dbContext.Remove(compToDelete);
           await _dbContext.SaveChangesAsync();

           Message = "Component deleted";

          return RedirectToAction(nameof(Details), new { id = blendId });  

       }
       
    }
}
