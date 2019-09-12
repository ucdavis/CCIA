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
        public bool PurityWarning { get; set; }
        public string PurityError { get; set; }


         public LabResultsCheckStandards()
        {  
            HasWarnings = false;
            PurityWarning = false;
        }

         public static async Task<LabResultsCheckStandards> CheckStandardsFromLabs(CCIAContext _dbContext, SampleLabResults labs)
        {
            var returnList = new LabResultsCheckStandards();
            var properties = await _dbContext.Seeds.Where(s => s.Id == labs.SeedsId)
                .Include(s => s.Variety)
                .Include(s => s.Application)
                .Include(s => s.ClassProduced)
                .Select(s => new StandardsProperties{ CropId = s.GetCropId(), CertProgram = s.CertProgram, ClassAbbreviation = s.ClassProduced.Abbrv, ClassId = s.Class.Value})
                .FirstOrDefaultAsync();
            
                
            var cs = await _dbContext.CropStandards.Where(c => c.CropId == properties.CropId && c.Standards.Program == properties.CertProgram && (c.Standards.Category == properties.ClassAbbreviation || c.Standards.Category == "A"))
                .Include(c => c.Standards)
                .Select(c => c.Standards)
                .ToListAsync();
            
            var standard = new Standards();

            if(cs.Any(c => c.Name == "min_purity")){
                standard = cs.First(c => c.Name == "min_purity");
                if(labs.PurityPercent < standard.MinValue || labs.PurityPercent > standard.MaxValue)
                {
                    returnList.HasWarnings = true;
                    returnList.PurityWarning = true;
                    returnList.PurityError = $"Purity does not fall withing crop standard (min: {100 * standard.MinValue}, max: {100 * standard.MaxValue})";
                }                
            }                                    

            return returnList;
        }
    }
}