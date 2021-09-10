using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Models
{   
     
    public class AdminPotatoHealthCertificateViewModel
    {
        public Applications application { get; set; }

        public PotatoHealthCertificates certificate { get; set; }

        public List<PotatoHealthCertificateHistory> histories { get; set; }  

        public List<PotatoHealthCertificateInspections> inspections { get; set; }    
        
               
        public static async Task<AdminPotatoHealthCertificateViewModel> Create(CCIAContext _dbContext, int id)
        {   
            var p0 = new SqlParameter("@app_id",id);         
            var model = new AdminPotatoHealthCertificateViewModel
                {
                   application = await _dbContext.Applications
                        .Include(a => a.GrowerOrganization)
                        .ThenInclude(g => g.Address)
                        .ThenInclude(a => a.StateProvince)                
                        .Include(a => a.Variety)
                        .Include(a => a.ClassProduced)
                        .Where(a => a.Id == id && a.AppType == "PO")
                        .FirstOrDefaultAsync(),
                    certificate = await _dbContext.PotatoHealthCertificates.Where(c => c.AppId == id).FirstOrDefaultAsync(),
                    histories = await _dbContext.PotatoHealthCertificateHistory.Where(h => h.AppId == id).OrderBy(h => h.ProductionYear).ToListAsync(),
                    inspections = await _dbContext.PotatoHealthCertificateInspections.FromSqlRaw($"EXEC mvc_print_po_cert_summer_readings @app_id", p0).ToListAsync(),  
                };
            
            return model;
        }
    }    
}
