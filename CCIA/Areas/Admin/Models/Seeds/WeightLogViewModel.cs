using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;

namespace CCIA.Models.ViewModels
{   
    public class WeightLogViewModel 
    {

        public List<Seeds> seeds { get; set; }
        public List<int> Years { get; set; }
        public int Year { get; set; }

        public static async Task<WeightLogViewModel> Create(CCIAContext _dbContext, int year)
        {
            var viewModel = new WeightLogViewModel
            {
                seeds = await _dbContext.Seeds.Where(s => s.YearConfirmed == year && s.Submitted).ToListAsync(),
                Years = await _dbContext.Seeds.OrderByDescending(s => s.YearConfirmed.Value).Select(s => s.YearConfirmed.Value).Distinct().ToListAsync(),
                Year = year,                
            };

            return viewModel;
        }
    }    
}