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

        public ActionResult Index()
        {     
           return View();
        }
       
        
       
    }
}
