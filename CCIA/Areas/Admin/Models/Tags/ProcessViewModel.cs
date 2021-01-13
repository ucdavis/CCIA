using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using CCIA.Helpers;
using System.ComponentModel.DataAnnotations;
using CCIA.Services;

namespace CCIA.Models
{   
     
    public class ProcessViewModel
    {
        public List<Tags> Tags { get; set; }

        public int SelectedSearch { get; set; }
        
        
        public static async Task<ProcessViewModel> Create(CCIAContext _dbContext, ProcessViewModel vm, IFullCallService helper)
        {  
            var stage = (TagStages) vm.SelectedSearch;
            if(vm.SelectedSearch == 0)
            {
                var newtag = helper.FullTag();
                stage = TagStages.Requested;
                var newmodel = await newtag.Where(t => t.Stage == stage.GetDisplayName()).ToListAsync();       
                var freshmodel = new ProcessViewModel
                {
                    Tags = newmodel,
                    SelectedSearch = 0                   
                };  
                    
                return freshmodel;
            }

            var tag = helper.FullTag();
            var model = await tag.Where(t => t.Stage == stage.GetDisplayName()).ToListAsync();       
            var viewModel = new ProcessViewModel
            {
                Tags = model,
                SelectedSearch = vm.SelectedSearch                   
            };  
                
            return viewModel;

            
        }

        
    } 
    
}
