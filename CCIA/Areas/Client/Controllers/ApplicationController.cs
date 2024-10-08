﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using CCIA.Services;
using System.IO;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using System.Text;
using static Humanizer.On;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

using CCIA.Models.ApplicationViewModels;


namespace CCIA.Controllers.Client
{

    [Authorize(Roles = "AllowApps")]
    public class ApplicationController : ClientController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly IFileIOService _fileService;
        private readonly INotificationService _notificationService;
        private readonly IConfiguration _config;

        public ApplicationController(CCIAContext dbContext, IFullCallService helper,IFileIOService fileIOService,  INotificationService notificationService, IConfiguration config)
        {
            _dbContext = dbContext;
            _helper = helper;
            _fileService = fileIOService;
            _notificationService = notificationService;
            _config = config;

        }

        // TODO Add file upload/download. Both need security checks to make sure they are only uploading/downloading to their own apps
        // TODO Same for FIR, seeds, blends, tags

        // GET: Application
        public async Task<IActionResult> Index(int certYear)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);          
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.Applications.Where(a => a.ApplicantId == orgId).MaxAsync(x => (int?)x.CertYear);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }
            var model = await ApplicationIndexViewModel.Create(_dbContext, orgId, certYearToUse.Value);
            return View(model);
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await _dbContext.Applications.Where(a => a.Id == id && a.ApplicantId == orgId)
                .Include(a => a.GrowerOrganization)
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.County)
                .Include(a => a.Crop)
                .Include(a => a.Variety)
                .Include(a => a.ClassProduced)
                .Include(a => a.AppTypeTrans)
                .Include(a => a.Certificates)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.PsClassNavigation)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedStateProvince)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.GrownCountry)
                .Include(a => a.PlantingStocks).ThenInclude(p => p.TaggedCountry)
                .Include(a => a.FieldHistories).ThenInclude(fh => fh.FHCrops)
                .Include(a => a.Subspecies)
                .Include(a => a.Sites)
                .ThenInclude(s => s.County)
                .Include(a => a.Ecoregion)
                .Include(a => a.NSG0StateProvince)
                .Include(a => a.FieldInspectionReport)
                .FirstOrDefaultAsync();

            if(model == null)
            {
                ErrorMessage = "That app was either not found or does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }    
            return View(model);
        }

        public async Task<IActionResult> Replant(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }            
            var replantModel = await ApplicationViewModel.CreateEditModel(_dbContext, id);  
            if(replantModel.Application.AppType != AppTypes.Seed.GetDisplayName() || replantModel.Application.CertYear != CertYearFinder.CertYear - 1 || replantModel.Application.CropId != 63)
            {
                ErrorMessage = "App is not a seed app, rice app, or from last cert year";
                return Redirect(nameof(Index));
            }
            replantModel.Replant = true;
            replantModel.ReplantId = id;
            replantModel.Application.CertYear = CertYearFinder.CertYear;
            return View("CreateApplication",replantModel);
        }

        public async Task<IActionResult> Reuse(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }            
            var reuseModel = await ApplicationViewModel.CreateEditModel(_dbContext, id); 
            reuseModel.Application.CertYear = CertYearFinder.CertYear;  
            if(reuseModel.Application.PlantingStocks.Any())
            {
                reuseModel.Application.PlantingStocks.First().PsCertNum = null;
                reuseModel.Application.PlantingStocks.First().PoundsPlanted = null;
                reuseModel.Application.PlantingStocks.First().SeedPurchasedFrom = null;
            }
            
            reuseModel.Application.FieldName = null;
            reuseModel.Application.DatePlanted = null;
            reuseModel.Application.FarmCounty = 0;
            reuseModel.Application.AcresApplied = null;
            reuseModel.Application.Comments = null;


            return View("CreateApplication",reuseModel);

        }

        public async Task<IActionResult> CreateApplication(int growerId, int appTypeId)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }
            if(appTypeId == 10)
            {
                return RedirectToAction(nameof(HempInfo), new { growerId = growerId, appTypeId = appTypeId});
            }
            var model = await ApplicationViewModel.CreateGeneric(_dbContext, growerId, appTypeId, orgId, null, null, null, null, contactId);
            return View(model);
        }

        public async Task<IActionResult> CreateHempApplication(int growerId, int appTypeId, string whatPlanted, string whatProduced, string producingSeedType, string whereProduction)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }
            if(growerId == 0)
            {
                return RedirectToAction(nameof(GrowerLookup), new { appTypeID = 10});
            }
            if(string.IsNullOrWhiteSpace(whatPlanted) || string.IsNullOrWhiteSpace(whatProduced) || string.IsNullOrWhiteSpace(whereProduction))
            {
                return RedirectToAction(nameof(HempInfo), new { growerId = growerId, appTypeId = appTypeId});
            }
            var model = await ApplicationViewModel.CreateGeneric(_dbContext, growerId, appTypeId, orgId, whatPlanted, whatProduced, producingSeedType, whereProduction, contactId);
            return View(nameof(CreateApplication), model);
        }

        
        
        [HttpPost]
        public async Task<IActionResult> CreateApplication(ApplicationViewModel model)
        {           
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contact = await _dbContext.Contacts.Where(c => c.Id == contactId).FirstOrDefaultAsync();
            contact.LastApplicationAgreementYear = CertYearFinder.CertYear;
            var newPS2 = new PlantingStocks();
            var site = new NativeSeedSites();
            List<FieldHistory> newFieldHistories = new List<FieldHistory>();
            var newFieldHistory = new FieldHistory();
            var newApp = new Applications();
            var submittedApp = model.Application;
            newApp.AppType = submittedApp.AppType;
            newApp.CertYear = submittedApp.CertYear;
            newApp.OriginalCertYear = submittedApp.CertYear;
            newApp.UserDataentry = contactId;
            newApp.EnteredVariety = submittedApp.EnteredVariety;
            newApp.Received = DateTime.Now;
            newApp.Status = ApplicationStatus.PendingSupportingMaterial.GetDisplayName();
            newApp.ApplicantComments = submittedApp.ApplicantComments;
            if (model.ReplantId != 0 && model.ReplantId != orgId)
            {
                newApp.ApplicantNotes = "Replant on AppId " + model.ReplantId.ToString() + "; ";
            }
            newApp.FieldName = submittedApp.FieldName;
            newApp.DatePlanted = submittedApp.DatePlanted;
            newApp.AcresApplied = submittedApp.AcresApplied;
            newApp.ApplicantComments = submittedApp.ApplicantComments;
            newApp.CropId = submittedApp.CropId;
            newApp.ApplicantId = orgId;
            newApp.GrowerId = submittedApp.GrowerId;
            newApp.FarmCounty = submittedApp.FarmCounty;
            newApp.SelectedVarietyId = submittedApp.SelectedVarietyId;

            var newPS1 = TransferPlantingStockFromSubmission(model.PlantingStock1);
            if (submittedApp.ClassProducedId != 80)
            {                
                if (submittedApp.AppType == "LT")
                {
                    newApp.ClassProducedId = 78;
                    newPS1.PsClass = 77;
                    newPS1.PsCertNum = "LT";
                    model.PlantingStock1.PsCertNum = "LT";
                }
                else
                {
                    newApp.ClassProducedId = submittedApp.ClassProducedId;
                }
                if (submittedApp.EnteredVariety == model.PlantingStock1.PsEnteredVariety)
                {
                    newPS1.OfficialVarietyId = submittedApp.SelectedVarietyId;
                }
                if (submittedApp.AppType == "PO")
                {
                    newPS1.OfficialVarietyId = int.Parse(submittedApp.EnteredVariety);
                    newApp.SelectedVarietyId = int.Parse(submittedApp.EnteredVariety);
                }

                if (model.PlantingStock2 != null && !string.IsNullOrWhiteSpace(model.PlantingStock2.PsCertNum))
                {
                    newPS2 = TransferPlantingStockFromSubmission(model.PlantingStock2);
                }
                else
                {
                    if (model.PlantingStock2 != null)
                    {
                        model.PlantingStock2.PsCertNum = "0";
                    }
                }
                if ((newPS1.PsClass >= submittedApp.ClassProducedId && submittedApp.ClassProducedAccession == null) || (newPS1.PsAccession >= submittedApp.ClassProducedAccession))
                {
                    newApp.WarningFlag = true;
                    if(newApp.ApplicantNotes == null)
                    {
                        newApp.ApplicantNotes = "Class produced is less than or equal to class planted; ";
                    } else if (!newApp.ApplicantNotes.Contains("Class produced is less than or equal to class planted"))
                    {
                        newApp.ApplicantNotes += "Class produced is less than or equal to class planted; ";
                    }
                }
            } else
            {
                newApp.ClassProducedId = submittedApp.ClassProducedId;
                newPS1.PsCertNum = "0";
                model.PlantingStock1.PsCertNum = "0";
                model.PlantingStock2.PsCertNum = "0";
            }

            if(FieldHistoryExists(model.FieldHistory1))
            {
                newFieldHistory = TransferFieldHistoryFromSubmission(model.FieldHistory1);
                newFieldHistories.Add(newFieldHistory);
            }  
            if(FieldHistoryExists(model.FieldHistory2))
            {
                newFieldHistory = TransferFieldHistoryFromSubmission(model.FieldHistory2);
                newFieldHistories.Add(newFieldHistory);
            }  
            if(FieldHistoryExists(model.FieldHistory3))
            {
                newFieldHistory = TransferFieldHistoryFromSubmission(model.FieldHistory3);
                newFieldHistories.Add(newFieldHistory);
            }  
            if(FieldHistoryExists(model.FieldHistory4))
            {
                newFieldHistory = TransferFieldHistoryFromSubmission(model.FieldHistory4);
                newFieldHistories.Add(newFieldHistory);
            }  
            if(FieldHistoryExists(model.FieldHistory5))
            {
                newFieldHistory = TransferFieldHistoryFromSubmission(model.FieldHistory5);
                newFieldHistories.Add(newFieldHistory);
            }  
           
            if(submittedApp.AppType == "HP")
            {
                newApp.CountyPermit = submittedApp.CountyPermit;
                if(model.WhereProduction == "Indoors")
                {
                    newApp.IsSquareFeet = true;
                }
                newApp.HempWhatPlanted = model.WhatPlanted;
                newApp.HempWhatProduced = model.WhatProduced;
                newApp.HempWhereProduced = model.WhereProduction;
            }
            if(submittedApp.AppType == "PO")
            {               
                newApp.PoLotNum = submittedApp.PoLotNum;
            }
            if(submittedApp.AppType == "GQ")
            {
                newApp.ClassProducedAccession = submittedApp.ClassProducedAccession;
            }
            
            if(submittedApp.AppType == "PV")
            {
                newApp.PvgSource = submittedApp.PvgSource;
                newApp.PvgSelectionId = submittedApp.PvgSelectionId;
                newApp.EcoregionId = submittedApp.EcoregionId;
                newApp.FieldElevation = submittedApp.FieldElevation;
                newApp.SubspeciesId = submittedApp.SubspeciesId;
            }
            if(submittedApp.AppType == "NS")
            {
                newApp.PvgSelectionId = submittedApp.PvgSelectionId;
                newApp.SubspeciesId = submittedApp.SubspeciesId;
                newApp.EcoregionId = submittedApp.EcoregionId;
            }
            if (submittedApp.AppType == "NS" && newApp.ClassProducedId != 80)
            {
                newApp.PvgSource = submittedApp.PvgSource;                
                newApp.FieldElevation = submittedApp.FieldElevation;                
            }
            if(newApp.AppType == "NS" && newApp.ClassProducedId == 80)
            {
                var site1 = model.Site1;
                site.SiteName = site1.SiteName;
                newApp.FieldName = site1.SiteName;
                site.HarvestDate = site1.HarvestDate;
                site.Lat = site1.Lat;
                site.Long = site1.Long;
                site.Comments = site1.Comments;
                site.CollectionAreaSize = site1.CollectionAreaSize;
                site.FieldElevation = site1.FieldElevation;
                site.SiteCounty = site1.SiteCounty;
                newApp.AcresApplied = site1.CollectionAreaSize;
                newApp.FarmCounty = site1.SiteCounty;
                model.Application.AcresApplied = site1.CollectionAreaSize;
                model.Application.FieldName = site1.SiteName;
                model.Application.DatePlanted = site1.HarvestDate;
                newApp.NSG0StateProvinceIdCollected = submittedApp.NSG0StateProvinceIdCollected;
                newApp.G0Ownership = submittedApp.G0Ownership;
                newApp.DatePlanted = site1.HarvestDate;
            }

            if (model.Replant)
            {
                var replantApp = await _dbContext.Applications.Where(a => a.Id == model.ReplantId).FirstOrDefaultAsync();
                newApp.Maps = replantApp.Maps;
                newApp.MapsSubmissionDate = replantApp.MapsSubmissionDate;
                newApp.MapCenterLat = replantApp.MapCenterLat;
                newApp.MapCenterLong = replantApp.MapCenterLong;
                newApp.MapVe = replantApp.MapVe;
                newApp.TextField  = replantApp.TextField;
                newApp.GeoTextField = replantApp.GeoTextField;
                newApp.GeoField = replantApp.GeoField;
            }

            ModelState.Clear();    
            if(submittedApp.FarmCounty == 0 && newApp.ClassProducedId != 80)       
            {
                ModelState.AddModelError("Application.FarmCounty","Must select a Farm county");
            }
            if (TryValidateModel(model))
            {   
                _dbContext.Add(newApp);                
                await _dbContext.SaveChangesAsync();

                if(model.PlantingStock2 != null && model.PlantingStock2.PsCertNum != "0")
                {
                    newPS2.AppId = newApp.Id;
                    _dbContext.Add(newPS2);
                }  
                if(newApp.AppType == "NS" && newApp.ClassProducedId == 80)
                {
                    site.AppId = newApp.Id;
                    _dbContext.Add(site);
                } else
                {
                    newPS1.AppId = newApp.Id;
                    _dbContext.Add(newPS1);  
                }
                foreach(FieldHistory history in newFieldHistories)
                {
                    history.AppId = newApp.Id;
                    _dbContext.Add(history);
                }           

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = newApp.Id });
            }
            
            var retryModel = await ApplicationViewModel.CreateRetryModel(_dbContext, model,  orgId, contactId);   
            return View(retryModel);
        }

        [HttpPost]
        public async Task<IActionResult> UpdatePOPoundsHarvested(int id, int PoundsHarvested)
        {
			var appToUpdate = await _dbContext.Applications.Include(a => a.FieldInspectionReport)
				.Where(a => a.Id == id).FirstOrDefaultAsync();
			if (appToUpdate == null)
			{
				ErrorMessage = "Application not found!";
				return RedirectToAction(nameof(Index));
			}
			if (appToUpdate.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
			{
				ErrorMessage = "That app does not belong to your organization.";
				return RedirectToAction(nameof(Index));
			}
            if(appToUpdate.Status != ApplicationStatus.FieldInspectionReportReady.GetDisplayName() || appToUpdate.AppType != "PO")
            {
				ErrorMessage = "That app is either not a Potato App or not 'Field Inspection Report Ready'.";
				return RedirectToAction(nameof(Index));
			}
            appToUpdate.FieldInspectionReport.PotatoPoundsHarvested = PoundsHarvested;

			if (TryValidateModel(appToUpdate))
			{
				await _dbContext.SaveChangesAsync();

				Message = "Pounds Harvested Updated!";
				return RedirectToAction("Details", new { id = appToUpdate.Id });
			}

            ErrorMessage = "Something went wrong";
			return RedirectToAction("Details", new { id = appToUpdate.Id });
		}


		public async Task<IActionResult> Edit(int id)
        {
            var retryModel = await ApplicationViewModel.CreateEditModel(_dbContext, id);   
            return View(retryModel);            
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, ApplicationViewModel model)
        {             
            var appToUpdate = await _dbContext.Applications
                .Include(a => a.PlantingStocks)
                .Where(a => a.Id == id).FirstOrDefaultAsync();            
            if(appToUpdate == null)
            {
                ErrorMessage = "Application not found!";
                return  RedirectToAction(nameof(Index));
            }   
            if(appToUpdate.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            var submittedApp = model.Application;
            
            appToUpdate.GrowerId = submittedApp.GrowerId;
            appToUpdate.CertYear = submittedApp.CertYear;
            appToUpdate.OriginalCertYear = submittedApp.CertYear;
            appToUpdate.UserAppModifed = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            appToUpdate.UserAppModDt = DateTime.Now;
            appToUpdate.EnteredVariety = submittedApp.EnteredVariety;                        
            appToUpdate.ApplicantComments = submittedApp.ApplicantComments;
            appToUpdate.CropId = submittedApp.CropId; 
            appToUpdate.SelectedVarietyId = submittedApp.SelectedVarietyId;

            if (submittedApp.AppType == "NS")            {                
                
                appToUpdate.EcoregionId = submittedApp.EcoregionId;                
                appToUpdate.SubspeciesId = submittedApp.SubspeciesId;
            }

            if (appToUpdate.ClassProducedId == 80)
            {                                   
                appToUpdate.G0Ownership = submittedApp.G0Ownership;
                appToUpdate.NSG0StateProvinceIdCollected = submittedApp.NSG0StateProvinceIdCollected;
                model.Application.FieldName = appToUpdate.FieldName;
                model.Application.DatePlanted = appToUpdate.DatePlanted;
                model.Application.AcresApplied = appToUpdate.AcresApplied;
                model.Application.FarmCounty = appToUpdate.FarmCounty;
            } else
            {
                if (submittedApp.AppType == "NS")
                {
                    appToUpdate.PvgSource = submittedApp.PvgSource;
                    appToUpdate.FieldElevation = submittedApp.FieldElevation;
                }

                appToUpdate.FieldName = submittedApp.FieldName;
                appToUpdate.DatePlanted = submittedApp.DatePlanted;
                appToUpdate.AcresApplied = submittedApp.AcresApplied;
                appToUpdate.FarmCounty = submittedApp.FarmCounty;
                var PSToUpdate = appToUpdate.PlantingStocks.First();
                PSToUpdate.PsCertNum = model.PlantingStock1.PsCertNum;
                PSToUpdate.PsEnteredVariety = model.PlantingStock1.PsEnteredVariety;
                PSToUpdate.PoundsPlanted = model.PlantingStock1.PoundsPlanted;
                PSToUpdate.PsClass = model.PlantingStock1.PsClass;
                PSToUpdate.PsAccession = model.PlantingStock1.PsAccession;
                PSToUpdate.StateCountryGrown = model.PlantingStock1.StateCountryGrown;
                PSToUpdate.StateCountryTagIssued = model.PlantingStock1.StateCountryTagIssued;
                PSToUpdate.SeedPurchasedFrom = model.PlantingStock1.SeedPurchasedFrom;
                PSToUpdate.WinterTest = model.PlantingStock1.WinterTest;
                PSToUpdate.PvxTest = model.PlantingStock1.PvxTest;
                PSToUpdate.ThcPercent = model.PlantingStock1.ThcPercent;

                if (submittedApp.EnteredVariety == model.PlantingStock1.PsEnteredVariety)
                {
                    PSToUpdate.OfficialVarietyId = submittedApp.SelectedVarietyId;
                }

                if (!string.IsNullOrWhiteSpace(model.PlantingStock2.PsCertNum))
                {
                    if (appToUpdate.PlantingStocks.Count > 1)
                    {
                        // Original App had 2 PS as does this one
                        var PS2ToUpdate = appToUpdate.PlantingStocks.Last();
                        PS2ToUpdate.PsCertNum = model.PlantingStock2.PsCertNum;
                        PS2ToUpdate.PsEnteredVariety = model.PlantingStock2.PsEnteredVariety;
                        PS2ToUpdate.PoundsPlanted = model.PlantingStock2.PoundsPlanted;
                        PS2ToUpdate.PsClass = model.PlantingStock2.PsClass;
                        PS2ToUpdate.PsAccession = model.PlantingStock2.PsAccession;
                        PS2ToUpdate.StateCountryGrown = model.PlantingStock2.StateCountryGrown;
                        PS2ToUpdate.StateCountryTagIssued = model.PlantingStock2.StateCountryTagIssued;
                        PS2ToUpdate.SeedPurchasedFrom = model.PlantingStock2.SeedPurchasedFrom;
                        PS2ToUpdate.WinterTest = model.PlantingStock2.WinterTest;
                        PS2ToUpdate.PvxTest = model.PlantingStock2.PvxTest;
                        PS2ToUpdate.ThcPercent = model.PlantingStock2.ThcPercent;
                    }
                    else
                    {
                        // Original app had 1 ps, but edit has 2
                        var newPS2 = TransferPlantingStockFromSubmission(model.PlantingStock2);
                        newPS2.AppId = PSToUpdate.AppId;
                        _dbContext.Add(newPS2);
                    }

                }
                else
                {
                    //Check if app had 2 ps, removed second one
                    model.PlantingStock2.PsCertNum = "0";
                    if (appToUpdate.PlantingStocks.Count > 1)
                    {
                        var psToRemove = appToUpdate.PlantingStocks.Last();
                        _dbContext.Remove(psToRemove);
                    }
                }

                if ((model.PlantingStock1.PsClass >= submittedApp.ClassProducedId && submittedApp.ClassProducedAccession == null) || (model.PlantingStock1.PsAccession >= submittedApp.ClassProducedAccession))
                {
                    appToUpdate.WarningFlag = true;
                    if (appToUpdate.ApplicantNotes == null)
                    {
                        appToUpdate.ApplicantNotes = "Class produced is less than or equal to class planted; ";
                    } else if (!appToUpdate.ApplicantNotes.Contains("Class produced is less than or equal to class planted"))
                    {
                        appToUpdate.ApplicantNotes += "Class produced is less than or equal to class planted; ";
                    }
                }
            }
            appToUpdate.ClassProducedId = submittedApp.ClassProducedId;

            if(submittedApp.AppType == "HP")
            {
                appToUpdate.CountyPermit = submittedApp.CountyPermit;                
            }
            if(submittedApp.AppType == "PO")
            {               
                appToUpdate.PoLotNum = submittedApp.PoLotNum;
            }
            if(submittedApp.AppType == "GQ")
            {
                appToUpdate.ClassProducedAccession = submittedApp.ClassProducedAccession;
            }
            
            if(submittedApp.AppType == "PV")
            {
                appToUpdate.PvgSource = submittedApp.PvgSource;
                appToUpdate.PvgSelectionId = submittedApp.PvgSelectionId;
                appToUpdate.EcoregionId = submittedApp.EcoregionId;
                appToUpdate.FieldElevation = submittedApp.FieldElevation;
            }
            

            ModelState.Clear();    
            if(submittedApp.FarmCounty == 0)       
            {
                ModelState.AddModelError("Application.FarmCounty","Must select a Farm county");
            }
            if (TryValidateModel(model))
            {                  
                await _dbContext.SaveChangesAsync();

                Message = "Application successfully updated!";
                return RedirectToAction("Details", new { id = appToUpdate.Id });
            }
            
            var retryModel = await ApplicationViewModel.CreateEditModel(_dbContext, id);    
            return View(retryModel);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(int id)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found!";
                return  RedirectToAction(nameof(Index));
            }   
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            var crop = await _dbContext.Crops.Where(c => c.CropId == app.CropId).FirstOrDefaultAsync();
            app.Postmark = DateTime.Now;
            app.Submitable = false;
            app.Status = ApplicationStatus.PendingAcceptance.GetDisplayName();
            app.Deadline = app.DatePlanted.Value.AddDays(crop.AppDue.Value);
            await _notificationService.ApplicationSubmitted(app);

            if (TryValidateModel(app))
            {                  
                await _dbContext.SaveChangesAsync();
                Message = "Application submitted for acceptance!";
                                
                var p0 = new SqlParameter("@appId", System.Data.SqlDbType.Int);
                p0.Value = id;
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_app_submission_update_pinning_map @appId", p0);
            } else
            {
                ErrorMessage = "Something went wrong";
            }
            return RedirectToAction("Details", new { id = app.Id });
        }

        [HttpPost]
        public async Task<IActionResult> ReSubmit(int id)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found!";
                return  RedirectToAction(nameof(Index));
            }   
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }           
            app.Status = ApplicationStatus.PendingAcceptance.GetDisplayName();
            
            await _notificationService.ApplicationReSubmitted(app);

            if (TryValidateModel(app))
            {                  
                await _dbContext.SaveChangesAsync();
                Message = "Application resubmitted";
            } else
            {
                ErrorMessage = "Something went wrong";
            }
            return RedirectToAction("Details", new { id = app.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Cancel(int id)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found!";
                return  RedirectToAction(nameof(Index));
            }   
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            app.Status = ApplicationStatus.ApplicationCancelled.GetDisplayName();
            app.Submitable = false;
            app.Cancelled = true;
            app.Comments += "Canceled prior to submission";

            if (TryValidateModel(app))
            {                  
                await _dbContext.SaveChangesAsync();
                Message = "Application cancelled!";
            } else
            {
                ErrorMessage = "Something went wrong";
            }
            return RedirectToAction("Details", new { id = app.Id });

        }

        public async Task<IActionResult> NewSite(int id)
        {
            var model = await NativeSeedSiteEditModel.CreateModel(_dbContext, id);
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewSite(int id, NativeSeedSiteEditModel vm)
        {
            var siteToCreate = new NativeSeedSites();
            var app = await _dbContext.Applications.Where(a => a.Id == vm.Site.AppId).FirstOrDefaultAsync();
            if (app == null || id != vm.Site.AppId)
            {
                ErrorMessage = "Site Application not found!";
                return RedirectToAction(nameof(Index));
            }
            if (app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That site belongs to an app that does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            var submittedSite = vm.Site;
            siteToCreate.SiteName = submittedSite.SiteName;
            siteToCreate.AppId = submittedSite.AppId;
            siteToCreate.CollectionAreaSize = submittedSite.CollectionAreaSize;
            siteToCreate.FieldElevation = submittedSite.FieldElevation;
            siteToCreate.Lat = submittedSite.Lat;
            siteToCreate.Long = submittedSite.Long;
            siteToCreate.SiteCounty = submittedSite.SiteCounty;
            siteToCreate.HarvestDate = submittedSite.HarvestDate;
            siteToCreate.Comments = submittedSite.Comments;

            if (ModelState.IsValid)
            {
                _dbContext.Add(siteToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Site Created";
            }
            else
            {
                ErrorMessage = "Something went wrong.";
                var model = await NativeSeedSiteEditModel.CreateModel(_dbContext, id);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id = siteToCreate.AppId });
        }

        public async Task<IActionResult> EditSite (int id)
        {
            var model = await NativeSeedSiteEditModel.EditModel(_dbContext, id);
            if(model.Site == null)
            {
                ErrorMessage = "Site not found!";
                return RedirectToAction(nameof(Index));
            }
            var app = await _dbContext.Applications.Where(a => a.Id == model.Site.AppId).FirstOrDefaultAsync();
            if (app == null)
            {
                ErrorMessage = "Site Application not found!";
                return RedirectToAction(nameof(Index));
            }
            if (app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That site belongs to an app that does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditSite (int id, NativeSeedSiteEditModel vm)
        {
            var siteToUpdate = await _dbContext.NativeSeedSites.Where(s => s.Id == vm.Site.Id).FirstOrDefaultAsync();
            if (siteToUpdate == null)
            {
                ErrorMessage = "Site not found!";
                return RedirectToAction(nameof(Index));
            }
            var app = await _dbContext.Applications.Where(a => a.Id == siteToUpdate.AppId).FirstOrDefaultAsync();
            if (app == null)
            {
                ErrorMessage = "Site Application not found!";
                return RedirectToAction(nameof(Index));
            }
            if (app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That site belongs to an app that does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            var updatedSite = vm.Site;
            siteToUpdate.SiteName = updatedSite.SiteName;
            siteToUpdate.CollectionAreaSize = updatedSite.CollectionAreaSize;
            siteToUpdate.FieldElevation = updatedSite.FieldElevation;
            siteToUpdate.Lat = updatedSite.Lat;
            siteToUpdate.Long = updatedSite.Long;
            siteToUpdate.SiteCounty = updatedSite.SiteCounty;
            siteToUpdate.HarvestDate = updatedSite.HarvestDate;
            siteToUpdate.Comments = updatedSite.Comments;

            if (ModelState.IsValid)
            {
                await _dbContext.SaveChangesAsync();
                Message = "Site Updated";
            }
            else
            {
                ErrorMessage = "Something went wrong.";
                var model = await NativeSeedSiteEditModel.EditModel(_dbContext, id);
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id = siteToUpdate.AppId });
            
        }

        public async Task<IActionResult> DeleteHistory (int id)
        {
            var historyToDelete = await _dbContext.FieldHistory.Where(h => h.Id == id).FirstOrDefaultAsync();
            var app = await _dbContext.Applications.Where(a => a.Id == historyToDelete.AppId).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            _dbContext.Remove(historyToDelete);
            await _dbContext.SaveChangesAsync();
            Message ="History deleted.";
            return RedirectToAction(nameof(Edit), new { id = historyToDelete.AppId }); 
        }

         public async Task<IActionResult> EditHistory(int id)
        {
            var model = await AdminHistoryViewModel.Create(_dbContext, id);
            var app = await _dbContext.Applications.Where(a => a.Id == model.history.AppId).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }
        
        public async Task<IActionResult> NewHistory(int id)
        {
            var model = await AdminHistoryViewModel.Create(_dbContext, 0, id);
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);           
        }

        [HttpPost]
        public async Task<IActionResult> NewHistory(int id, AdminHistoryViewModel historyVm)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            var history = historyVm.history;
            var newHistory = new FieldHistory();
            newHistory.AppId = history.AppId;
            newHistory.Year = history.Year;
            if(history.Crop != 0)
            {
                newHistory.Crop = history.Crop;
            }            
            newHistory.Variety = history.Variety;
            newHistory.AppNumber = history.AppNumber;
            newHistory.DateModified = DateTime.Now;
            newHistory.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);
            if(ModelState.IsValid){
                _dbContext.Add(newHistory);
                await _dbContext.SaveChangesAsync();
                Message = "Field History Created";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminHistoryViewModel.Create(_dbContext, 0, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Edit), new { id = newHistory.AppId });              
        }

        [HttpPost]
        public async Task<IActionResult> EditHistory(int id, AdminHistoryViewModel historyVm)
        {
            var historyToUpdate = await _dbContext.FieldHistory.Where(f => f.Id == id).FirstAsync();
            var app = await _dbContext.Applications.Where(a => a.Id == historyToUpdate.AppId).FirstOrDefaultAsync();
            if(app == null)
            {
                ErrorMessage = "Application not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            var history = historyVm.history;
            
            historyToUpdate.Year = history.Year;
            historyToUpdate.Crop = history.Crop;
            historyToUpdate.Variety = history.Variety;
            historyToUpdate.AppNumber = history.AppNumber;
            historyToUpdate.DateModified = DateTime.Now;
            historyToUpdate.UserEmpModified = User.FindFirstValue(ClaimTypes.Name);
            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Field History Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminHistoryViewModel.Create(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Edit), new { id = historyToUpdate.AppId });              
        }

        public async Task<IActionResult> FieldMap(int id)
        {
            var model = await AdminMapFieldsViewModel.SingleMap(_dbContext, id, _helper);
            if(model.details.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }           
            return View(model);  
        }


        public async Task<IActionResult> FieldMapG(int id)
        {
            var model = await AdminMapFieldsViewModel.SingleMap(_dbContext, id, _helper);
            if (model.details.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GoogleAPIKey = _config["GoogleApiKey"];
            return View(model);
          }

        public async Task<IActionResult> SitesMap(int id)
        {
            var model = await AdminMapFieldsViewModel.SitesMap(_dbContext, id, _helper);

            if (model.details.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GoogleAPIKey = _config["GoogleApiKey"];
            return View(model);
        }

        public async Task<IActionResult> LinkMap(int id)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            if(app == null || app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "App doesn't exist or does not belong to your organization";
                return RedirectToAction(nameof(Index));
            }

            var p0 = new SqlParameter("@contactId", System.Data.SqlDbType.Int);
            p0.Value = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);         
            var p1 = new SqlParameter("@appId", System.Data.SqlDbType.Int);
            p1.Value = id;
           
            var model =  await _dbContext.MapLinks.FromSqlRaw($"EXEC mvc_app_map_link_list @contactId,@appId", p0, p1).ToListAsync();
            ViewBag.AppId = id;
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> LinkMap(int AppId, int CropptId)
        {
            var p0 = new SqlParameter("@pinId", System.Data.SqlDbType.Int);
            p0.Value = CropptId;
            var p1 = new SqlParameter("@contactId", System.Data.SqlDbType.Int);
            p1.Value = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);         
            var p2 = new SqlParameter("@appId", System.Data.SqlDbType.Int);
            p2.Value = AppId;

            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_app_iso_map_link_field @pinId, @contactId, @appId", p0, p1, p2);

            Message = $"App {AppId} updated and linked to CropPtId {CropptId}";
            return RedirectToAction(nameof(Details), new { id = AppId }); 
            
        }

        public async Task<IActionResult> NewMap(int id)
        {
            var model = await NewMapViewModel.CreateClient(_dbContext, id);           
            if(model.application == null)
            {
                ErrorMessage = "App not found!";
                return  RedirectToAction(nameof(Index));
            }
            if(model.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> NewMapG(int id)
        {
            var model = await NewMapViewModel.CreateClient(_dbContext, id);
            if (model.application == null)
            {
                ErrorMessage = "App not found!";
                return RedirectToAction(nameof(Index));
            }
            if (model.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return RedirectToAction(nameof(Index));
            }
            ViewBag.GoogleAPIKey = _config["GoogleApiKey"];
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewMapG(int id, string newPolygon, string updateBoth)
        {
            var points = new SqlParameter("points", newPolygon);
            var msg = new SqlParameter
            {
                ParameterName = "msg",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 100,
            };
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_map_app_validate_field @points, @msg output", points, msg);
            if (msg.Value.ToString() != "Ok")
            {
                switch (msg.Value.ToString())
                {
                    case "Not valid":
                        ErrorMessage = "Field not valid. This most commonly is because the external boundary of the field crosses itself. Please redraw field.";
                        break;
                    case "Ring orientation":
                        ErrorMessage = "Field area is quite large. This most commonly is because the field was drawn in the wrong direction. Please redraw field; ensure you draw in a clockwise direction (start at north west corner, then move east).";
                        break;
                };
                var model = await NewMapViewModel.CreateClient(_dbContext, id);
                if (model.application == null)
                {
                    ErrorMessage = "App not found!";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.GoogleAPIKey = _config["GoogleApiKey"];
                return View(model);
            }

            var appId = new SqlParameter("app_id", id);
            var link = new SqlParameter("link", updateBoth == "on" ? true : false);

            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_insert_app_map @app_Id, @points, @link", appId, points, link);
            Message = "Field Updated";
            return RedirectToAction(nameof(Details), new { id = id });
        }

        [HttpPost]
        public async Task<IActionResult> NewMap(int id, string map, string updateBoth)
        {
            var points = new SqlParameter("points", map);
            var msg = new SqlParameter
            {
                ParameterName = "msg",
                SqlDbType = System.Data.SqlDbType.VarChar,
                Direction = System.Data.ParameterDirection.Output,
                Size = 100,
            };
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_map_app_validate_field @points, @msg output", points, msg); 
            if(msg.Value.ToString() != "Ok")
            {
                switch(msg.Value.ToString()){
                    case "Not valid":
                        ErrorMessage = "Field not valid. This most commonly is because the external boundary of the field crosses itself. Please redraw field.";
                        break;
                    case "Ring orientation":
                        ErrorMessage = "Field area is quite large. This most commonly is because the field was drawn in the wrong direction. Please redraw field; ensure you draw in a clockwise direction (start at north west corner, then move east).";
                        break;
                };
                var model = await NewMapViewModel.CreateClient(_dbContext, id);           
                if(model.application == null)
                {
                    ErrorMessage = "App not found!";
                    return  RedirectToAction(nameof(Index));
                }
                return View(model);
            }

            var appId = new SqlParameter("app_id", id);
            var link = new SqlParameter("link", updateBoth == "on" ? true : false);
            
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_insert_app_map @app_Id, @points, @link", appId, points, link); 
            Message = "Field Updated";
            return RedirectToAction(nameof(Details), new { id = id }); 
        }

        [HttpPost]
        public async Task<IActionResult> UploadCertificate(int id, string certName, IFormFile file)
        {
            certName = certName?.Trim();
           var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
           if(app == null)
           {
               ErrorMessage = "App not found";
               return RedirectToAction(nameof(Index));
           }
           if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
           if(string.IsNullOrWhiteSpace(certName))
            {                
                ErrorMessage = "Must provide a certificate name for this file!";
                return RedirectToAction(nameof(Details), new { id = id });
            }
           
           var ext = Path.GetExtension(file.FileName).ToLowerInvariant();

           if(_fileService.CheckDeniedExtension(ext))
           {
               ErrorMessage = "File extension not allowed!";
               return RedirectToAction(nameof(Details), new { id = id });
           }      
           
           if(file.Length >0)
           {
               await _fileService.SaveCertificateFile(app, file);               
                var cert = new AppCertificates();
                cert.AppId = app.Id;
                cert.Name = certName;
                cert.Link = file.FileName;
                _dbContext.Add(cert);
                await _dbContext.SaveChangesAsync();
                Message = "Certificate added";

                var appId = new SqlParameter("app_id", id);
                await _dbContext.Database.ExecuteSqlRawAsync($"EXEC check_app_status @app_id", appId); 

           }
           return RedirectToAction(nameof(Details), new { id = id }); 
        }

        public async Task<IActionResult> GetCertificateFile(int id, string link)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == id).FirstOrDefaultAsync();
            
            if(app == null)
            {
                ErrorMessage = "App not found";
                return RedirectToAction(nameof(Index));
            }
            if(app.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            var contentType = "APPLICATION/octet-stream";
            return File(_fileService.DownloadCertificateFile(app, link), contentType, link);
        }

        public async Task<IActionResult> FIRSummary(int certYear)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);          
            int? certYearToUse;
            if (certYear == 0)
            {
                certYearToUse = await _dbContext.Applications.Where(a => a.ApplicantId == orgId).MaxAsync(x => (int?)x.CertYear);
            } else
            {
                certYearToUse = certYear;
            }
            if(certYearToUse == null)
            {
                certYearToUse = CertYearFinder.CertYear;
            }
            var model = await ApplicationIndexViewModel.FIRSummary(_dbContext, orgId, certYearToUse.Value);
            ViewBag.IsFIR = true;
            return View("Index",model);
        }

        public async Task<IActionResult> FIR(int id)
        {
            var model = await AdminViewModel.CreateFIR(_dbContext, id, _helper);
            if(model.application == null)
            {
                ErrorMessage = "Application not found or no FIR ready (not accepted?)";
                return RedirectToAction(nameof(Index));
            }
            if(model.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }
            if(model.application.AppType == AppTypes.Potato.GetDisplayName())
            {
                return View("~/Areas/Admin/Views/Application/FIRCertificatePotato.cshtml", model);
            }
            if(model.application.ApplicantId != int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value))
            {
                ErrorMessage = "That app does not belong to your organization.";
                return  RedirectToAction(nameof(Index));
            }            
            return View("~/Areas/Admin/Views/Application/FIRCertificate.cshtml",model);
        }

        public async Task<IActionResult> Renewals()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }
            var certYear = CertYearFinder.CertYear;
            var model = await _helper.FullRenewFields().Where(r => r.Year == certYear && r.Action == 0 && r.Application.ApplicantId == orgId).ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> Renew_app(int id)
        {
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
            var appToRenew = await _dbContext.Applications.Where(a => a.Id == renew.AppId).FirstAsync();
            var newApp =  MapRenewFromApp(appToRenew);   

            renew.Action = 1;
            renew.DateRenewed = DateTime.Now;
           
            _dbContext.Add(newApp);                            
            _dbContext.Update(renew);
            await _dbContext.SaveChangesAsync();

             var newFIR = new FieldInspectionReport();
            newFIR.AppId = newApp.Id;                        
            _dbContext.Add(newFIR);

            await _notificationService.ApplicationRenewed(newApp);

            await _dbContext.SaveChangesAsync();

            Message = $"App renewed. New App ID: {newApp.Id}";		
		
            return  RedirectToAction(nameof(Renewals));
        }

         public async Task<IActionResult> Renew_noseed(int id)
        {
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
            var appToRenew = await _dbContext.Applications.Where(a => a.Id == renew.AppId).FirstAsync();
            var newApp =  MapRenewFromApp(appToRenew);  

            newApp.Comments = "App renew, NO CROP";          

            renew.Action = 3;
            renew.DateRenewed = DateTime.Now;
            
            _dbContext.Add(newApp);                
            _dbContext.Update(renew);
            await _dbContext.SaveChangesAsync();

             var newFIR = new FieldInspectionReport();
            newFIR.AppId = newApp.Id;                        
            _dbContext.Add(newFIR);

            await _notificationService.ApplicationRenewNoSeed(newApp);

            await _dbContext.SaveChangesAsync();

            Message = $"App renewed for NO SEED. New App ID: {newApp.Id}";		
		
            return  RedirectToAction(nameof(Renewals));;
        }

        public async Task<IActionResult> Renew_cancel(int id)
        {
            var renew = await _dbContext.RenewFields.Where(r => r.Id == id).FirstAsync();
           
            renew.Action = 2;
            renew.DateRenewed = DateTime.Now;
            
            _dbContext.Update(renew);
            await _notificationService.ApplicationRenewalCancelled(renew);
            await _dbContext.SaveChangesAsync();

            Message = $"App renewal cancelled.";		
		
            return  RedirectToAction(nameof(Renewals));;
        }


        private bool FieldHistoryExists(FieldHistory submittedFh)
        {
            if(submittedFh == null)
            {
                return false;
            }
            if(!string.IsNullOrWhiteSpace(submittedFh.Variety) || !string.IsNullOrWhiteSpace(submittedFh.AppNumber) || submittedFh.Crop != 0)
            {
                return true;
            }
            return false;
        }

        private FieldHistory TransferFieldHistoryFromSubmission(FieldHistory submittedFH)
        {
            var newFH = new FieldHistory();
            newFH.Year = submittedFH.Year;
            newFH.Crop = submittedFH.Crop;
            newFH.Variety = submittedFH.Variety;
            newFH.AppNumber = submittedFH.AppNumber;

            return newFH;
        }

        private PlantingStocks TransferPlantingStockFromSubmission (PlantingStocks submittedPS)
        {
            var newPS = new PlantingStocks();
            newPS.PsCertNum = submittedPS.PsCertNum;
            newPS.PsEnteredVariety = submittedPS.PsEnteredVariety;            
            newPS.PoundsPlanted = submittedPS.PoundsPlanted;
            newPS.PsClass = submittedPS.PsClass;
            newPS.PsAccession = submittedPS.PsAccession;
            newPS.StateCountryGrown = submittedPS.StateCountryGrown;
            newPS.StateCountryTagIssued = submittedPS.StateCountryTagIssued;
            newPS.SeedPurchasedFrom = submittedPS.SeedPurchasedFrom;
            newPS.WinterTest = submittedPS.WinterTest;
            newPS.PvxTest = submittedPS.PvxTest;
            newPS.DateEntered = DateTime.Now;
            newPS.ThcPercent = submittedPS.ThcPercent;

            return newPS;

        }

        public ActionResult HempInfo(int growerId, int appTypeId)
        {
            ViewBag.GrowerId = growerId;
            ViewBag.AppType = appTypeId;
            return View();
        }

        // GET: Application/CreateSeedApplication
        public async Task<IActionResult> CreateSeedApplication(int orgId, int appTypeId)
        {
            var model = new SeedApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Seed/CreateSeedApplication", model);
        }

        // POST: Application/CreateSeedApplication
        [HttpPost]
        public async Task<IActionResult> CreateSeedApplication(SeedApp seedApp)
        {
            if (ModelState.IsValid)
            {
                // Get contact id associated with growerid
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
                var days = await _dbContext.Crops.Where(c => c.CropId == seedApp.CropId).Select(c => c.AppDue).FirstOrDefaultAsync();

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateSeedAppRecord(seedApp, contactId, "SD");
                if (app.DatePlanted.HasValue && days.HasValue)
                {
                    app.Deadline = app.DatePlanted.Value.AddDays(days.Value); 
                }
                
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)seedApp.GrowerId, (int)AppTypes.Seed);
            if (SecondPlantingStockErrors())
            {
                appViewModel.RenderSecondPlantingStock = true;
            }
            appViewModel.RenderFormRemainder = true;

            // Newtonsoft.Json.Linq.JArray fhIndices = Newtonsoft.Json.Linq.JArray.Parse(seedApp.AppViewModel.FieldHistoryIndicesStr);
            appViewModel.FieldHistoryIndices = seedApp.AppViewModel.FieldHistoryIndices;
            seedApp.AppViewModel = appViewModel;
            return View("Seed/CreateSeedApplication", seedApp);
        }

        private bool SecondPlantingStockErrors()
        {
            foreach (var key in ModelState.Keys)
            {
                var val = ModelState[key];
                foreach (var error in val.Errors)
                {
                    if (key.Contains("PlantingStocks[1]"))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        // GET: Application/CreatePotatoApplication
        public async Task<IActionResult> CreatePotatoApplication(int orgId, int appTypeId)
        {            
            var model = new PotatoApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Potato/CreatePotatoApplication", model);
        }

        // POST: Application/CreatePotatoApplication
        [HttpPost]
        public async Task<IActionResult> CreatePotatoApplication(PotatoApp potatoApp)
        {
            if (ModelState.IsValid)
            {                
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);                

                // Use helper class to create application record based on app type
                // TODO double check this is actually using contact ID and not looking for grower id
                Applications app = ApplicationPostMap.CreatePotatoAppRecord(potatoApp, contactId, "PO");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)potatoApp.GrowerId, (int)AppTypes.Potato);
            appViewModel.RenderFormRemainder = true;
            potatoApp.AppViewModel = appViewModel;
            return View("Potato/CreatePotatoApplication", potatoApp);
        }

        // GET: Application/CreateHeritageGrainApplication
        public async Task<IActionResult> CreateHeritageGrainApplication(int orgId, int appTypeId)
        {
            var model = new HeritageGrainQAApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("HeritageGrain/CreateHeritageGrainApplication", model);
        }

        // POST: Application/CreateHeritageGrainApplication
        [HttpPost]
        public async Task<IActionResult> CreateHeritageGrainApplication(HeritageGrainQAApp heritageGrainApp)
        {
            if (ModelState.IsValid)
            {               
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateHeritageGrainAppRecord(heritageGrainApp, contactId, "GQ");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)heritageGrainApp.GrowerId, (int)AppTypes.GrainQA);
            appViewModel.RenderFormRemainder = true;
            heritageGrainApp.AppViewModel = appViewModel;
            return View("HeritageGrain/CreateheritageGrainApplication", heritageGrainApp);
        }

        // GET: Application/CreateGermplasmApplication
        public async Task<IActionResult> CreatePreVarietyGermplasmApplication(int orgId, int appTypeId)
        {
            var model = new PreVarietyGermplasmApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("PreVarietyGermplasm/CreatePreVarietyGermplasmApplication", model);
        }

        // POST: Application/CreatePreVarietyGermplasmApplication
        [HttpPost]
        public async Task<IActionResult> CreatePreVarietyGermplasmApplication(PreVarietyGermplasmApp preVarietyGermplasmApp)
        {
            if (ModelState.IsValid)
            {
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreatePreVarietyGermplasmAppRecord(preVarietyGermplasmApp, contactId, "PV");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)preVarietyGermplasmApp.GrowerId, (int)AppTypes.PrevarietyGermplasm);
            appViewModel.RenderFormRemainder = true;
            preVarietyGermplasmApp.AppViewModel = appViewModel;
            return View("PreVarietyGermplasm/CreatepreVarietyGermplasmApplication", preVarietyGermplasmApp);
        }

        // GET: Application/CreateRiceApplication
        public async Task<IActionResult> CreateRiceApplication(int orgId, int appTypeId)
        {
            var model = new RiceQAApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("Rice/CreateRiceApplication", model);
        }

        // POST: Application/CreateRiceApplication
        [HttpPost]
        public async Task<IActionResult> CreateRiceApplication(RiceQAApp riceApp)
        {
            if (ModelState.IsValid)
            {
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateRiceAppRecord(riceApp, contactId, "RQ");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)riceApp.GrowerId, (int)AppTypes.RiceQA);
            appViewModel.RenderFormRemainder = true;
            riceApp.AppViewModel = appViewModel;
            return View("Rice/CreatericeApplication", riceApp);
        }

        // GET: Application/CreateHempFromSeedApplication
        public async Task<IActionResult> CreateHempFromSeedApplication(int orgId, int appTypeId)
        {
            var model = new HempFromSeedApp();
            var viewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId);
            model.AppViewModel = viewModel;
            return View("HempFromSeed/CreateHempFromSeedApplication", model);
        }

        // POST: Application/CreateHempFromSeedApplication
        [HttpPost]
        public async Task<IActionResult> CreateHempFromSeedApplication(HempFromSeedApp hempFromSeedApp)
        {
            if (ModelState.IsValid)
            {
                var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);

                // Use helper class to create application record based on app type
                Applications app = ApplicationPostMap.CreateHempFromSeedAppRecord(hempFromSeedApp, contactId, "HS");
                _dbContext.Add(app);

                // Adds to database and populates AppId.
                await _dbContext.SaveChangesAsync();

                // Add AppId wherever we need it in plantingstocks and fieldhistory
                foreach (PlantingStocks ps in app.PlantingStocks)
                {
                    ps.AppId = app.Id;
                }

                if (app.FieldHistories != null)
                {
                    foreach (FieldHistory fh in app.FieldHistories)
                    {
                        fh.AppId = app.Id;
                    }
                }

                await _dbContext.SaveChangesAsync();
                Message = "Application successfully submitted!";
                return RedirectToAction("Details", new { id = app.Id });
            }
            var appViewModel = await ApplicationViewModel.Create(_dbContext, (int)hempFromSeedApp.GrowerId, (int)AppTypes.HempProgarm);
            appViewModel.RenderFormRemainder = true;
            hempFromSeedApp.AppViewModel = appViewModel;
            return View("HempFromSeed/CreatehempFromSeedApplication", hempFromSeedApp);
        }

        // GET: Application/GrowerLookup
        public async Task<IActionResult> GrowerLookup(int appTypeId)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var checker = await MembershipChecker.Check(_dbContext, orgId);
            if(!checker.CurrentMember)
            {
                return RedirectToAction("Membership","Organization");
            }
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            var contact = await _dbContext.Contacts
                                .Where(c => c.Id == contactId)
                                .FirstOrDefaultAsync();

            // Check if grower is same as applicant
            var abbrevAppType = await _dbContext.AbbrevAppType
                            .Where(a => a.AppTypeId == appTypeId)
                            .FirstOrDefaultAsync();
            if (abbrevAppType.GrowerSameAsApplicant)
            {                
                return RedirectToAction("CreateApplication", new { growerId = orgId, appTypeId = appTypeId });
            }
            else
            {
                return View(abbrevAppType);
            }
        }

      
        // GET: Application/Lookup
        [HttpGet]
        public async Task<IActionResult> Lookup(string lookupVal, int appTypeId)
        {
            lookupVal = lookupVal?.Trim();
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(lookupVal, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.Id == id)
                    .Include(o => o.Addresses.Where(a => a.Active)).ThenInclude(a => a.Address).ThenInclude(a => a.StateProvince)                    
                    .ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.Name.Contains(lookupVal.ToLower()))                    
                    .Include(o => o.Addresses.Where(a => a.Active)).ThenInclude(a => a.Address).ThenInclude(a => a.StateProvince)
                    .ToListAsync();
            }
            GrowerInfo growerInfo = new GrowerInfo();
            growerInfo.Orgs = orgs;
            growerInfo.AppTypeId = appTypeId;
            growerInfo.ActionType = ApplicationPostMap.ActionTypes[appTypeId];
            string fullPartialPath = "/Areas/Client/Views/Application/Shared/_GrowerLookupInfoTable.cshtml";
            return PartialView(fullPartialPath, growerInfo);
        }

        // GET: Application/FindStateProvince
        [HttpGet]
        public async Task<JsonResult> FindStateProvince(int code)
        {
            ModelState.Clear();
            var state_province = await _dbContext.StateProvince.Where(sp => sp.StateProvinceId == code)
                .Select(sp => sp.StateProvinceCode).ToListAsync();
            return Json(state_province);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<JsonResult> GetSubspecies(int cropId)
        {
            var subspecies  = await _dbContext.Subspecies.Where(s => s.CropId == cropId).ToListAsync();
            subspecies.Insert(0, new Subspecies { Id = 0, CropId = cropId, Name = "--" });
            return Json(subspecies);
        }

        // GET: Application/FindVariety
        [HttpGet]
        [AllowAnonymous] 
        public async Task<JsonResult> FindVariety(string name, int cropId)
        {
            name = name?.Trim();
            var varieties = await _dbContext.VarFull
                .Where(v => v.Name.Contains(name) && v.CropId == cropId)
                .Select(v => new VarietyList
                {
                    CropId = v.CropId,                    
                    Id = v.Id,
                    Name = v.Name,                    
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/FindCropVarieties
        [HttpGet]
        [AllowAnonymous] 
        public async Task<JsonResult> FindCropVarieties(int cropId)
        {
            var varieties = await _dbContext.VarOfficial.Where(v => v.CropId == cropId)
                .Select(v => new VarOfficial
                {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/FindGermplasmEntities
        [HttpGet]
        [AllowAnonymous] 
        public async Task<JsonResult> FindGermplasmEntities(string name)
        {
            name = name?.Trim();
            var varieties = await _dbContext.VarOfficial
                .Where(v => v.VarOffName.ToLower() == name.ToLower())
                .Where(v => v.GermplasmEntity == true)
                .Select(v => new VarOfficial
                {
                    CropId = v.CropId,
                    Crop = _dbContext.Crops.Select(c => new Crops { Crop = c.Crop, CropId = c.CropId }).Where(c => c.CropId == v.CropId).SingleOrDefault(),
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName
                })
                .ToListAsync();
            return Json(varieties);
        }

        // GET: Application/NewApp
        public async Task<IActionResult> NewApp()
        {
            var model = await _dbContext.AbbrevAppType
                .ToListAsync();
            return View(model);
        }

        public async Task<IActionResult> GetPartial(string folder, string partialName, int orgId, int appTypeId, int fhEntryId = -1)
        {
            var app = ApplicationPostMap.CreateAppByAppType(appTypeId);
            var appViewModel = await ApplicationViewModel.Create(_dbContext, orgId, appTypeId, fhEntryId);
            app.AppViewModel = appViewModel;
            string fullPartialPath = $"~/Views/Application/{folder}/{partialName}.cshtml";
            ViewData["fhEntryId"] = fhEntryId;
            return PartialView(fullPartialPath, app);
        }

        public async Task<IActionResult> LookupOrg (string lookup)
        {
            lookup = lookup?.Trim();
            var orgs = new List<Organizations>();
            int id = 0;
            // Parsing was successful (we have an ID number instead of a name)
            if (Int32.TryParse(lookup, out id))
            {
                orgs = await _dbContext.Organizations.Where(o => o.Id == id).ToListAsync();
            }
            else
            {
                orgs = await _dbContext.Organizations.Where(o => o.Name.Contains(lookup.ToLower())).ToListAsync();
            }                        
            return PartialView("_LookupOrg", orgs);

        }

        private Applications MapRenewFromApp(Applications appToRenew)
        {
            var newApp = new Applications();
            
            newApp.PaperAppNum = appToRenew.Id;
            newApp.UserDataentry = appToRenew.UserDataentry;
            newApp.CertNum = appToRenew.CertNum;
            newApp.CertYear = CertYearFinder.CertYear;
            newApp.OriginalCertYear = appToRenew.OriginalCertYear;
            newApp.AppType = appToRenew.AppType;
            newApp.ApplicantId = appToRenew.ApplicantId;
            newApp.GrowerId = appToRenew.GrowerId;
            newApp.CropId = appToRenew.CropId;
            newApp.SelectedVarietyId = appToRenew.SelectedVarietyId;
            newApp.EnteredVariety = appToRenew.EnteredVariety;
            newApp.ClassProducedId = appToRenew.ClassProducedId;
            newApp.Received = DateTime.Now;
            newApp.Postmark = DateTime.Now;
            newApp.Deadline = DateTime.Now.AddDays(1);
            newApp.Status = ApplicationStatus.PendingAcceptance.GetDisplayName();
            newApp.AcresApplied = appToRenew.AcresApplied;
            newApp.Renewal = true;
            newApp.FieldName = appToRenew.FieldName;
            newApp.FarmCounty = appToRenew.FarmCounty;
            newApp.Maps = appToRenew.Maps;
            newApp.MapsSubmissionDate = appToRenew.MapsSubmissionDate;
            newApp.MapCenterLat = appToRenew.MapCenterLat;
            newApp.MapCenterLong = appToRenew.MapCenterLong;
            newApp.MapVe = appToRenew.MapVe;
            newApp.MapUploadFile = appToRenew.MapUploadFile;
            newApp.TextField = appToRenew.TextField;
            newApp.GeoTextField = appToRenew.GeoTextField;
            newApp.GeoField = appToRenew.GeoField;
            newApp.DatePlanted = appToRenew.DatePlanted;
            newApp.Tags = appToRenew.Tags;
            newApp.Submitable = false;

            return newApp;

        }

    }
}