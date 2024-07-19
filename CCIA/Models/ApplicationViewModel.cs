using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    public class ApplicationViewModel
    {        
        public Applications Application { get; set; }
        public bool Replant { get; set; }
        public int ReplantId { get; set; }
        public ApplicationLabels ApplicationLabels { get; set; }
        public int AppTypeId { get; set; }
        public List<AbbrevClassProduced> ClassProducedList { get; set; }
        public List<AbbrevClassProduced> ClassPlantedList { get; set; }
        public List<County> Counties { get; set; }
        public List<Crops> Crops { get; set; }
        public List<Crops> FullCrops { get; set; }
        public List<Ecoregions> Ecoregions { get; set; }
        public string FieldHistoryIndices { get; set; }
        public int MaxFieldHistoryRecords { get; set; }
        public ICollection<PlantingStocks> PlantingStocks { get; set; }  
        public PlantingStocks PlantingStock1 { get; set; }
        public PlantingStocks PlantingStock2 { get; set; }      
        public Organizations GrowerOrg { get; set; }
        public Organizations Organization { get; set; }
        public bool RenderFormRemainder { get; set; }
        public bool RenderSecondPlantingStock { get; set; }
        public List<StateProvince> StateProvince { get; set; }
        public List<StatesAndCountries> statesAndCountries { get; set; }
        public List<VarOfficial> Varieties { get; set; }
        public List<VarFull> PrefiilledVarieties { get; set; }
        public AbbrevAppType AppType { get; set; }
        public int CertYear { get; set; }

        public string WhatPlanted { get; set; }
        public string WhatProduced { get; set; }
        public string ProducingSeedType { get; set; }
        public string WhereProduction { get; set; }

        public FieldHistory FieldHistory1 { get; set; }
        public FieldHistory FieldHistory2 { get; set; }
        public FieldHistory FieldHistory3 { get; set; }
        public FieldHistory FieldHistory4 { get; set; }
        public FieldHistory FieldHistory5 { get; set; }
        // Maximum number of field history records allowed for a specified app type
        public int LastAgreementYear { get; set; }

        public ApplicationViewModel()
        {
            Replant = false;
        }

        public static async Task<ApplicationViewModel> CreateGeneric(CCIAContext _dbContext, int growerId, int appType, int applicantOrg, string whatPlanted, string whatProduced, string producingSeedType, string whereProduction, int contactId)
        {

            // TODO get rid of all the separate app stuff. Views, model stuff, controller entries.
            var abbrevAppType = await _dbContext.AbbrevAppType.Where(a => a.AppTypeId == appType).FirstOrDefaultAsync();
            var app = new Applications();
            app.AppType = abbrevAppType.Abbreviation;
            app.ApplicantOrganization = await _dbContext.Organizations.Where(o => o.Id == applicantOrg).FirstAsync();            
            var crops = new List<Crops>();
            var varieties = new List<VarFull>();         
            var classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appType).ToListAsync();
            var planted = classes;
           

            switch (appType)
            {
                // Seed
                case 1:
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true && c.CropId != (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();                    
                    break;
                // Potato
                case 2:
                    crops = await _dbContext.Crops.Where(c => c.CropId == (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.CropId == (int) CropIdNames.Potato).OrderBy(v => v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    app.CropId = (int) CropIdNames.Potato;
                    planted = classes.Where(c => c.ClassProducedId != 11).ToList();
                    break;
                // Heritage Grain QA
                case 3:
                    crops = await _dbContext.Crops.Where(c => c.Heritage == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Pre Variety Germplasm
                case 4:
                    crops = await _dbContext.Crops.Where(c => c.PreVarietyGermplasm == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Rice QA
                case 5:
                    crops = await _dbContext.Crops.Where(c => c.CropId == (int) CropIdNames.Rice).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.RiceQa).OrderBy(v=> v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    app.CropId = (int) CropIdNames.Rice;
                    break;
                // Turfgrass
                case 6:
                    varieties = await _dbContext.VarFull.Where(v => v.Turfgrass).OrderBy(v=> v.Name).ToListAsync();
                    var turfgrassVarieties = varieties.Select(v => v.CropId).Distinct().ToList();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    crops = await _dbContext.Crops.Where(c => turfgrassVarieties.Contains(c.CropId)).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;               
                // Hemp program
                case 10:
                    crops = await _dbContext.Crops.Where(c => c.CropId == (int) CropIdNames.Hemp).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    app.CropId = (int) CropIdNames.Hemp;
                    var hempProducing = whatProduced;
                    if(whatProduced == "Seed")
                    {
                        hempProducing = whatProduced + " - " + producingSeedType;
                    }
                    classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appType && c.HempProduction == hempProducing).ToListAsync();
                    planted = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appType && c.HempProduction == hempProducing && c.HempPlanted).ToListAsync();
                    break;
                // LacTracker program
                case 11:
                    crops = await _dbContext.Crops.Where(c => c.LacTracker).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Native Seed
                case 12:
                    crops = await _dbContext.Crops.Where(c => c.PreVarietyGermplasm == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
            }

            crops.Insert(0, new Crops{ CropId=0, Crop="Select crop..."});
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId = 0, Name="Select County..."});

            var model = new ApplicationViewModel
            {                
                AppType = abbrevAppType,
                Application = app,
            ClassProducedList = classes,
                ClassPlantedList = planted,
                Counties = counties,
                Ecoregions =  await _dbContext.Ecoregions.ToListAsync(),
                GrowerOrg = await _dbContext.Organizations.Where(o => o.Id == growerId)                   
                    .Include(o => o.Addresses.Where(a => a.Active))
                    .ThenInclude(o => o.Address)
                    .ThenInclude(a => a.StateProvince)
                    .FirstOrDefaultAsync(),
                statesAndCountries =  await _dbContext.StatesAndCountries.OrderBy(s => s.Ord).ThenBy(s => s.Name).ToListAsync(),
                Crops = crops,
                CertYear = Helpers.CertYearFinder.CertYear,
                FullCrops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                PrefiilledVarieties = varieties,
                WhatPlanted = whatPlanted,
                WhatProduced = whatProduced,
                ProducingSeedType = producingSeedType,
                WhereProduction = whereProduction, 
                PlantingStock1 = new PlantingStocks(),
                PlantingStock2 = new PlantingStocks(),
                LastAgreementYear = await _dbContext.Contacts.Where(c => c.Id == contactId).Select(c => c.LastApplicationAgreementYear.Value).FirstOrDefaultAsync(),
            };

            return model;

        }

        public static async Task<ApplicationViewModel> CreateRetryModel(CCIAContext _dbContext, ApplicationViewModel submittedModel, int applicantId, int contactId)
        {
            var abbrevAppType = await _dbContext.AbbrevAppType.Where(a => a.Abbreviation == submittedModel.Application.AppType).FirstOrDefaultAsync();
            var app = submittedModel.Application;
            app.ApplicantOrganization = await _dbContext.Organizations.Where(o => o.Id == applicantId).FirstAsync();
            var crops = new List<Crops>();
            var varieties = new List<VarFull>();         
            var classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId).ToListAsync();
            var planted = classes;

            switch (abbrevAppType.AppTypeId)
            {
                // Seed
                case 1:
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true && c.CropId != (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();                    
                    break;
                // Potato
                case 2:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.CropId ==  (int) CropIdNames.Potato).OrderBy(v => v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    break;
                // Heritage Grain QA
                case 3:
                    crops = await _dbContext.Crops.Where(c => c.Heritage == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Pre Variety Germplasm
                case 4:
                    crops = await _dbContext.Crops.Where(c => c.PreVarietyGermplasm == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Rice QA
                case 5:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Rice).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.RiceQa).OrderBy(v=> v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    break;
                // Turfgrass
                case 6:
                    varieties = await _dbContext.VarFull.Where(v => v.Turfgrass).OrderBy(v=> v.Name).ToListAsync();
                    var turfgrassVarieties = varieties.Select(v => v.CropId).Distinct().ToList();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    crops = await _dbContext.Crops.Where(c => turfgrassVarieties.Contains(c.CropId)).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;               
                // Hemp program
                case 10:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Hemp).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    var hempProducing = submittedModel.WhatProduced;
                    if(submittedModel.WhatProduced == "Seed")
                    {
                        hempProducing = submittedModel.WhatProduced + " - " + submittedModel.ProducingSeedType;
                    }
                    classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId && c.HempProduction == hempProducing).ToListAsync();
                    planted = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId && c.HempProduction == hempProducing && c.HempPlanted).ToListAsync();
                    break;
                // LacTracker program
                case 11:
                    crops = await _dbContext.Crops.Where(c => c.LacTracker).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
            }

            crops.Insert(0, new Crops{ CropId=0, Crop="Select crop..."});
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId = 0, Name="Select County..."});

            var model = new ApplicationViewModel
            {
                AppType = abbrevAppType,
                Application = app,
                ClassProducedList = classes,
                ClassPlantedList = planted,
                Counties = counties,
                Ecoregions =  await _dbContext.Ecoregions.ToListAsync(),
                GrowerOrg = await _dbContext.Organizations.Where(o => o.Id == submittedModel.Application.GrowerId)
                    .Include(o => o.Addresses.Where(a=> a.Active))
                    .ThenInclude(a => a.Address)
                    .ThenInclude(a => a.StateProvince)
                    .FirstOrDefaultAsync(),
                statesAndCountries =  await _dbContext.StatesAndCountries.OrderBy(s => s.Ord).ThenBy(s => s.Name).ToListAsync(),
                Crops = crops,
                CertYear = Helpers.CertYearFinder.CertYear,
                FullCrops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                PrefiilledVarieties = varieties,
                WhatPlanted = submittedModel.WhatPlanted,
                WhatProduced = submittedModel.WhatProduced,
                ProducingSeedType = submittedModel.ProducingSeedType,
                WhereProduction = submittedModel.WhereProduction, 
                PlantingStock1 = submittedModel.PlantingStock1,
                PlantingStock2 = submittedModel.PlantingStock2,
                LastAgreementYear = await _dbContext.Contacts.Where(c => c.Id == contactId).Select(c => c.LastApplicationAgreementYear.Value).FirstOrDefaultAsync(),
            };

            return model;

        }

        public static async Task<ApplicationViewModel> CreateEditModel(CCIAContext _dbContext, int appId)
        {            
            var app = await _dbContext.Applications
                .Include(a => a.ApplicantOrganization)
                .Include(a => a.PlantingStocks)
                .Include(a => a.FieldHistories)
                .ThenInclude(f => f.FHCrops)
                .Where(a => a.Id == appId)
                .FirstOrDefaultAsync();
            var ps1 = new PlantingStocks();
            var ps2 = new PlantingStocks();
            var abbrevAppType = await _dbContext.AbbrevAppType.Where(a => a.Abbreviation == app.AppType).FirstOrDefaultAsync();
            var crops = new List<Crops>();
            var varieties = new List<VarFull>();         
            var classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId).ToListAsync();
            var planted = classes;
            if(app.PlantingStocks.Count > 0)
            {
                ps1 = app.PlantingStocks.First();
            }
            if(app.PlantingStocks.Count > 1 )
            {
                ps2 = app.PlantingStocks.Last();
            }

            switch (abbrevAppType.AppTypeId)
            {
                // Seed
                case 1:
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true && c.CropId !=  (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();                    
                    break;
                // Potato
                case 2:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Potato).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.CropId ==  (int) CropIdNames.Potato).OrderBy(v => v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    break;
                // Heritage Grain QA
                case 3:
                    crops = await _dbContext.Crops.Where(c => c.Heritage == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Pre Variety Germplasm
                case 4:
                    crops = await _dbContext.Crops.Where(c => c.PreVarietyGermplasm == true).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Rice QA
                case 5:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Rice).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.RiceQa).OrderBy(v=> v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    break;
                // Turfgrass
                case 6:
                    varieties = await _dbContext.VarFull.Where(v => v.Turfgrass).OrderBy(v=> v.Name).ToListAsync();
                    var turfgrassVarieties = varieties.Select(v => v.CropId).Distinct().ToList();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    crops = await _dbContext.Crops.Where(c => turfgrassVarieties.Contains(c.CropId)).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;               
                // Hemp program
                case 10:
                    crops = await _dbContext.Crops.Where(c => c.CropId ==  (int) CropIdNames.Hemp).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    var hempProducing = app.HempWhatProduced;
                    if(app.HempWhatProduced == "Seed")
                    {
                        hempProducing = app.HempWhatProduced + " - " + app.HempProducingSeedType;
                    }
                    classes = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId && c.HempProduction == hempProducing).ToListAsync();
                    planted = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == abbrevAppType.AppTypeId && c.HempProduction == hempProducing && c.HempPlanted).ToListAsync();
                    break;
                // LacTracker program
                case 11:
                    crops = await _dbContext.Crops.Where(c => c.LacTracker).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
            }

            crops.Insert(0, new Crops{ CropId=0, Crop="Select crop..."});
            var counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).ToListAsync();
            counties.Insert(0, new County { CountyId = 0, Name="Select County..."});

            var model = new ApplicationViewModel
            {
                AppType = abbrevAppType,
                Application = app,
                ClassProducedList = classes,
                ClassPlantedList = planted,
                Counties = counties,
                Ecoregions =  await _dbContext.Ecoregions.ToListAsync(),
                GrowerOrg = await _dbContext.Organizations.Where(o => o.Id == app.GrowerId)
                    .Include(o => o.Addresses.Where(a => a.Active))
                    .ThenInclude(a => a.Address)
                    .ThenInclude(a => a.StateProvince)
                    .FirstOrDefaultAsync(),
                statesAndCountries =  await _dbContext.StatesAndCountries.OrderBy(s => s.Ord).ThenBy(s => s.Name).ToListAsync(),
                Crops = crops,
                CertYear = Helpers.CertYearFinder.CertYear,
                FullCrops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                PrefiilledVarieties = varieties,
                WhatPlanted = app.HempWhatPlanted,
                WhatProduced = app.HempWhatProduced,
                ProducingSeedType = app.HempProducingSeedType,
                WhereProduction = app.IsSquareFeet ? "Outdoors" : "Indoors", 
                PlantingStock1 = ps1,
                PlantingStock2 = ps2,
                LastAgreementYear = CertYearFinder.CertYear,
            };

            return model;

        }

        public static async Task<ApplicationViewModel> Create(CCIAContext dbContext, int orgId, int appType, int fhEntryId = -1)
        {
            var appLabels = ApplicationLabels.Create(appType);
            var classToProduce = await dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appType).ToListAsync();
            var crops = await dbContext.Crops.ToListAsync();
            var maxFieldHistoryRecords = 4;            
            // Make a string representation of field history indices for use in JavaScript files
            var fieldHistoryIndices = "[" + string.Join(",", new int[maxFieldHistoryRecords]) + "]";

            // Querying for certain crops depending on Application Type
            switch (appType)
            {
                // Seed
                case 1:
                    crops = await dbContext.Crops
                        .Where(c => c.CertifiedCrop == true)
                        .ToListAsync();
                    break;
                // Potato
                case 2:
                    crops = await dbContext.Crops
                        .Where(c => c.CropId ==  (int) CropIdNames.Potato)
                        .ToListAsync();
                    break;
                // Heritage Grain QA
                case 3:
                    crops = await dbContext.Crops
                        .Where(c => c.Heritage == true)
                        .ToListAsync();
                    break;
                // Pre Variety Germplasm
                case 4:
                    crops = await dbContext.Crops
                        .Where(c => c.PreVarietyGermplasm == true)
                        .ToListAsync();
                    maxFieldHistoryRecords = 5;
                    break;
                // Rice QA
                case 5:
                    maxFieldHistoryRecords = 1;
                    crops = await dbContext.Crops
                        .Where(c => c.CropId ==  (int) CropIdNames.Rice)
                        .ToListAsync();
                    break;
                // // Turfgrass
                // case 6:
                //     break;
                // Hemp from seed
                case 7:
                    maxFieldHistoryRecords = 5;
                    crops = await dbContext.Crops
                        .Where(c => c.CropId ==  (int) CropIdNames.Hemp)
                        .ToListAsync();
                    break;
                // // Hemp from clones
                // case 8:
                //     maxFieldHistoryRecords = 5;
                //     break;
            }
            var stateProvince = await dbContext.StateProvince.ToListAsync();
            var organization = await dbContext.Organizations.Where(o => o.Id == orgId)
                .Include(o => o.Addresses)
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.Countries)
                .Include(o => o.Addresses)
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(o => o.Addresses)
                .ThenInclude(o => o.Address)
                .ThenInclude(a => a.County)
                .FirstOrDefaultAsync();
            /* California's StateProvinceID is 102 -- All applications must come from CA */
            var counties = await dbContext.County
                .Where(c => c.StateProvinceId == 102)
                .ToListAsync();
            var varieties = await dbContext.VarOfficial
                .Select(v => new VarOfficial
                {
                    VarOffId = v.VarOffId,
                    VarOffName = v.VarOffName,
                    CropId = v.CropId,
                    Crop = v.Crop
                })
                .ToListAsync();
            var ecoregions = await dbContext.Ecoregions.ToListAsync();

            var model = new ApplicationViewModel
            {
                ApplicationLabels = appLabels,
                AppTypeId = appType,
                ClassProducedList = classToProduce,
                Crops = crops,
                Counties = counties,
                Ecoregions = ecoregions,
                FieldHistoryIndices = fieldHistoryIndices,
                MaxFieldHistoryRecords = maxFieldHistoryRecords,
                Organization = organization,
                RenderFormRemainder = false,
                RenderSecondPlantingStock = false,
                StateProvince = stateProvince,
                Varieties = varieties
            };

            return model;
        }
    }
}