using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using CCIA.Models;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace CCIA.Helpers
{
    public class ApplicationPostMap
    {
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

        // Remove invalid fieldhistories

        // public static ICollection<T> RemoveInvalidFieldHistories<T>(Applications app)
        // {
        //     List<T> newFieldHistories = new List<T>();
        //     foreach (var fh in app.FieldHistories)
        //     {
        //         if (fh.Year == 0 || fh.Crop == null)
        //         {
        //             newFieldHistories.Add(fh);
        //         }
        //     }
        //     return newFieldHistories;
        // }

        // Remove invalid plantingstocks
        public static List<T> RemoveInvalidPlantingStocks<T>(List<T> plantingStocks)
            where T: PlantingStocks
        {
            foreach (var ps in plantingStocks)
            {
                if (ps.PoundsPlanted == null || ps.PsCertNum == null || ps.PsClass == null)
                {
                    plantingStocks.Remove(ps);
                }
            }
            return plantingStocks;
        }
    }
}