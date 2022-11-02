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

        public int YearSubmitted { get; set; }

        public List<int> YearsToDisplay { get; set; }
        
        
        public static async Task<ProcessViewModel> Create(CCIAContext _dbContext, ProcessViewModel vm, IFullCallService helper)
        {  
            var stage = (TagStages) vm.SelectedSearch;
            if(vm.YearSubmitted ==0)
            {
                vm.YearSubmitted = CertYearFinder.CertYear;
            }
            var p0 = new SqlParameter("@stage", System.Data.SqlDbType.VarChar); 
            var p1 = new SqlParameter("@year", System.Data.SqlDbType.Int);
            p1.SqlValue = vm.YearSubmitted;
            if(vm.SelectedSearch == 0)
            {                
                p0.SqlValue = TagStages.Requested;
                var tags =  await _dbContext.ProcessTag.FromSqlRaw($"EXEC mvc_tags_by_status_blends @stage, @year", p0, p1).ToListAsync();                                         
                var freshmodel = new ProcessViewModel
                {
                    Tags = tags,
                    SelectedSearch = 0 , 
                    YearsToDisplay = CertYearFinder.certYearListReverse               
                };  
                    
                return freshmodel;
            }

            p0.SqlValue = stage.GetDisplayName();
            
            var model = await _dbContext.ProcessTag.FromSqlRaw($"EXEC mvc_tags_by_status_blends @stage, @year", p0, p1).ToListAsync();
            var viewModel = new ProcessViewModel
            {
                Tags = model,
                SelectedSearch = vm.SelectedSearch,
                YearsToDisplay = CertYearFinder.certYearListReverse                   
            };  
                
            return viewModel;

            
        }

        
    } 
    
}
