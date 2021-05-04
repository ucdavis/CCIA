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

            if(thisBlend.BlendType == "Lot" || thisBlend.BlendType == "Varietal")
            {
                model.components = await _dbContext.LotBlendSummary.FromSqlRaw($"EXEC mvc_lot_blend_components_summary @blend_id", p0).ToListAsync();                
            } else
            {
                model.dirtComponents = await _dbContext.InDirtBlendSummary.FromSqlRaw($"EXEC mvc_indirt_blend_components @blend_id", p0).ToListAsync();                
            }
            if(thisBlend.BlendType == "Varietal")
            {
                model.varietalComponents = await _dbContext.VarietyBlendComponents
                    .Include(c => c.BlendVariety)
                    .Include(c => c.ComponentVariety)
                    .Where(c => c.BlendVarietyId == thisBlend.VarietyId)
                    .ToListAsync();
            }
            return model;
        }
    }    
}
