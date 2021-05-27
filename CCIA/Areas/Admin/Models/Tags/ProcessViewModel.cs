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
     
    public class ProcessViewModel
    {
        public List<ProcessTag> Tags { get; set; }

        public int SelectedSearch { get; set; }
        
        
        public static async Task<ProcessViewModel> Create(CCIAContext _dbContext, ProcessViewModel vm, IFullCallService helper)
        {  
            var stage = (TagStages) vm.SelectedSearch;
            var p0 = new SqlParameter("@stage", System.Data.SqlDbType.VarChar); 
            if(vm.SelectedSearch == 0)
            {                
                p0.SqlValue = TagStages.Requested;
                var tags =  await _dbContext.ProcessTag.FromSqlRaw($"EXEC mvc_tags_by_status_blends @stage", p0).ToListAsync();                                         
                var freshmodel = new ProcessViewModel
                {
                    Tags = tags,
                    SelectedSearch = 0                   
                };  
                    
                return freshmodel;
            }

            p0.SqlValue = stage.GetDisplayName();
            
            var model = await _dbContext.ProcessTag.FromSqlRaw($"EXEC mvc_tags_by_status_blends @stage", p0).ToListAsync();
            var viewModel = new ProcessViewModel
            {
                Tags = model,
                SelectedSearch = vm.SelectedSearch                   
            };  
                
            return viewModel;

            
        }

        
    } 
    
}
