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
     
    public class AdminChargesSearchViewModel
    {
        public List<Charges> charges { get; set; }                     
       
        
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime beginDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime endDate { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:d}")]
        public DateTime? reportDate { get; set; }

                       
        public static async Task<AdminChargesSearchViewModel> Create(CCIAContext _dbContext, AdminChargesSearchViewModel vm)
        {               
            
            if(vm.beginDate != DateTime.MinValue)
            {
                
                var viewModel = new AdminChargesSearchViewModel
                {
                    charges = await _dbContext.Charges
                        .Include(c => c.Organization)
                        .Where(c => !c.HoldCheck && c.Deletecharge == 0 && 
                        c.DateApplied == null && !c.Correction &&
                        c.DateEntered.Value.Date <= vm.endDate && c.DateEntered.Value.Date >= vm.beginDate).ToListAsync(),
                    beginDate = vm.beginDate,
                    endDate = vm.endDate,
                    reportDate = vm.reportDate,
                };  
                return viewModel;


            }
            var freshModel = new AdminChargesSearchViewModel
            {
                charges = new List<Charges>(),
                beginDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1),
                endDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month + 1, 1).AddDays(-1),
                reportDate = vm.reportDate.HasValue ? vm.reportDate : null,
            };           

            return freshModel;
        }

                
    } 

     
    
}
