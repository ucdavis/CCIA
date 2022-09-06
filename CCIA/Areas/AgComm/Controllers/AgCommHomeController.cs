using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;

namespace CCIA.Controllers.AgComm
{
    public class AgCommHomeController : AgCommController
    {

        private readonly CCIAContext _dbContext;        

        public AgCommHomeController(CCIAContext dbContext)
        {
            _dbContext = dbContext;            
        }

        public async Task<IActionResult> Index()
        {    
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);       
            var p0 = new SqlParameter("@org_id", orgId);
            var model =  await _dbContext.LandingStats.FromSqlRaw($"EXEC mvc_agcomm_landstats @org_id", p0).ToListAsync();
            return View(model);
        }
       
        
       
    }
}
