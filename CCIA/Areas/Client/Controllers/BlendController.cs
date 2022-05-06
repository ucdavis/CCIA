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

namespace CCIA.Controllers.Client.Client
{
    public class BlendController : ClientController
    {
        private readonly CCIAContext _dbContext;

        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;

        public BlendController(CCIAContext dbContext, IFullCallService helper, IFileIOService fileService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileService;
        }

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {           
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.BlendRequests.Where(o => o.ConditionerId == orgId).MaxAsync(x => (int?)x.RequestStarted.Year);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }

            var model = await BlendIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);             
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {            
            var model = await AdminBlendsDetailsViewModel.Create(_dbContext, _helper, id);           
            if(model.blend == null)
            {
                ErrorMessage = "Blend Request not found";
                return RedirectToAction(nameof(Index));
            }
            if(model.blend.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "Blend Conditioner does not match your ID. Access denied.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

       public async Task<IActionResult> GetBlendFile(int id)
        {
            var blendDoc = await _dbContext.BlendDocuments.Where(a => a.Id == id).FirstOrDefaultAsync();
            var blend = await _dbContext.BlendRequests.Where(b => b.Id == blendDoc.BlendId).FirstOrDefaultAsync();
            if(blend == null || blendDoc == null)
            {
                ErrorMessage = "Blend document not found.";
                return RedirectToAction(nameof(Index));
            }
            if(blend.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "Blend Conditioner does not match your ID. Access denied.";
                return RedirectToAction(nameof(Index));
            }
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadBlendFile(blendDoc, blend.CertYear), contentType, blendDoc.Link);
        }


        [HttpPost]
        public async Task<IActionResult> UploadBlendDocument(int id, string docName, IFormFile file)
        {
           var blend = await _dbContext.BlendRequests.Where(a => a.Id == id).FirstOrDefaultAsync();
           if(blend == null)
            {
                ErrorMessage = "Blend document not found.";
                return RedirectToAction(nameof(Index));
            }
            if(blend.ConditionerId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "Blend Conditioner does not match your ID. Access denied.";
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

        
        [HttpGet]
        public async Task<JsonResult> FindBlend(string name)
        {
            var varieties = await _dbContext.VarFull
                .Where(v => EF.Functions.Like(v.Name, "%" + name + "%") && v.Blend)
                .Select(v => new VarFull
                {
                    CropId = v.CropId,                    
                    Id = v.Id,
                    Name = v.Name
                })
                .ToListAsync();
            return Json(varieties);
        }

        public async Task<IActionResult> StartNewInDirtBlend()
        {
           var comp = await AdminBlendsInDirtEditViewModel.Create(_dbContext, 0);                                         
           return View(comp);
        }

        [HttpPost]
        public async Task<IActionResult> StartNewInDirtBlend(AdminBlendsInDirtEditViewModel model)
        {
            bool error = false;
            var comp = model.comp;
            var app = new Applications();
            var applicant = new Organizations();
            var variety = new VarFull();
            var newComp = new BlendInDirtComponents();
            if(model.comp.Weight <= 0)
            {
                ErrorMessage = "Weight must be greater than 0";
                error = true;
            }
            if(model.origin == "Ca")
            {
                app = await _dbContext.Applications.Where(a => a.Id == model.comp.AppId).FirstOrDefaultAsync();
                if(app == null)
                {
                    ErrorMessage = "Application not found";
                    error = true;
                }
            }
            if(model.origin == "OOS")
            {
                applicant = await _dbContext.Organizations.Where(o => o.Id == comp.ApplicantId).FirstOrDefaultAsync();
                if(applicant == null)
                {
                    ErrorMessage = "Applicant not found";
                    error = true;
                }
                if(comp.CropId == null || comp.CropId == 0)
                {
                    ErrorMessage = "Please select a crop";
                    error = true;
                }
                variety = await _dbContext.VarFull.Where(v => v.Id == comp.OfficialVarietyId).FirstOrDefaultAsync();
                if(variety == null)
                {
                    ErrorMessage = "Variety not found";
                    error = true;
                }
                if(comp.CertYear == null || comp.CertYear < 2000 || comp.CertYear > 2100)
                {
                    ErrorMessage = "Cert year must be a valid 4 digit year";
                    error = true;
                }
                if(comp.CountryOfOrigin == null || comp.CountryOfOrigin == 0)
                {
                    ErrorMessage = "Country of Origin is required";
                    error = true;
                }
                if(comp.StateOfOrigin == null || comp.StateOfOrigin == 0)
                {
                    ErrorMessage = "State of Origin is required";
                    error = true;
                }
                if(string.IsNullOrWhiteSpace(comp.CertNumber))
                {
                    ErrorMessage = "Cert number is required";
                    error = true;
                }
                if(string.IsNullOrWhiteSpace(comp.LotNumber))
                {
                    ErrorMessage = "Lot number is required";
                    error = true;
                }
                if(comp.Class == null || comp.Class == 0)
                {
                    ErrorMessage = "Please select a class";
                    error = true;
                }
            }
            if(error)
            {
                var compRetry = await AdminBlendsInDirtEditViewModel.Create(_dbContext, 0);
                compRetry.comp = model.comp;
                return View(compRetry);
            }

            var newBlend = new BlendRequests();
            newBlend.BlendType = BlendType.InDirt.GetDisplayName();
            newBlend.RequestStarted = DateTime.Now;
            newBlend.ConditionerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            newBlend.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            newBlend.Status = BlendStatus.Initiated.GetDisplayName();

            _dbContext.Add(newBlend);
            await _dbContext.SaveChangesAsync();  

            if(model.origin == "Ca")
            {
                newComp.AppId = comp.AppId;
                newComp.Weight = comp.Weight;
            } 
            if(model.origin == "OOS")
            {
                newComp.ApplicantId = comp.ApplicantId;
                newComp.CropId = comp.CropId;
                newComp.OfficialVarietyId = comp.OfficialVarietyId;
                newComp.CertYear = comp.CertYear;
                newComp.CountryOfOrigin = comp.CountryOfOrigin;
                newComp.StateOfOrigin = comp.StateOfOrigin;
                newComp.CertNumber = comp.CertNumber;
                newComp.LotNumber = comp.LotNumber;
                newComp.Class = comp.Class;
            }

            newComp.BlendId = newBlend.Id;
            _dbContext.Add(newComp);
            await _dbContext.SaveChangesAsync();

            Message = "New In-Dirt Blend created and lot added.";

            return RedirectToAction(nameof(Details), new {id = newBlend.Id});
        }

       


        public ActionResult StartNewVarietalBlend()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartNewVarietalBlend(int varietyId, int sid, int weight)
        {
            var varietySearch = await _dbContext.VarietyBlendComponents.Where(v => v.BlendVarietyId == varietyId).ToListAsync();
            if(!varietySearch.Any())
            {
                ErrorMessage = "Variety not found. Please try again";
                return View();
            }
            var sidSearch = await _dbContext.Seeds.Where(s => s.Id == sid).FirstOrDefaultAsync();
            if(sidSearch == null)
            {
                ErrorMessage = "SID not found. Please check entered SID.";
                return View();
            }
            if(!varietySearch.Any(v => v.ComponentVarietyId == sidSearch.OfficialVarietyId))
            {
                ErrorMessage = "SID variety is not a component of the entered variety. Please double check values.";
                return View();
            }
            var newBlend = new BlendRequests();
            newBlend.BlendType = BlendType.Varietal.GetDisplayName();
            newBlend.RequestStarted = DateTime.Now;
            newBlend.ConditionerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            newBlend.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            newBlend.Status = BlendStatus.Initiated.GetDisplayName();
            newBlend.VarietyId = varietyId;

            _dbContext.Add(newBlend);
            await _dbContext.SaveChangesAsync();   

            var newLot = new LotBlends();

            newLot.BlendId = newBlend.Id;
            newLot.Sid = sid;
            newLot.Weight = weight;

            _dbContext.Add(newLot);
            await _dbContext.SaveChangesAsync();         

            Message = "New Varietal Blend created and lot added.";

            return RedirectToAction(nameof(Details), new {id = newBlend.Id});
        }

        public ActionResult StartNewLotBlend()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> StartNewLotBlend(int sid, int weight)
        {
            var seed = await _dbContext.Seeds.Where(s => s.Id == sid).Include(s => s.LabResults).FirstOrDefaultAsync();
            if(seed == null)
            {
                ErrorMessage = "SID not found";
                return RedirectToAction(nameof(StartNewLotBlend));
            }
            if(!seed.HasLabs)
            {
                ErrorMessage = "SID must have lab results to be the first lot added to blend.";
                return RedirectToAction(nameof(StartNewLotBlend));
            }
            var newBlend = new BlendRequests();
            newBlend.BlendType = BlendType.Lot.GetDisplayName();
            newBlend.RequestStarted = DateTime.Now;
            newBlend.ConditionerId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            newBlend.UserEntered = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            newBlend.Status = BlendStatus.Initiated.GetDisplayName();

            _dbContext.Add(newBlend);
            await _dbContext.SaveChangesAsync();

            var newLot = new LotBlends();

            newLot.BlendId = newBlend.Id;
            newLot.Sid = sid;
            newLot.Weight = weight;

            _dbContext.Add(newLot);
            await _dbContext.SaveChangesAsync();

            Message = "New Lot Blend created and lot added";

            return RedirectToAction(nameof(Details), new {id = newBlend.Id});

        }


        [HttpPost]
        public async Task<IActionResult> AddLot(int blendId, int sid, int weight)
        {
            var seed = await _dbContext.Seeds.Where(s => s.Id == sid).FirstOrDefaultAsync();
            if(seed == null)
            {
                ErrorMessage = "SID not found";
                return RedirectToAction(nameof(Details), new {id = blendId});
            }
            var blend = await _dbContext.BlendRequests.Where(b => b.Id == blendId).FirstOrDefaultAsync();
            if(blend == null)
            {
                ErrorMessage = "Blend not found";
                return RedirectToAction(nameof(Index));
            }

            if(blend.BlendType == BlendType.Lot.GetDisplayName())
            {
                var variety = await _dbContext.LotBlends.Where(l => l.BlendId == blendId)
                .Include(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .FirstOrDefaultAsync();
                if(seed.OfficialVarietyId != variety.Seeds.OfficialVarietyId)
                {
                    ErrorMessage = "New SID variety does not match other SID(s) in blend. All varieties must be same in Lot Blend.";
                    return RedirectToAction(nameof(Details), new {id = blendId});
                }
            } else {
                var varietySearch = await _dbContext.VarietyBlendComponents.Where(v => v.BlendVarietyId == blend.VarietyId).ToListAsync();                       
                if(!varietySearch.Any(v => v.ComponentVarietyId == seed.OfficialVarietyId))
                {
                    ErrorMessage = "SID variety is not a component of the entered variety. Please double check values.";
                    return RedirectToAction(nameof(Details), new {id = blendId});
                }
            }
            
            var newLot = new LotBlends();

            newLot.BlendId = blendId;
            newLot.Sid = sid;
            newLot.Weight = weight;

            _dbContext.Add(newLot);
            await _dbContext.SaveChangesAsync();

            Message = "New lot added to blend.";

            return RedirectToAction(nameof(Details), new {id = blendId});

        }

         public async Task<IActionResult> EditLot(int id)
       {
           var comp = await _dbContext.LotBlends.Where(lb => lb.CompId == id).FirstOrDefaultAsync();
           if(comp == null)
           {
               ErrorMessage = "Component not found!";
               return RedirectToAction(nameof(Index));
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
               return RedirectToAction(nameof(Index));
           }
           var blendToUpdate = await _dbContext.BlendRequests.Where(b => b.Id == lotToUpdate.BlendId).FirstOrDefaultAsync();
           if(blendToUpdate == null)
           {
               ErrorMessage = "Blend not found";
               return RedirectToAction(nameof(Index));
           }
           var seeds = await _dbContext.Seeds.Where(s => s.Id == blend.Sid).FirstOrDefaultAsync();
           if(seeds == null)
           {
               ErrorMessage = "SID not found";
               return RedirectToAction(nameof(EditLot), new { id = id }); 
           }

           if(blendToUpdate.BlendType == BlendType.Lot.GetDisplayName())
            {
                var variety = await _dbContext.LotBlends.Where(l => l.BlendId == blendToUpdate.Id)
                .Include(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .FirstOrDefaultAsync();
                if(seeds.OfficialVarietyId != variety.Seeds.OfficialVarietyId)
                {
                    ErrorMessage = "New SID variety does not match other SID(s) in blend. All varieties must be same in Lot Blend.";
                    return RedirectToAction(nameof(EditLot), new { id = id });
                }
            } else {
                var varietySearch = await _dbContext.VarietyBlendComponents.Where(v => v.BlendVarietyId == blendToUpdate.VarietyId).ToListAsync();                       
                if(!varietySearch.Any(v => v.ComponentVarietyId == seeds.OfficialVarietyId))
                {
                    ErrorMessage = "SID variety is not a component of the entered variety. Please double check values.";
                    return RedirectToAction(nameof(EditLot), new { id = id });
                }
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
               return RedirectToAction(nameof(Index));
           }
           var blendId = compToDelete.BlendId;
           _dbContext.Remove(compToDelete);
           await _dbContext.SaveChangesAsync();

           Message = "Component deleted";

          return RedirectToAction(nameof(Details), new { id = blendId });  

       }

    }
}