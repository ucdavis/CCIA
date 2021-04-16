using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdministrationController : AdminController              
    {
        private readonly CCIAContext _dbContext;
        public AdministrationController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IActionResult Index()
        {            
            return View();
        }  

        public async Task<IActionResult> Employees()
        {
            var model = await _dbContext.CCIAEmployees.ToListAsync();
            return View(model);
        }  

        public async Task<IActionResult> EmployeeDetails(string id)    
        {
            var model = await _dbContext.CCIAEmployees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Employee not found";
                return RedirectToAction(nameof(Employees));
            }
            return View(model);
        }

        public async Task<IActionResult> EditEmployee(string id)
        {
            var model = await _dbContext.CCIAEmployees.Where(e => e.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Employee not found";
                return RedirectToAction(nameof(Employees));
            }
            return View(model);
        }
       
    }
}
