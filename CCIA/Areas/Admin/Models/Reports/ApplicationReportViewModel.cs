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
     
    public class AdminApplicationReportViewModel
    {
       // public List<VarFull> varieties { get; set; }

       public List<AbbrevAppType> appTypes {get; set; }

       public string appTypeReport { get; set; }

        public List<Crops> crops { get; set; }
        
        [Display(Name="Crop(s)")]
        public List<int> cropsReport { get; set; }

        public List<int> certYears { get; set; }

        public int certYearReport { get; set; }

        public int varietyIdReport { get; set; }

        public List<County> counties { get; set; }

        public List<int> countiesReport { get; set; }

        public List<string> reportTypes { get; set; }

        public string reportType { get; set; }
      
               
        public static async Task<AdminApplicationReportViewModel> Create(CCIAContext _dbContext)
        {  
            var apptypes = await _dbContext.AbbrevAppType.OrderBy(a => a.AppTypeTrans).ToListAsync();
            apptypes.Insert(0, new AbbrevAppType{ AppTypeId = 0, AppTypeTrans = "Any"});
           
                       
            var model = new AdminApplicationReportViewModel
            {                 
                crops = await _dbContext.Crops.OrderBy(c => c.Crop).ThenBy(c => c.CropKind).ToListAsync(), 
                appTypes = apptypes,
                certYears = CertYearFinder.certYearList,
                counties = await _dbContext.County.Where(c => c.StateProvinceId == 102).OrderBy(c => c.Name).ToListAsync(),
                reportTypes = new List<string> {  "By Crop", "By County", "By Crop and County"}, 
            };

            return model;
        }

        
    }    
    
}
