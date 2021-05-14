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
     
    public class AdminBulkSalesCertificatesEditViewModel
    {
       public BulkSalesCertificates bsc { get; set; }

       public List<Countries> countries { get; set;}  

       public List<StateProvince> stateProvinces { get; set; }

       public List<AbbrevClassSeeds> classSeeds { get; set; }

        public static async Task<AdminBulkSalesCertificatesEditViewModel> Create(CCIAContext _dbContext, int id, IFullCallService _helper)
        {    
            var thisBSC = await _helper.FullBulkSalesCertificates().Where(b => b.Id == id).FirstOrDefaultAsync(); 
           
            var states = await _dbContext.StateProvince.OrderBy(s => s.Name).ToListAsync();            
            states.Insert(0, new StateProvince{ StateProvinceId = 0, Name=""});           
            
            var countryList = await _dbContext.Countries.OrderBy(c => c.Name).ToListAsync();            
            countryList.Insert(0, new Countries{ Id = 0, Name = ""});
            
            var classes = await _dbContext.AbbrevClassSeeds.Include(c => c.AppType).OrderBy(c => c.AppType.AppTypeTrans).ThenBy(c => c.CertClass).ToListAsync();            
            classes.Insert(0, new AbbrevClassSeeds{ Id = 0, CertClass = "", Program=0});            

            var model = new AdminBulkSalesCertificatesEditViewModel
                {
                    bsc = thisBSC,
                    countries = countryList,
                    stateProvinces = states,
                    classSeeds = classes,
                };
            
            return model;
        }
    }    
}
