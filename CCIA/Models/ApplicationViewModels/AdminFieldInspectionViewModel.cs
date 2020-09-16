using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Models.DetailsViewModels
{   
    public class AdminFieldInspectionViewModel 
    {
        public FieldInspection FI { get; set; }
        
        public bool potatoApp { get; set; }            

        public List<CCIAEmployees> Employees { get; set; }

        
        public static async Task<AdminFieldInspectionViewModel> Create(CCIAContext _dbContext, int fiId)
        {         
            var fi = await _dbContext.FieldInspection.Where(f => f.Id == fiId).FirstAsync();             
            var viewModel = new AdminFieldInspectionViewModel
            {
                FI = fi,
                potatoApp = await _dbContext.Applications.Where(a => a.Id == fi.AppId).AnyAsync(a => a.AppType == "PO"),
                Employees = await _dbContext.CCIAEmployees.Where(e => e.FieldInspector == true).ToListAsync(),
            };           

            return viewModel;
        }

        
    } 
    
}
