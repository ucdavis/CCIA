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
using CCIA.Models.BulkSalesCreateViewModel;
using CCIA.Models.CertificateViewModel;
using CCIA.Models.BulkSalesCertificateShareViewModel;




namespace CCIA.Controllers.Client
{
    public class BulkSalesCertificatesController : ClientController
    {
        // TODO Get shipping location from address w/ Facility
        private readonly CCIAContext _dbContext;

        public BulkSalesCertificatesController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {           
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.BulkSalesCertificates.Where(o => o.ConditionerOrganizationId == orgId).MaxAsync(x => (int?)x.Date.Year);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }
            
            var model = await BulkSalesCertificatesIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);
            return View(model);
        }

        // GET: Application/Create
        public async Task<ActionResult> Create()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await BulkSalesCreateViewModel.Create(_dbContext, orgId);
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(BulkSalesCreateViewModel model)
        {
            try
            {
               var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
                var newBulkSalesCertificate = new BulkSalesCertificates();
                newBulkSalesCertificate.ConditionerOrganizationId = orgId;
                newBulkSalesCertificate.Date = model.BulkSalesCertificate.Date;
                if (model.selectType == "SID")
                {
                    newBulkSalesCertificate.SeedsID = model.textId;
                    var seed = await _dbContext.Seeds.Where(s => s.Id == model.textId).SingleAsync();
                    newBulkSalesCertificate.CertProgramAbbreviation = seed.CertProgram;
                }
                else
                {
                    newBulkSalesCertificate.BlendId = model.textId;
                    newBulkSalesCertificate.CertProgramAbbreviation = "Blend";
                }
                newBulkSalesCertificate.PurchaserName = model.BulkSalesCertificate.PurchaserName;
                newBulkSalesCertificate.PurchaserAddressLine1 = model.BulkSalesCertificate.PurchaserAddressLine1;
                newBulkSalesCertificate.PurchaserAddressLine2 = model.BulkSalesCertificate.PurchaserAddressLine2;
                newBulkSalesCertificate.PurchaserCity = model.BulkSalesCertificate.PurchaserCity;
                newBulkSalesCertificate.PurchaserStateId = model.BulkSalesCertificate.PurchaserStateId;
                newBulkSalesCertificate.PurchaserCountryId = model.BulkSalesCertificate.PurchaserCountryId;
                newBulkSalesCertificate.PurchaserZip = model.BulkSalesCertificate.PurchaserZip;
                newBulkSalesCertificate.PurchaserPhone = model.BulkSalesCertificate.PurchaserPhone;
                newBulkSalesCertificate.PurchaserEmail = model.BulkSalesCertificate.PurchaserEmail;
                newBulkSalesCertificate.Pounds = model.BulkSalesCertificate.Pounds;
                newBulkSalesCertificate.ClassId = model.BulkSalesCertificate.ClassId;

               
                newBulkSalesCertificate.CreatedById =  int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                newBulkSalesCertificate.CreatedOn = DateTime.Now;


                if (ModelState.IsValid)
                {
                    _dbContext.Add(newBulkSalesCertificate);
                    await _dbContext.SaveChangesAsync();
                }
                else
                {
                    return View(model);
                }

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View(model);
            }
        }

        public async Task<ActionResult> Certificate(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await CertificateViewModel.Create(_dbContext, id, orgId);
            return View(model);
        }

        public async Task<ActionResult> Share(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await BulkSalesCertificateShareViewModel.Create(_dbContext, id, orgId);
            if (model.BulkSalesCertificate == null)
            {
                Message = "Bulk Sales Certificate not found, or does not belong to your Organization";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<ActionResult> AddShare(int id, int shareOrgId)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var bulkSalesCertificate = await _dbContext.BulkSalesCertificates.Where(b => b.Id == id && b.ConditionerOrganizationId == orgId).SingleOrDefaultAsync();
            if (bulkSalesCertificate == null)
            {
                ErrorMessage = "Bulk Sales Certificate could not be found.";
                return RedirectToAction(nameof(Index));
            }
            var newShare = new BulkSalesCertificatesShares();
            newShare.BulkSalesCertificatesId = bulkSalesCertificate.Id;
            newShare.ShareOrganizationId = shareOrgId;

            if (ModelState.IsValid)
            {
                _dbContext.Add(newShare);
                await _dbContext.SaveChangesAsync();
                Message = $"Bulk Sales Certificate shared with {shareOrgId}";
            }
            else
            {
                ErrorMessage = "Something bad happened";
            }

            var model = await BulkSalesCertificateShareViewModel.Create(_dbContext, id, orgId);
            return View("Share", model);
        }



        [HttpGet]
        public async Task<JsonResult> GetAvailableClasses(string lookupType, int id)
        {
            if (lookupType == "Blend")
            {
                int? blendClass = await _dbContext.BlendRequests.Where(b => b.Id == id).Select(b => b.Class).FirstAsync();
                if (blendClass.HasValue)
                {
                    int appType = await _dbContext.AbbrevClassSeeds.Where(a => a.Id == blendClass.Value).Select(a => a.Program).FirstAsync();
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.Id >= blendClass && a.Program == appType)
                        .OrderBy(a => a.Id)
                        .Select(a => new { value = a.Id, text = a.CertClass })
                        .ToListAsync();
                    return Json(model);
                }
                else
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.CertClass == "Certified" && a.Program == 1)
                        .OrderBy(a => a.Id)
                        .Select(c => new { value = c.Id, text = c.CertClass })
                        .ToListAsync();
                    return Json(model);
                }
            }
            else
            {
                int? sidClass = await _dbContext.Seeds.Where(s => s.Id == id).Select(s => s.Class).FirstAsync();
                int appType = await _dbContext.Seeds.Where(s => s.Id == id).Include(s => s.AppTypeTrans).Select(s => s.AppTypeTrans.AppTypeId).FirstAsync();
                if (sidClass.HasValue)
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.Id >= sidClass && a.Program == appType)
                        .OrderBy(a => a.Id)
                        .Select(a => new { value = a.Id, text = a.CertClass })
                        .ToListAsync();
                    return Json(model);
                }
                else
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.CertClass == "Certified")
                        .OrderBy(a => a.Id)
                        .Select(c => new { value = c.Id, text = c.CertClass })
                        .ToListAsync();
                    return Json(model);
                }
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetSidOrBlend(string lookupType, int id)
        {
            if (lookupType == "Blend")
            {
                var model = await _dbContext.BlendRequests.Where(b => b.Id == id)
                .Include(b => b.Conditioner)
                .Include(b => b.LotBlends)  // blendrequest (lot) => lotblend => seeds => variety => crop
                .ThenInclude(l => l.Seeds)
                .ThenInclude(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from knownh app) => indirt => application => variety
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Variety)
                .Include(b => b.InDirtBlends)  // blendrequest (in dirt from known app) => indirt => application => crop
                .ThenInclude(i => i.Application)
                .ThenInclude(a => a.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => crop
                .ThenInclude(i => i.Crop)
                .Include(b => b.InDirtBlends) // blendrequest (in dirt from oos app) => indirt => variety
                .ThenInclude(i => i.Variety)
                .Include(b => b.Variety) // blendrequest (varietal) => variety => crop
                .ThenInclude(v => v.Crop)
                .Select(b => new
                {
                    id = b.Id,
                    saleType = "Blend",
                    program = b.BlendType,
                    applicant = b.ConditionerId + " " + b.Conditioner.Name,
                    conditioner = b.ConditionerId + " " + b.Conditioner.Name,
                    crop = b.GetCrop(),
                    variety = b.GetVarietyName(),
                    cert = b.CertNumber,
                    lot = "",
                }).SingleAsync();
                return Json(model);
            }
            else
            {
                var model = await _dbContext.Seeds.Where(s => s.Id == id)
                .Include(s => s.ApplicantOrganization)
                .Include(s => s.ConditionerOrganization)
                .Include(s => s.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.Application)
                .ThenInclude(a => a.Variety)
                .ThenInclude(v => v.Crop)
                .Include(s => s.AppTypeTrans)
                .Select(s => new
                {
                    id = s.Id,
                    saleType = "SID",
                    program = s.AppTypeTrans.AppTypeTrans,
                    applicant = s.ApplicantId + " " + s.ApplicantOrganization.Name,
                    conditioner = s.ConditionerId + " " + s.ConditionerOrganization.Name,
                    crop = s.GetCropName(),
                    variety = s.GetVarietyName(),
                    cert = s.CertNumber,
                    lot = s.LotNumber,
                }).SingleAsync();
                return Json(model);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetMyCustomerInfo(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await _dbContext.MyCustomers.Where(m => m.Id == id && m.OrganizationId == orgId)
                .Include(m => m.State)
                .Include(m => m.Country)
                .SingleAsync();
            return Json(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetOrgs(string lookup)
        {
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(lookup, out id))
            {
                var orgs = await _dbContext.Organizations.Where(o => o.Id == id).Select(o => new { id = o.Id, name = o.Name }).ToListAsync();
                return Json(orgs);
            }
            else
            {
                var orgs = await _dbContext.Organizations.Where(o => o.Name.Contains(lookup.ToLower())).Select(o => new { id = o.Id, name = o.Name }).ToListAsync();
                return Json(orgs);
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetMyCustomerFromOrgId(int id)
        {
            var model = await _dbContext.Organizations.Where(o => o.Id == id)
                .Include(o => o.Address)
                .Select(o => new MyCustomers
                {
                    Name = o.Name,
                    Address1 = o.Address.Address1,
                    Address2 = o.Address.Address2,
                    City = o.Address.City,
                    StateId = o.Address.StateProvinceId.Value,
                    CountryId = o.Address.CountryId.Value,
                    Zip = o.Address.PostalCode,
                    Phone = o.Phone,
                    Email = o.Email
                })
                .SingleAsync();

            return Json(model);
        }
    }
}