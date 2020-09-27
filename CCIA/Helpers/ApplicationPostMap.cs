using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CCIA.Helpers
{
    public class ApplicationPostMap
    {
        // Dictionary to store names of Action methods corresponding to App Type Ids
        public static Dictionary<int, string> ActionTypes = new Dictionary<int, string>
        {
            {1, "CreateSeedApplication"},
            {2, "CreatePotatoApplication"},
            {3, "CreateHeritageGrainApplication"},
            {4, "CreatePreVarietyGermplasmApplication"},
            {5, "CreateRiceApplication"},
            {6, "CreateTurfgrassApplication"},
            {7, "CreateHempFromSeedApplication"},
            {8, "CreateHempFromClonesApplication"},
        };

        /* Assigns all required fields from our cshtml */
        public static Applications CreateSeedAppRecord(SeedApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                // Maybe eventually move this to a generic method?
                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications CreatePotatoAppRecord(PotatoApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications CreateHeritageGrainAppRecord(HeritageGrainQAApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications CreatePreVarietyGermplasmAppRecord(PreVarietyGermplasmApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications CreateRiceAppRecord(RiceQAApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications CreateHempFromSeedAppRecord(HempFromSeedApp app, int contactId, string appType)
        {
            var newApp = AssignCommonAppFields(app, contactId, appType);
            newApp.PlantingStocks = new List<PlantingStocks>();

            foreach(var p in app.AppSpecificPlantingStocks)
            {
                var newPlantingStock = new PlantingStocks();

                newPlantingStock.OfficialVarietyId = p.OfficialVarietyId;
                newPlantingStock.PsEnteredVariety = p.PsEnteredVariety;
                newPlantingStock.PsClass = p.PsClass;
                newPlantingStock.PsCertNum = p.PsCertNum;
                newPlantingStock.PoundsPlanted = p.PoundsPlanted;
                newPlantingStock.StateCountryGrown = p.StateCountryGrown;
                newPlantingStock.StateCountryTagIssued = p.StateCountryTagIssued;
                newPlantingStock.SeedPurchasedFrom = p.SeedPurchasedFrom;

                newApp.PlantingStocks.Add(newPlantingStock);
            }

            return newApp;
        }

        public static Applications AssignCommonAppFields(Applications app, int contactId, string appType)
        {
            return new Applications()
            {
                AcresApplied = app.AcresApplied,
                ApplicantComments = app.ApplicantComments,
                ApplicantId = contactId,
                OriginalCertYear = app.CertYear,
                Received = DateTime.Now,
                AppType = appType,
                CertYear = app.CertYear,
                ClassProducedId = app.ClassProducedId,
                CropId = app.CropId,
                DatePlanted = app.DatePlanted,
                EnteredVariety = app.EnteredVariety,
                FarmCounty = app.FarmCounty,
                FieldName = app.FieldName,
                GrowerId = app.GrowerId,
                MapVe = false,
                SelectedVarietyId = app.SelectedVarietyId,
                Status = ApplicationStatus.PendingSupportingMaterial.GetDisplayName(),
                WarningFlag = false,

                FieldHistories = app.FieldHistories
            };
        }

        public static MasterApplicationViewModel CreateAppByAppType(int appTypeId)
        {
            switch(appTypeId)
            {
                case 2:
                    return new PotatoApp();
                case 3:
                    return new HeritageGrainQAApp();
                case 4:
                    return new PreVarietyGermplasmApp();
                case 5:
                    return new RiceQAApp();
                case 6:
                    return new HempFromSeedApp();
                default:
                    return new SeedApp();
            }
        }
    }
}