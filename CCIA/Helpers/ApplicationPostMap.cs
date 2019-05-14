using System;
using CCIA.Models;

namespace CCIA.Helpers
{
    public class ApplicationPostMap
    {
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

            foreach (PlantingStocks ps in newApp.PlantingStocks)
            {
                ps.Applications = newApp;
            }

            // Insert app-specific logic (Fieldhistory, plantingstocks, unique fields, validation)
            return newApp;
        }
    }
}
