using CCIA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Helpers
{
    public class LabResultsCheckStandards
    {
        public bool HasWarnings { get; set; }
        public string PurityError { get; set; }
        public string InertError { get; set; }
        public string OtherCropError { get; set; }

        public string OtherKindError { get; set; }
        public string WeedSeedError { get; set; }
        public string PurityGramsError { get; set; }
        public string OtherVarietyError { get; set; }
        public string ForeignMaterialError { get; set; }
        public string SplitsAndCracksError { get; set; }
        public string BadlyDiscoloredError { get; set; }
        public string ChewingInsectDamageError { get; set; }
        public string NoxiousSeedError { get; set; }
        public string NoxiousGramsError { get; set; }
        public string BushelWeightError { get; set; }
        public string GermError { get; set; }
        public string Assay1Error { get; set; }
        // public string Assay2Error { get; set; }

        public bool AssayNeeded { get; set; }
        public string GeneralError {get; set; }


        public LabResultsCheckStandards()
        {
            HasWarnings = false;
            AssayNeeded = false;
        }

        public static async Task<LabResultsCheckStandards> CheckStandardsFromLabs(CCIAContext _dbContext, SampleLabResults labs)
        {
            var returnList = new LabResultsCheckStandards();
            var properties = new StandardsProperties();
            var seed =  await _dbContext.Seeds.Where(s => s.Id == labs.SeedsId)
                .Include(s => s.Variety)
                .Include(s => s.Application)
                .Include(s => s.ClassProduced)
                .FirstOrDefaultAsync();
            if(!string.IsNullOrWhiteSpace(seed.GetVarietyName()) && seed.GetCropId() != 0 && seed.Class.HasValue && seed.CertProgram != null && seed.ClassProduced != null)
            {
                properties.CropId = seed.GetCropId();
                properties.CertProgram = seed.CertProgram;
                properties.ClassAbbreviation = seed.ClassProduced.Abbrv;
                properties.ClassId = seed.Class.Value;                
            } else if(seed.NotFinallyCertified)
            {
                return returnList;
            } else
            {
                returnList.HasWarnings = true;
                returnList.GeneralError = "SID does not have valid crop or class; Cannot verify standards";
                return returnList;
            }


            var cs = await _dbContext.CropStandards.Where(c => c.CropId == properties.CropId && c.Standards.Program == properties.CertProgram && (c.Standards.Category == properties.ClassAbbreviation || c.Standards.Category == "A"))
                .Include(c => c.Standards)
                .Select(c => c.Standards)
                .ToListAsync();

            var standard = new Standards();
            
            if(labs.PurityResults == null || labs.PurityResults == "P")
            {

                if (cs.Any(c => c.Name == "min_purity"))
                {                
                    standard = cs.First(c => c.Name == "min_purity");
                    if (labs.PurityPercent < standard.MinValue || labs.PurityPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.PurityError = $"Purity does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }            
                }

                if (cs.Any(c => c.Name == "max_inert"))
                {
                    standard = cs.First(c => c.Name == "max_inert");
                    if (labs.InertPercent < standard.MinValue || labs.InertPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.InertError = $"Inert does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_other_crop"))
                {
                    standard = cs.First(c => c.Name == "max_other_crop");
                    if (labs.OtherCropPercent < standard.MinValue || labs.OtherCropPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.OtherCropError = $"Other Crop does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_weed_seed"))
                {
                    standard = cs.First(c => c.Name == "max_weed_seed");
                    if (labs.WeedSeedPercent < standard.MinValue || labs.WeedSeedPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.WeedSeedError = $"Weed Seed does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_other_varieties"))
                {
                    standard = cs.First(c => c.Name == "max_other_varieties");
                    if (labs.OtherVarietyPercent < standard.MinValue || labs.OtherVarietyPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.OtherVarietyError = $"Other Variety does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_other_kind"))
                {
                    standard = cs.First(c => c.Name == "max_other_kind");
                    if (labs.OtherVarietyPercent < standard.MinValue || labs.OtherVarietyPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.OtherKindError = $"Other kind does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_foreign_material"))
                {
                    standard = cs.First(c => c.Name == "max_foreign_material");
                    if (labs.ForeignMaterialPercent < standard.MinValue || labs.ForeignMaterialPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.ForeignMaterialError = $"Foreign Material does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_splits_cracks"))
                {
                    standard = cs.First(c => c.Name == "max_splits_cracks");
                    if (labs.SplitsAndCracksPercent < standard.MinValue || labs.SplitsAndCracksPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.SplitsAndCracksError = $"Splits & Cracks does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_badly_discolored"))
                {
                    standard = cs.First(c => c.Name == "max_badly_discolored");
                    if (labs.BadlyDiscoloredPercent < standard.MinValue || labs.BadlyDiscoloredPercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.BadlyDiscoloredError = $"Badly Discolored does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "max_chewing_insect"))
                {
                    standard = cs.First(c => c.Name == "max_chewing_insect");
                    if (labs.ChewingInsectDamagePercent < standard.MinValue || labs.ChewingInsectDamagePercent > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.ChewingInsectDamageError = $"Chewing Insect Damage does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "purity_grams"))
                {
                    standard = cs.First(c => c.Name == "purity_grams");
                    if (labs.PurityGrams < standard.Value)
                    {
                        returnList.HasWarnings = true;
                        returnList.PurityGramsError = $"Purity Grams does not meet crop standard (min: {standard.Value:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "noxious_seed"))
                {
                    standard = cs.First(c => c.Name == "noxious_seed");
                    if (labs.NoxiousCount > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.NoxiousSeedError = $"Noxious Weed Seed does not meet crop standard (max: {standard.MaxValue:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "noxious_grams"))
                {
                    standard = cs.First(c => c.Name == "noxious_grams");
                    if (labs.NoxiousGrams < standard.Value)
                    {
                        returnList.HasWarnings = true;
                        returnList.NoxiousGramsError = $"Noxious Grams does not meet crop standard (min: {standard.Value:n2})";
                    }
                }

                if (cs.Any(c => c.Name == "min_bushel_weight"))
                {
                    standard = cs.First(c => c.Name == "min_bushel_weight");
                    if (labs.BushelWeight < standard.MinValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.BushelWeightError = $"Bushel Weight does not meet crop standard (min: {standard.MinValue:n2})";
                    }
                }
            }

            if(labs.GermPercent == null || labs.GermResults == "P")
            {
                if (cs.Any(c => c.Name == "min_germ"))
                {
                    var totalGerm = labs.GermPercent;
                    if (cs.Any(c => c.Name == "germ_and_hard"))
                    {
                        totalGerm = labs.GermPercent + labs.HardSeedPercent;
                    }
                    if(cs.Any(c => c.Name == "germ_and_dormant"))
                    {
                        totalGerm = labs.GermPercent + labs.DormantSeedPercent;
                    }
                    standard = cs.First(c => c.Name == "min_germ");
                    if (totalGerm < standard.MinValue || totalGerm > standard.MaxValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.GermError = $"Germination does not fall within crop standard (min: {100 * standard.MinValue:n2}, max: {100 * standard.MaxValue:n2})";
                    }
                }
            }

            if(labs.AssayResults == null || labs.AssayResults == "P")
            {
                if (cs.Any(c => c.Name == "assay_required"))
                {
                    returnList.AssayNeeded = true;
                    standard = cs.First(c => c.Name == "assay_required");
                    decimal assayResult = labs.AssayTest.Value ? 1 : 0;
                    if (assayResult > standard.MaxValue || assayResult < standard.MinValue)
                    {
                        returnList.HasWarnings = true;
                        returnList.Assay1Error = $"{standard.TextValue} does not fall within crop standard";
                    }                
                }
            }

            

            return returnList;
        }
    }
}