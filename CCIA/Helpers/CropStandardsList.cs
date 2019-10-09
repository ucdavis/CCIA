using CCIA.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


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
        public string OtherVarietyType { get; set; }
        public string OtherCropType { get; set; }
        public string WeedSeedType { get; set; }
        public bool ShowBushelWeight { get; set; }
        public bool ShowBeans { get; set; }
        public bool ShowInert { get; set; }

        public CropStandardsList()
        {  
            ShowAssay1 = false;
            Assay1Name = "";
            ShowAssay2 = false;
            Assay2Name = "";
            ShowDodderGrams = false;
            ShowBeans = false; 
            OtherVarietyType = null; 
            OtherCropType = null;
            WeedSeedType = null;
            ShowBushelWeight = false;
            ShowInert = false;
        }

        public static async Task<CropStandardsList> GetStandardsFromSeed(CCIAContext _dbContext, int sid)
        {
            var returnList = new CropStandardsList();
            var cropId = await _dbContext.Seeds.Where(s => s.Id == sid)
                .Include(s => s.Variety)
                .Include(s => s.Application)
                .Select(s => new Tuple<int, string>(s.GetCropId(), s.CertProgram))
                .FirstOrDefaultAsync();
            
                
            var cs = await _dbContext.CropStandards.Where(c => c.CropId == cropId.Item1 && c.Standards.Program == cropId.Item2)
                .Include(c => c.Standards)
                .ToListAsync();

            if(cs.Any(c => c.Standards.Name == "max_foreign_material")){
                returnList.ShowBeans = true;
            }

            if(cs.Any(c => c.Standards.Name == "assay_required")){
                var stand = cs.First(c => c.Standards.Name == "assay_required");
                returnList.ShowAssay1 = true;
                returnList.Assay1Name = stand.Standards.TextValue;

                // if(cs.Count(c => c.Standards.Name == "assay_required") > 1)
                // {
                //     stand = cs.Last(c => c.Standards.Name == "assay_required");
                //     returnList.ShowAssay2 = true;
                //     returnList.Assay2Name = stand.Standards.TextValue;
                // }                  
            } 

            if(cs.Any(c => c.Standards.Name == "max_other_varieties"))
            {
                var maxOtherVariety = cs.First(c => c.Standards.Name == "max_other_varieties");
                returnList.OtherVarietyType = maxOtherVariety.Standards.ValueType;
            }

            if(cs.Any(c => c.Standards.Name == "max_other_crop"))
            {
                var maxOtherCrop = cs.First(c => c.Standards.Name == "max_other_crop");
                returnList.OtherCropType = maxOtherCrop.Standards.ValueType;
            }

            if(cs.Any(c => c.Standards.Name == "max_weed_seed"))
            {
                var maxWeedSeed = cs.First(c => c.Standards.Name == "max_weed_seed");
                returnList.WeedSeedType = maxWeedSeed.Standards.ValueType;
            }

            if(cs.Any(c => c.Standards.Name == "min_bushel_weight"))
            {                
                returnList.ShowBushelWeight = true;
            }

             if(cs.Any(c => c.Standards.Name == "max_inert"))
            {                
                returnList.ShowInert = true;
            }

            if(returnList.Assay1Name == "Dodder" || returnList.Assay2Name == "Dodder")
            {
                returnList.ShowDodderGrams = true;
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