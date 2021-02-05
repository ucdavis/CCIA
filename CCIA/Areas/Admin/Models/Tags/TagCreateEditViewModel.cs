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
     
    public class TagCreateEditViewModel
    {
        public Tags tag { get; set; } 
        
       public List<AbbrevTagType> tagTypes { get; set; }
               
       public List<AbbrevClassSeeds> tagClass { get; set; }
               
        public static async Task<TagCreateEditViewModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id)
        {              
            var model = new TagCreateEditViewModel
            {
                tag = await _helper.FullTag()
                    .Include(t => t.TagBagging)
                    .Include(t => t.EmployeePrinted)
                    .Where(t => t.Id == id).FirstOrDefaultAsync(),
                tagTypes = await _dbContext.AbbrevTagType.OrderBy(t => t.SortOrder).ToListAsync(),
                tagClass = await _dbContext.AbbrevClassSeeds.OrderBy(c => c.SortOrder).ToListAsync(),
            };

            return model;
        }

        
    }    
    
}
