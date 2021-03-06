using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.Collections.Generic;

namespace CCIA.Models.SampleLabResultsViewModel
{
    public class SampleLabResultsViewModel
    {
        public SampleLabResults Labs { get; set; }
        public CropStandardsList  Standards { get; set; }

        public List<Organizations> PrivateLabs { get; set; }

        public static async Task<SampleLabResultsViewModel> Create(CCIAContext _dbContext, int sid)
        { 
            var privateLabs = await _dbContext.Organizations.Where(o => o.GermLab)
                    .Select(o => new Organizations { Id = o.Id, Name = o.Name})
                    .OrderBy(o => o.Name)
                    .ToListAsync();
            privateLabs.Insert(0, new Organizations {Id = 0, Name = "Select lab..."});
            privateLabs.Add(new Organizations {Id= -1, Name = "Other...list in comments"});

            if (!await _dbContext.SampleLabResults.AnyAsync(s => s.SeedsId == sid))
            {
                var labresults = new SampleLabResults();
                labresults.SeedsId = sid;
                await _dbContext.SampleLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();
            }

            return new SampleLabResultsViewModel
            {
                Labs = await _dbContext.SampleLabResults.Where(s => s.SeedsId == sid).FirstOrDefaultAsync(),
                Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid),
                PrivateLabs = privateLabs,
            };
        }

        public static async Task<SampleLabResultsViewModel> ReUse(CCIAContext _dbContext, SampleLabResults labs)
        {
            var privateLabs = await _dbContext.Organizations.Where(o => o.GermLab)
                    .Select(o => new Organizations { Id = o.Id, Name = o.Name})
                    .OrderBy(o => o.Name)
                    .ToListAsync();
            privateLabs.Insert(0, new Organizations {Id = 0, Name = "Select lab..."});
            privateLabs.Add(new Organizations {Id= -1, Name = "Other...list in comments"});

            return new SampleLabResultsViewModel
            {
                Labs = labs,
                Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, labs.SeedsId),
                PrivateLabs = privateLabs,
            };


        }

        
    }

    
}
