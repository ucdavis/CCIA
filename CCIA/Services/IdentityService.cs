using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.EntityFrameworkCore;
using System.Text;
using CCIA.Models;


namespace CCIA.Services
{
    public interface IIdentityService
    {
       
        Task<CCIAEmployees> GetByKerberos(string kerb);
       
    }

    public class IdentityService : IIdentityService
    {       
        private readonly CCIAContext _context;
       


        public IdentityService(CCIAContext context)
        {           
            _context = context;            
        }

        

        public async Task<CCIAEmployees> GetByKerberos(string kerb)
        {
            var rtValue = await _context.CCIAEmployees.Where(e => e.KerberosId == kerb && e.Current).FirstOrDefaultAsync();
            return rtValue;
        }

       
    }
}