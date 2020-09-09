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

        public bool potatoApp { get; set; }
        public static async Task<AdminHistoryViewModel> Create(CCIAContext _dbContext, int id)
        {          
            var cropsWNull = await _dbContext.Crops.OrderBy(c => c.Name).ToListAsync();
            cropsWNull.Insert(0, new Crops{ CropId = 0, Crop = "Select Crop"});
            var viewModel = new AdminHistoryViewModel
            {
                history = await _dbContext.FieldHistory.Where(h => h.Id == id).FirstAsync(),
                crops = cropsWNull,
            };           

            return viewModel;
        }

        
    } 
}
