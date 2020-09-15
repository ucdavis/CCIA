using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace CCIA.Models.DetailsViewModels
{   
    public class AdminFIRViewModel 
    {
        public FieldInspectionReport fir { get; set; }
        
        public bool potatoApp { get; set; }

        public decimal? AcresApplied { get; set; }        

        public List<AbbrevClassProduced> Classes { get; set; }

        
        public static async Task<AdminFIRViewModel> Create(CCIAContext _dbContext, int appId)
        {
            var classes = await _dbContext.AbbrevClassProduced.Include(c => c.AppType).Where(c => c.AppType.Abbreviation == "PO").OrderBy(c => c.SortOrder).ToListAsync();

            classes.Insert(0, new AbbrevClassProduced {ClassProducedId = 0, ClassAbbrv = "AS", ClassProducedTrans = "As applied"});
            classes.Add(new AbbrevClassProduced {ClassProducedId = 255, ClassAbbrv = "RE", ClassProducedTrans = "Rejected"});
          
            var viewModel = new AdminFIRViewModel
            {
                fir = await _dbContext.FieldInspectionReport.Where(f => f.AppId == appId).FirstAsync(),
                potatoApp = await _dbContext.Applications.Where(a => a.Id == appId).AnyAsync(a => a.AppType == "PO"),
                AcresApplied = _dbContext.Applications.Where(a => a.Id == appId).Select(a => a.AcresApplied).SingleAsync().Result,
                Classes = classes,
            };           

            return viewModel;
        }

        
    } 
    
}
