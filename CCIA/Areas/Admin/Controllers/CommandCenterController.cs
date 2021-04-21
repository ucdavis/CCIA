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
    public class CommandCenterController : AdminController              
    {
        private readonly CCIAContext _dbContext;
        public CommandCenterController(CCIAContext dbContext)
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

        public IActionResult NewEmployee()
        {
            var model = new CCIAEmployees();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> NewEmployee(CCIAEmployees employee)
        {
            var newEmployee = new CCIAEmployees();

            newEmployee.Id = employee.Id;
            newEmployee.FirstName = employee.FirstName;
            newEmployee.LastName = employee.LastName;
            newEmployee.KerberosId = employee.KerberosId;
            newEmployee.UCDMaildID = employee.UCDMaildID;
            newEmployee.CampusPhone = employee.CampusPhone;
            newEmployee.CellPhone = employee.CellPhone;
            newEmployee.Current = employee.Current;
            newEmployee.CCIAAccess = employee.CCIAAccess;
            newEmployee.CoreStaff = employee.CoreStaff;
            newEmployee.FieldInspector = employee.FieldInspector;
            newEmployee.ConditionerStatusUpdate = employee.ConditionerStatusUpdate;
            newEmployee.Admin = employee.Admin;
            newEmployee.EditVarieties = employee.EditVarieties;
            newEmployee.SeedLotInform = employee.SeedLotInform;
            newEmployee.SeasonalEmployee = employee.SeasonalEmployee;
            newEmployee.BillingAccess = employee.BillingAccess;
            newEmployee.NewTag = employee.NewTag;
            newEmployee.TagPrint = employee.TagPrint;
            newEmployee.HeritageGrainQA = employee.HeritageGrainQA;
            newEmployee.PrevarietyGermplasm = employee.PrevarietyGermplasm;
            newEmployee.OECDInvoicePrinter =employee.OECDInvoicePrinter;

            if(ModelState.IsValid){ 
                _dbContext.Add(newEmployee);                              
                await _dbContext.SaveChangesAsync();
                Message = "Employee Added";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(employee); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = newEmployee.Id });

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

        [HttpPost]
        public async Task<IActionResult> EditEmployee(string id, CCIAEmployees employee)
        {
            var employeeToUpdate = await _dbContext.CCIAEmployees.Where(e => e.Id == id).FirstOrDefaultAsync();
             if(employeeToUpdate == null)
            {
                ErrorMessage = "Employee not found";
                return RedirectToAction(nameof(Employees));
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.KerberosId = employee.KerberosId;
            employeeToUpdate.UCDMaildID = employee.UCDMaildID;
            employeeToUpdate.CampusPhone = employee.CampusPhone;
            employeeToUpdate.CellPhone = employee.CellPhone;
            employeeToUpdate.Current = employee.Current;
            employeeToUpdate.CCIAAccess = employee.CCIAAccess;
            employeeToUpdate.CoreStaff = employee.CoreStaff;
            employeeToUpdate.FieldInspector = employee.FieldInspector;
            employeeToUpdate.ConditionerStatusUpdate = employee.ConditionerStatusUpdate;
            employeeToUpdate.Admin = employee.Admin;
            employeeToUpdate.EditVarieties = employee.EditVarieties;
            employeeToUpdate.SeedLotInform = employee.SeedLotInform;
            employeeToUpdate.SeasonalEmployee = employee.SeasonalEmployee;
            employeeToUpdate.BillingAccess = employee.BillingAccess;
            employeeToUpdate.NewTag = employee.NewTag;
            employeeToUpdate.TagPrint = employee.TagPrint;
            employeeToUpdate.HeritageGrainQA = employee.HeritageGrainQA;
            employeeToUpdate.PrevarietyGermplasm = employee.PrevarietyGermplasm;
            employeeToUpdate.OECDInvoicePrinter =employee.OECDInvoicePrinter;


            if(ModelState.IsValid){                               
                await _dbContext.SaveChangesAsync();
                Message = "Employee Updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(employee); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = employeeToUpdate.Id });

        }
       
    }
}
