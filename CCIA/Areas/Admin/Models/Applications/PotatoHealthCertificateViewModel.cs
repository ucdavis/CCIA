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
     
    public class AdminPotatoHealthCertificateViewModel
    {
        public Applications application { get; set; }

        public PotatoHealthCertificates certificate { get; set; }
        
               
        public static async Task<AdminPotatoHealthCertificateViewModel> Create(CCIAContext _dbContext, int id)
        {            
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
                    certificate = await _dbContext.PotatoHealthCertificates.Where(c => c.AppId == id).FirstOrDefaultAsync()
                };
            
            return model;
        }
    }    
}
