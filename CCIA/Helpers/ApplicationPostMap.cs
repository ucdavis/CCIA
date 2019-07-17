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
        public static Applications CreateAppRecord(Applications app, int contactId, string appType)
        {
            var newApp = new Applications()
            {
                AcresApplied = app.AcresApplied,
                ApplicantComments = app.ApplicantComments,
                ApplicantId = contactId,
                AppOriginalCertYear = app.CertYear,
                AppReceived = DateTime.Now,
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
                Status = "Pending supporting material",
                WarningFlag = false,

                PlantingStocks = app.PlantingStocks,
                FieldHistories = app.FieldHistories
            };

            return newApp;
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