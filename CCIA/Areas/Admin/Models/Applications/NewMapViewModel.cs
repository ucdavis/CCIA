using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{   
     
    public class NewMapViewModel
    {
        public Applications application { get; set; }

        public bool showLink { get; set; }

       
        
               
        public static async Task<NewMapViewModel> Create(CCIAContext _dbContext, int id)
        {        
            var appid = new SqlParameter("app_id", id);
            var link = new SqlParameter
            {
                ParameterName = "linked",
                SqlDbType = System.Data.SqlDbType.Bit,
                Direction = System.Data.ParameterDirection.Output,
                Size = 1,
            };  
            await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_app_check_if_linked_to_iso_map @app_id, @linked output", appid, link);
            var model = new NewMapViewModel
                {
                   application = await _dbContext.Applications.Where(a => a.Id==id).FirstOrDefaultAsync(),
                   showLink =  (bool)link.Value,                  
                };
            
            return model;
        }

        public static async Task<NewMapViewModel> CreateClient(CCIAContext _dbContext, int id)
        {  
            var model = new NewMapViewModel
                {
                   application = await _dbContext.Applications.Where(a => a.Id==id).FirstOrDefaultAsync(),
                   showLink =  false,                  
                };
            
            return model;
        }
        
    }    
}
