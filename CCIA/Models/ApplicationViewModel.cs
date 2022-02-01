using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{
    public class ApplicationViewModel
    {
        public readonly static int PotatoCropId = 137;
        public readonly static int RiceCropId = 63;
        public readonly static int HempCropId = 316;
        public Applications Application { get; set; }
        public ApplicationLabels ApplicationLabels { get; set; }
        public int AppTypeId { get; set; }
        public List<AbbrevClassProduced> ClassProducedList { get; set; }
        public List<County> Counties { get; set; }
        public List<Crops> Crops { get; set; }
        public List<Crops> FullCrops { get; set; }
        public List<Ecoregions> Ecoregions { get; set; }
        public string FieldHistoryIndices { get; set; }
        public int MaxFieldHistoryRecords { get; set; }
        public ICollection<PlantingStocks> PlantingStocks { get; set; }        
        public Organizations GrowerOrg { get; set; }
        public Organizations Organization { get; set; }
        public bool RenderFormRemainder { get; set; }
        public bool RenderSecondPlantingStock { get; set; }
        public List<StateProvince> StateProvince { get; set; }
        public List<VarOfficial> Varieties { get; set; }
        public List<VarFull> PrefiilledVarieties { get; set; }
        public AbbrevAppType AppType { get; set; }
        public int CertYear { get; set; }

        public string WhatPlanted { get; set; }
        public string WhatProduced { get; set; }
        public string ProducingSeedType { get; set; }
        public string WhereProduction { get; set; }
        // Maximum number of field history records allowed for a specified app type

        public static async Task<ApplicationViewModel> CreateGeneric(CCIAContext _dbContext, int growerId, int appType, int applicantOrg, string whatPlanted, string whatProduced, string producingSeedType, string whereProduction)
        {
            var abbrevAppType = await _dbContext.AbbrevAppType.Where(a => a.AppTypeId == appType).FirstOrDefaultAsync();
            var app = new Applications();
            app.AppType = abbrevAppType.Abbreviation;
            app.ApplicantOrganization = await _dbContext.Organizations.Where(o => o.Id == applicantOrg).FirstAsync();
            var appLabels = ApplicationLabels.Create(appType);
            var crops = new List<Crops>();
            var varieties = new List<VarFull>();
           

            switch (appType)
            {
                // Seed
                case 1:
                    crops = await _dbContext.Crops.Where(c => c.CertifiedCrop == true && c.CropId != PotatoCropId).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;
                // Potato
                case 2:
                    crops = await _dbContext.Crops.Where(c => c.CropId == PotatoCropId).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.CropId == PotatoCropId).OrderBy(v => v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    app.CropId = PotatoCropId;
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
                    crops = await _dbContext.Crops.Where(c => c.CropId == RiceCropId).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    varieties = await _dbContext.VarFull.Where(v => v.RiceQa).OrderBy(v=> v.Name).ToListAsync();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    app.CropId = RiceCropId;
                    break;
                // // Turfgrass
                case 6:
                    varieties = await _dbContext.VarFull.Where(v => v.Turfgrass).OrderBy(v=> v.Name).ToListAsync();
                    var turfgrassVarieties = varieties.Select(v => v.CropId).Distinct().ToList();
                    varieties.Insert(0, new VarFull { Id=0, Name="Select variety..."});
                    crops = await _dbContext.Crops.Where(c => turfgrassVarieties.Contains(c.CropId)).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
                    break;               
                // Hemp program
                case 10:
                    crops = await _dbContext.Crops.Where(c => c.CropId == HempCropId).OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
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
                ApplicationLabels = appLabels,
                AppType = abbrevAppType,
                Application = app,
                ClassProducedList = await _dbContext.AbbrevClassProduced.Where(c => c.AppTypeId == appType).ToListAsync(),
                Counties = counties,
                Ecoregions =  await _dbContext.Ecoregions.ToListAsync(),
                GrowerOrg = await _dbContext.Organizations.Where(o => o.Id == growerId)
                    .Include(o => o.Address)
                    .Include(o => o.Address)
                    .ThenInclude(a => a.StateProvince)
                    .FirstOrDefaultAsync(),
                StateProvince =  await _dbContext.StateProvince.ToListAsync(),
                Crops = crops,
                CertYear = Helpers.CertYearFinder.CertYear,
                FullCrops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(),
                PrefiilledVarieties = varieties,
                WhatPlanted = whatPlanted,
                WhatProduced = whatProduced,
                ProducingSeedType = producingSeedType,
                WhereProduction = whereProduction
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
                        .Where(c => c.CropId == PotatoCropId)
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
                        .Where(c => c.CropId == RiceCropId)
                        .ToListAsync();
                    break;
                // // Turfgrass
                // case 6:
                //     break;
                // Hemp from seed
                case 7:
                    maxFieldHistoryRecords = 5;
                    crops = await dbContext.Crops
                        .Where(c => c.CropId == HempCropId)
                        .ToListAsync();
                    break;
                // // Hemp from clones
                // case 8:
                //     maxFieldHistoryRecords = 5;
                //     break;
            }
            var stateProvince = await dbContext.StateProvince.ToListAsync();
            var organization = await dbContext.Organizations.Where(o => o.Id == orgId)
                .Include(o => o.Address)
                .ThenInclude(a => a.Countries)
                .Include(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Include(o => o.Address)
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