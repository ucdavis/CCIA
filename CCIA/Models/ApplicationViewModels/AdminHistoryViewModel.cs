using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models.DetailsViewModels
{   
    public class AdminHistoryViewModel 
    {
        public FieldHistory  history { get; set; }
        public List<Crops> crops { get; set; }
        public int appId { get; set; }
       
        public static async Task<AdminHistoryViewModel> Create(CCIAContext _dbContext, int id, int appId = 0)
        {          
            var cropsWNull = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();
            cropsWNull.Insert(0, new Crops{ CropId = 0, Crop = "Select Crop"});
            var viewModel = new AdminHistoryViewModel
            {
                history = await _dbContext.FieldHistory.Where(h => h.Id == id).FirstOrDefaultAsync(),
                crops = cropsWNull,
                appId = appId
            };           

            return viewModel;
        }

        
    } 
}
