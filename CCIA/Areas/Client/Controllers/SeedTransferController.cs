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
using Microsoft.Data.SqlClient;
using System.Net;

namespace CCIA.Controllers.Client
{
    public class SeedTransferController : ClientController
    {

        // TODO Get shipping location from address w/ Facility
        private readonly CCIAContext _dbContext;

        private readonly IFullCallService _helper;

        public SeedTransferController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        
        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {           
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value); 
            if (certYear == 0)
            {
                var maxCertYear =  await _dbContext.SeedTransfers.Where(a => a.OriginatingOrganizationId == orgId).MaxAsync(a => (int?)a.CertificateDate.Year);
                if(maxCertYear.HasValue)
                {
                    certYear = maxCertYear.Value;
                } else {
                    certYear = CertYearFinder.CertYear;
                }               
            }
            var model = await SeedTransferIndexViewModel.Create(_dbContext, orgId, certYear);          
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {           
             var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public ActionResult Initiate()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(int Id, string Target)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value); 
            var model = await SeedTransferRequestModel.Create(_dbContext, _helper, Id, Target, orgId);            
            return View(model);
        }

        

        public async Task<IActionResult> Certificate(int id)
        {
            var model = await _helper.FullSeedTransfers().Where(b => b.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Seed Transfer not found!";
                return RedirectToAction(nameof(Index));
            }
            return View("~/Areas/Admin/Views/SeedTransfers/Certificate.cshtml",model);            
        }

        public async Task<IActionResult> GetPS(int Id, string Target)
        {
            var p0 = new SqlParameter("@id_type", Target);
            var p1 = new SqlParameter("@id", Id);
            
            var p2 = new SqlParameter
            {
                ParameterName = "@ps",
                SqlDbType =  System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 8000,
            };
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_seed_transfer_retrieve_planting_stock @id_type, @id, @ps OUTPUT", p0, p1, p2);              
            return Ok(p2.Value.ToString());
        }

        public async Task<JsonResult> GetPurchaserFromCustomer(int Id)
        {
            var customer = await _dbContext.MyCustomers.Where(c => c.Id == Id).FirstOrDefaultAsync();
            if(customer == null)
            {
                return new JsonResult("Customer not found"){
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            return Json(customer);
        }

        public async Task<JsonResult> GetPurchaserFromOrganization(int id)
        {
            var org = await _dbContext.Organizations.Include(o => o.Address).Where(o => o.Id == id).FirstOrDefaultAsync();
            if(org == null)
            {
                return new JsonResult("Org not found"){
                    StatusCode = (int)HttpStatusCode.NotFound
                };
            }
            var customer = new MyCustomers{
                Id = org.Id,
                Name = org.Name,
                Address1 = org.Address.Address1,
                Address2 = org.Address.Address2,
                City = org.Address.City,
                CountyId = org.CountyId.HasValue ? org.CountyId : 0,
                StateId = org.Address.StateProvinceId.HasValue ? org.Address.StateProvinceId.Value : 0,
                CountryId = org.Address.CountryId.HasValue ?  org.Address.CountryId.Value : 0,
                Zip = org.Address.PostalCode,
                Phone = org.Phone,
                Email = org.Email,
            };
            return Json(customer);
        }
    }
}