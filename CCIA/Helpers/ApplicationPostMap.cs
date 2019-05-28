using System;
using System.Collections.Generic;
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

            // Insert app-specific logic (Fieldhistory, plantingstocks, unique fields, validation)
            return newApp;
        }

        /* Iterates through FieldHistories List to find valid entries,
        Stages those to be added to the FieldHistories table
        Then sets our app's FieldHistories to be only the valid entries */
        public static Applications CreateFieldHistoryRecords(ICollection<FieldHistory> fieldHistories, Applications app)
        {
            ICollection<FieldHistory> newFieldHistories = new List<FieldHistory>();
            // Iterate through fieldhistories and make a new record for each
            foreach (var fh in fieldHistories)
            {
                if (fh.Year != 0 && fh.Crop != null)
                {
                    fh.AppId = app.Id;
                    //fh.Application = app;
                    newFieldHistories.Add(fh);
                }
            }
            app.FieldHistories = newFieldHistories;
            return app;
        }
    }
}