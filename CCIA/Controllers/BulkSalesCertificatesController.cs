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




namespace CCIA.Controllers
{
    public class BulkSalesCertificatesController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public BulkSalesCertificatesController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }


        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {

            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            if (certYear == 0)
            {
                certYear = await _dbContext.BulkSalesCertificates.Where(b => b.ConditionerOrganizationId == orgId).Select(b => b.Date.Year).MaxAsync();
            }
            var model = await BulkSalesCertificatesIndexViewModel.Create(_dbContext, orgId, certYear);
            return View(model);
        }

        // GET: Application/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Application/Create
        public async Task<ActionResult> Create()
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            var model = await BulkSalesCreateViewModel.Create(_dbContext, orgId);
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Application/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet]
        public async Task<JsonResult> GetAvailableClasses(string lookupType, int id)
        {
            if (lookupType == "Blend")
            {
                int? blendClass = await _dbContext.BlendRequests.Where(b => b.BlendId == id).Select(b => b.Class).FirstAsync();                
                if (blendClass.HasValue)
                {
                    int appType = await _dbContext.AbbrevClassSeeds.Where(a => a.Id == blendClass.Value).Select(a => a.Program).FirstAsync();
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.Id >= blendClass && a.Program == appType)
                        .OrderBy(a => a.Id)
                        .Select(a => new {value = a.Id, text = a.CertClass})
                        .ToListAsync();
                    return Json(model);
                }
                else
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.CertClass == "Certified" && a.Program == 1)
                        .OrderBy(a => a.Id)
                        .Select(c => new {value = c.Id, text = c.CertClass})
                        .ToListAsync();                   
                    return Json(model);
                }
            } 
            else
            {
                int? sidClass = await _dbContext.Seeds.Where(s => s.Id == id).Select(s => s.Class).FirstAsync();
                int appType = await _dbContext.Seeds.Where(s => s.Id == id).Include(s => s.AppTypeTrans).Select(s => s.AppTypeTrans.AppTypeId).FirstAsync();
                if(sidClass.HasValue)
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.Id >= sidClass && a.Program == appType)
                        .OrderBy(a => a.Id)
                        .Select(a => new  {value = a.Id, text = a.CertClass})
                        .ToListAsync();
                    return Json(model);
                }
                else
                {
                    var model = await _dbContext.AbbrevClassSeeds.Where(a => a.CertClass == "Certified")
                        .OrderBy(a => a.Id)
                        .Select(c => new  {value = c.Id, text = c.CertClass})
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
                var model = await _dbContext.BlendRequests.Where(b => b.BlendId == id)
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
                    id = b.BlendId,
                    saleType = "Blend",
                    program = b.BlendType,
                    applicant = b.ConditionerId + " " + b.Conditioner.OrgName,
                    conditioner = b.ConditionerId + " " + b.Conditioner.OrgName,
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
                    applicant = s.ApplicantId + " " + s.ApplicantOrganization.OrgName,
                    conditioner = s.ConditionerId + " " + s.ConditionerOrganization.OrgName,
                    crop = s.GetCropName(),
                    variety = s.GetVarietyName(),
                    cert = s.CertNumber(),
                    lot = s.LotNumber,
                }).SingleAsync();
                return Json(model);
            }

        }

        [HttpGet]
        public async Task<JsonResult> GetMyCustomerInfo(int id)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            var model = await _dbContext.MyCustomers.Where(m => m.Id == id && m.OrganizationId == orgId)
                .Include(m => m.State)
                .Include(m => m.Country)
                .SingleAsync();
            return Json(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetMyCustomerFromOrgId(int id)
        {
            var model = await _dbContext.Organizations.Where(o => o.OrgId == id)
                .Include(o => o.Address)
                .Select(o => new MyCustomers { 
                    Name = o.OrgName, 
                    Address1 = o.Address.Address1,
                    Address2 = o.Address.Address2,
                    City = o.Address.City,
                    StateId = o.Address.StateProvinceId,
                    CountryId = o.Address.CountryId,
                    Zip = o.Address.PostalCode,
                    Phone = o.Phone,
                    Email = o.Email
                    })
                .SingleAsync();
            
            return Json(model);
        }
    }
}