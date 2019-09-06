using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using CCIA.Helpers;

namespace CCIA.Models.SampleLabResultsViewModel
{
    public class SampleLabResultsViewModel
    {
        public SampleLabResults LabResults { get; set; }
        public CropStandardsList  StandardsList { get; set; }

        public static async Task<SampleLabResultsViewModel> Create(CCIAContext _dbContext, int sid)
        {   
            var returnList = new CropStandardsList();
            var seeds = await _dbContext.Seeds.Where(s => s.Id == sid).FirstOrDefaultAsync();
            
            if(seeds != null && seeds.OfficialVarietyId != null)
            {
                var variety = await _dbContext.VarFull.Where(v => v.Id == seeds.OfficialVarietyId)
                    .Include(v => v.Crop)
                    .Select(v => v.CropId)                                                    
                    .FirstOrDefaultAsync();

                var cs = await _dbContext.CropStandards.Where(c => c.CropId == variety)
                    .Include(c => c.Standards)
                    .ToListAsync();

                if(cs.Any(c => c.Standards.Name == "max_foreign_material")){
                    returnList.ShowBeans = true;
                }

               if(cs.Any(c => c.Standards.Name == "assay_required")){
                   var stand = cs.First(c => c.Standards.Name == "assay_required");
                   returnList.ShowAssay1 = true;
                   returnList.Assay1Name = stand.Standards.TextValue;

                   if(cs.Count(c => c.Standards.Name == "assay_required") > 1)
                   {
                       stand = cs.Last(c => c.Standards.Name == "assay_required");
                       returnList.ShowAssay2 = true;
                       returnList.Assay2Name = stand.Standards.TextValue;
                   }                  
               } 
            }          

            return new SampleLabResultsViewModel
            {
                LabResults = await _dbContext.SampleLabResults.Where(s => s.SeedsId == sid)
                    .Include(l => l.LabOrganization)
                    .FirstOrDefaultAsync(),
                StandardsList = returnList,
            };
        }

        
    }

    
}
