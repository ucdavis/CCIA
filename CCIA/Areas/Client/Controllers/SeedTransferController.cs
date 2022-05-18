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

        private readonly INotificationService _notificationService;

        public SeedTransferController(CCIAContext dbContext, IFullCallService helper,INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _notificationService = notificationService;
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

        [HttpPost]
        public async Task<IActionResult> SubmitSeedTransfer(SeedTransferRequestModel vm)
        {
            var submittedTransfer = vm.transfer;
            var transferToCreate = new SeedTransfers();
            var errors = SeedTransferValidator.CheckStandards(submittedTransfer);
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value); 

            if (errors.HasWarnings)
            {
                ModelState.AddModelError("transfer.StageFromFieldNumberOfAcres", errors.Error);
                var retryModel = await SeedTransferRequestModel.Retry(_dbContext, _helper, vm.request.Id, vm.request.Target, orgId, submittedTransfer); 
                return View("Create", retryModel);
            }

            if(vm.request.Target == "SID")
            {
                transferToCreate.SeedsID = vm.request.Id;
            }
            
            if(vm.request.Target == "AppId")
            {
                transferToCreate.ApplicationId = vm.request.Id;
            }
            
            if(vm.request.Target == "BID")
            {
                transferToCreate.BlendId = vm.request.Id;
            }



            if(vm.request.Target == "AppId" && submittedTransfer.StageFromField)
            {
                transferToCreate.OriginatingCountyId = await _dbContext.Applications.Where(a => a.Id == vm.request.Id).Select(a => a.FarmCounty).FirstOrDefaultAsync();
            } else {
                transferToCreate.OriginatingCountyId = await _dbContext.Organizations.Where(o => o.Id == orgId).Select(o => o.CountyId).FirstOrDefaultAsync();
            }

            if(transferToCreate.OriginatingCountyId == submittedTransfer.PurchaserCountyId)
            {
                transferToCreate.Type = SeedTransferTypes.IntraCounty.GetDisplayName();
            } else if(submittedTransfer.PurchaserStateId == 102) {
                transferToCreate.Type = SeedTransferTypes.InterCounty.GetDisplayName();
            } else {
                transferToCreate.Type = SeedTransferTypes.InterAgency.GetDisplayName();
            }

            transferToCreate.SubmittedForAnalysis = submittedTransfer.SubmittedForAnalysis ? true : false ;            
            transferToCreate.CreatedById =  int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            transferToCreate.CreatedOn = DateTime.Now;
            transferToCreate.TransferClassId = submittedTransfer.TransferClassId;
            transferToCreate.OriginatingOrganizationId = orgId;
            transferToCreate.CertificateDate = submittedTransfer.CertificateDate;           
            transferToCreate.Pounds = submittedTransfer.Pounds;
            transferToCreate.DestinationOrganizationId = vm.request.OrgId.HasValue ? vm.request.OrgId.Value : null;
            transferToCreate.PurchaserName = submittedTransfer.PurchaserName;
            transferToCreate.PurchaserAddressLine1 = submittedTransfer.PurchaserAddressLine1;
            transferToCreate.PurchaserAddressLine2 = submittedTransfer.PurchaserAddressLine2;
            transferToCreate.PurchaserCity = submittedTransfer.PurchaserCity;
            transferToCreate.PurchaserCountryId = submittedTransfer.PurchaserCountryId;
            transferToCreate.PurchaserEmail = submittedTransfer.PurchaserEmail;
            transferToCreate.PurchaserPhone = submittedTransfer.PurchaserPhone;
            transferToCreate.PurchaserStateId = submittedTransfer.PurchaserStateId;
            transferToCreate.PurchaserCountyId = submittedTransfer.PurchaserCountyId;
            transferToCreate.PurchaserZip = submittedTransfer.PurchaserZip;            
            transferToCreate.SeedstockLotNumbers = submittedTransfer.SeedstockLotNumbers;
            transferToCreate.StageInDirt = submittedTransfer.StageInDirt;
            transferToCreate.StageFromField = submittedTransfer.StageFromField;
            transferToCreate.StageFromStorage = submittedTransfer.StageFromStorage;
            transferToCreate.TypeRetail = submittedTransfer.TypeRetail;
            transferToCreate.TypeTote = submittedTransfer.TypeTote;
            transferToCreate.TypeBulk = submittedTransfer.TypeBulk;
            transferToCreate.StageConditioned = submittedTransfer.StageConditioned;
            transferToCreate.StageNotFinallyCertified = submittedTransfer.StageNotFinallyCertified;
            transferToCreate.NumberOfTrucks  = submittedTransfer.NumberOfTrucks;
            transferToCreate.StageCertifiedSeed = submittedTransfer.StageCertifiedSeed;
            transferToCreate.StageTreatment = submittedTransfer.StageTreatment;
            transferToCreate.StageTagging = submittedTransfer.StageTagging;
            transferToCreate.StageBagging = submittedTransfer.StageBagging;
            transferToCreate.StageBlending = submittedTransfer.StageBlending;
            transferToCreate.StageStorage = submittedTransfer.StageStorage;
            transferToCreate.StageOther = submittedTransfer.StageOther;
            transferToCreate.StageOtherValue = submittedTransfer.StageOtherValue;
            transferToCreate.StageFromFieldNumberOfAcres = submittedTransfer.StageFromFieldNumberOfAcres;
            transferToCreate.AdminUpdated = true;

            if(ModelState.IsValid){
                _dbContext.Add(transferToCreate);
                await _dbContext.SaveChangesAsync();
                await _notificationService.SeedTransferSubmitted(transferToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Seed Transfer Certificate Created";
            } else {
                ErrorMessage = "Something went wrong.";
                var retryModel = await SeedTransferRequestModel.Retry(_dbContext, _helper, vm.request.Id, vm.request.Target, orgId, submittedTransfer); 
                return View("Create", retryModel); 
            }

            return RedirectToAction(nameof(Details), new { id = transferToCreate.Id });  
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