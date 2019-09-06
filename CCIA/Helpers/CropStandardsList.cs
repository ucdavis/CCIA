using CCIA.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;

namespace CCIA.Helpers
{
    public class CropStandardsList
    {
                
        // Type: percent versus count
        
        public bool ShowAssay1 { get; set; }
        public string Assay1Name { get; set; }
        public bool ShowAssay2 { get; set; }
        public string Assay2Name { get; set; }
        public bool ShowDodderGrams { get; set; }

        public bool ShowBeans { get; set; }

        public CropStandardsList()
        {  
            ShowAssay1 = false;
            Assay1Name = "";
            ShowAssay2 = false;
            Assay2Name = "";
            ShowDodderGrams = false;
            ShowBeans = false;  
        }

        public static async Task<CropStandardsList> GetStandardsFromSeed(CCIAContext _dbContext, int sid)
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

            return returnList;
        }
    }

    public class LabsAndStandards
    {
        public SampleLabResults Labs { get; set; }
        public CropStandardsList Standards { get; set; }


        public LabsAndStandards()
        {  
            Labs = new SampleLabResults();
            Standards = new CropStandardsList();
        }

    }
}