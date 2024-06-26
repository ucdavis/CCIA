using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;
using Microsoft.Data.SqlClient;

namespace CCIA.Models
{   
     
    public class AdminBlendsDetailsViewModel
    {
       public BlendRequests blend { get; set; }
       
       public List<LotBlendSummary> components { get; set; }  

       public List<InDirtBlendSummary> dirtComponents { get; set;}  

       public List<VarietyBlendComponents> varietalComponents { get; set; }

       public List<BlendComponentChanges> changes { get; set; }

       public List<BlendDocuments> documents { get; set; }

       public BlendLabsAndStandards LabsAndStandards { get; set; }
        public List<BlendRequests>   relativeBlends { get; set; }



        public static async Task<AdminBlendsDetailsViewModel> Create(CCIAContext _dbContext, IFullCallService _helper, int id)
        {    
            var thisBlend = await _helper.FullBlendRequest().Where(b => b.Id ==id).FirstOrDefaultAsync();          
            var p0 = new SqlParameter("@blend_id", id); 
            var model = new AdminBlendsDetailsViewModel
                {
                    blend = thisBlend,
                    changes = await _dbContext.BlendComponentChanges.Include(c => c.Employee).Where(c => c.BlendId == id).ToListAsync(),
                    documents = await _dbContext.BlendDocuments.Where(d => d.BlendId == id).ToListAsync(),
                };

            if(thisBlend.BlendType == BlendType.Lot.GetDisplayName() || thisBlend.BlendType == BlendType.Varietal.GetDisplayName())
            {
                model.components = await _dbContext.LotBlendSummary.FromSqlRaw($"EXEC mvc_lot_blend_components_summary @blend_id", p0).ToListAsync();                
            } else
            {
                model.dirtComponents = await _dbContext.InDirtBlendSummary.FromSqlRaw($"EXEC mvc_indirt_blend_components @blend_id", p0).ToListAsync();                
            }
            if(thisBlend.BlendType == BlendType.Varietal.GetDisplayName())
            {
                model.varietalComponents = await _dbContext.VarietyBlendComponents
                    .Include(c => c.BlendVariety)
                    .Include(c => c.ComponentVariety)
                    .Where(c => c.BlendVarietyId == thisBlend.VarietyId)
                    .ToListAsync();
            }
            if(thisBlend.BlendType == BlendType.Lot.GetDisplayName() && await _dbContext.BlendLabResults.Where(x => x.BlendId == id).AnyAsync())
            {
                var labsAndStandards = new BlendLabsAndStandards();
                labsAndStandards.Labs = await _dbContext.BlendLabResults.Where(l => l.BlendId == id)
                        .Include(r => r.LabOrganization)
                        .Include(r => r.Changes)
                        .ThenInclude(c => c.Employee)
                        .Include(r => r.Changes)
                        .ThenInclude(c => c.Contact)
                        .FirstOrDefaultAsync();
                if (labsAndStandards.Labs != null && !thisBlend.Sublot)
                {
                    labsAndStandards.Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, thisBlend.LotBlends.First().Sid);
                }
                if (labsAndStandards.Labs != null && thisBlend.Sublot)
                {
                    var parent = await _dbContext.BlendRequests
                        .Include(b => b.LotBlends)
                        .Where(x => x.Id == thisBlend.ParentId).FirstOrDefaultAsync();
                    labsAndStandards.Standards = await CropStandardsList.GetStandardsFromSeed(_dbContext, parent.LotBlends.First().Sid);
                }
                model.LabsAndStandards = labsAndStandards;
            }
            if(thisBlend.Sublot)
            {
                model.relativeBlends = await _dbContext.BlendRequests.Where(b => b.ParentId == thisBlend.ParentId && b.Id != thisBlend.Id).ToListAsync();
            }
            else
            {
                model.relativeBlends = await _dbContext.BlendRequests.Where(b => b.ParentId == thisBlend.Id).ToListAsync();
            }

            return model;
        }
    }    
}
