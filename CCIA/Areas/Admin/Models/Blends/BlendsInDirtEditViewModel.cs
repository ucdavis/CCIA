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
     
    public class AdminBlendsInDirtEditViewModel
    {
       public BlendInDirtComponents comp { get; set; }
       
       public List<Crops> crops { get; set; }  

       public List<Countries> countries { get; set;}  

       public List<StateProvince> stateProvinces { get; set; }

       public List<AbbrevClassProduced> classSeeds { get; set; }

        public static async Task<AdminBlendsInDirtEditViewModel> Create(CCIAContext _dbContext, int id)
        {    
            var thisComp = await _dbContext.BlendInDirtComponents.Where(b => b.Id == id).FirstOrDefaultAsync(); 
            if(thisComp == null) 
            {
                thisComp = new BlendInDirtComponents();
            }
            if(thisComp.StateOfOrigin == null)
            {
                thisComp.StateOfOrigin = 0;
            }
            var states = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();            
            states.Insert(0, new StateProvince{ StateProvinceId = 0, Name=""});

            if(thisComp.CropId == null)
            {
                thisComp.CropId = 0;
            }
            var crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync();            
            crops.Insert(0, new Crops{ CropId = 0, Crop="", CropKind =""});

            if(thisComp.CountryOfOrigin == null)
            {
                thisComp.CountryOfOrigin = 0;
            }
            var countryList = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();            
            countryList.Insert(0, new Countries{ Id = 0, Name = ""});

            if(thisComp.Class == null)
            {
                thisComp.Class = 0;
            }
            var classes = await _dbContext.AbbrevClassProduced.Include(c => c.AppType).OrderBy(c => c.AppType.AppTypeTrans).ThenBy(c => c.ClassProducedTrans).ToListAsync();            
            classes.Insert(0, new AbbrevClassProduced{ ClassProducedId = 0, ClassProducedTrans = "", AppTypeId=0});
            

            var model = new AdminBlendsInDirtEditViewModel
                {
                    comp = thisComp,
                    crops = crops,
                    countries = countryList,
                    stateProvinces = states,
                    classSeeds = classes,
                };
            
            return model;
        }
    }    
}
