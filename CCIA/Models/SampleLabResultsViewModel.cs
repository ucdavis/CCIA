using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace CCIA.Models.SampleLabResultsViewModel
{
    public class SampleLabResultsViewModel
    {
        public SampleLabResults Labs { get; set; }
        public BlendLabResults BlendLabs { get; set; }
        public CropStandardsList  Standards { get; set; }

        public List<Organizations> PrivateLabs { get; set; }
        public bool SubmitAsRejected { get; set; }

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

        public static async Task<SampleLabResultsViewModel> CreateBlend(CCIAContext _dbContext, int bid)
        {
            var privateLabs = await _dbContext.Organizations.Where(o => o.GermLab)
                    .Select(o => new Organizations { Id = o.Id, Name = o.Name })
                    .OrderBy(o => o.Name)
                    .ToListAsync();
            privateLabs.Insert(0, new Organizations { Id = 0, Name = "Select lab..." });
            privateLabs.Add(new Organizations { Id = -1, Name = "Other...list in comments" });

            if (!await _dbContext.BlendLabResults.AnyAsync(b => b.BlendId == bid))
            {
                var labresults = new BlendLabResults();
                labresults.BlendId = bid;
                await _dbContext.BlendLabResults.AddAsync(labresults);
                await _dbContext.SaveChangesAsync();
            }

            var sid = await _dbContext.LotBlends.Where(l => l.BlendId == bid).FirstOrDefaultAsync();
            var standards = new CropStandardsList();

            if (sid == null)
            {
                standards = await CropStandardsList.GetStandardsForBlendSublot(_dbContext, bid);
            } else
            {
                standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid.Sid);
            }
            return new SampleLabResultsViewModel
            {
                BlendLabs = await _dbContext.BlendLabResults.Where(b => b.BlendId == bid).FirstOrDefaultAsync(),
                Standards = standards,
                PrivateLabs = privateLabs,
            };
        }

        public static async Task<SampleLabResultsViewModel> ReUse(CCIAContext _dbContext, SampleLabResults labs)
        {
            var privateLabs = await _dbContext.Organizations.Where(o => o.GermLab)
                    .Select(o => new Organizations { Id = o.Id, Name = o.Name })
                    .OrderBy(o => o.Name)
                    .ToListAsync();
            privateLabs.Insert(0, new Organizations { Id = 0, Name = "Select lab..." });
            privateLabs.Add(new Organizations { Id = -1, Name = "Other...list in comments" });
            
            return new SampleLabResultsViewModel
            {
                Labs = labs,
                Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, labs.SeedsId),
                PrivateLabs = privateLabs,
            };


        }

        public static async Task<SampleLabResultsViewModel> ReUseBlend(CCIAContext _dbContext, BlendLabResults labs)
        {
            var privateLabs = await _dbContext.Organizations.Where(o => o.GermLab)
                    .Select(o => new Organizations { Id = o.Id, Name = o.Name})
                    .OrderBy(o => o.Name)
                    .ToListAsync();
            privateLabs.Insert(0, new Organizations {Id = 0, Name = "Select lab..."});
            privateLabs.Add(new Organizations {Id= -1, Name = "Other...list in comments"});

            var sid = await _dbContext.LotBlends.Where(l => l.BlendId == labs.BlendId).FirstOrDefaultAsync();
            var standards = new CropStandardsList();

            if (sid == null)
            {
                standards = await CropStandardsList.GetStandardsForBlendSublot(_dbContext, labs.BlendId);
            }
            else
            {
                standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, sid.Sid);
            }
            return new SampleLabResultsViewModel
            {
                BlendLabs = await _dbContext.BlendLabResults.Where(b => b.BlendId == labs.BlendId).FirstOrDefaultAsync(),
                Standards = standards,
                PrivateLabs = privateLabs,
            };


        }

        
    }

    
}
