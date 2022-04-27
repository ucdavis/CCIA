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

       public List<AbbrevOECDClass> oecdClass {get; set;}

       public List<Countries> oecdCountries { get; set; }
               
        public static async Task<TagCreateEditViewModel> Create(CCIAContext _dbContext, IFullCallService _helper , int id)
        { 
            var tagToEdit = await _helper.FullTag()
                    .Include(t => t.TagBagging)
                    .Include(t => t.EmployeePrinted)
                    .Include(t => t.Documents)
                    .Where(t => t.Id == id).FirstOrDefaultAsync(); 
           var model = new TagCreateEditViewModel
            {
                tag = tagToEdit,
                tagTypes = await _dbContext.AbbrevTagType.OrderBy(t => t.SortOrder).ToListAsync(),
                tagClass = await _dbContext.AbbrevClassSeeds.FromSqlRaw("EXEC class_seed_pot_labeled").ToListAsync(),
                oecdClass = await _dbContext.AbbrevOECDClass.OrderBy(c => c.SortOrder).ToListAsync(),
                oecdCountries = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync()
            };

            if(id == 0){
                model.oecdClass = new List<AbbrevOECDClass>();
                model.oecdCountries = new List<Countries>();                
            }         

            return model;
        }

        
    }    
    
}
