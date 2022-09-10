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
using System.Text;
using Microsoft.AspNetCore.Routing;

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

        public async Task<IActionResult> Charges(AdminChargesSearchViewModel vm, string submit)
        {
            switch (submit)
            {
                case "Preview" :
                    break;
                case "Download" :
                    return RedirectToAction("ExportCharges", new {beginDate = vm.beginDate, endDate = vm.endDate, reportDate = vm.reportDate});
                    // return RedirectToAction( "ExportCharges", new RouteValueDictionary( 
                    //     new { controller = "CommandCenter", action = "ExportCharges", export = vm } ) );
                case "Mark" :
                    var model1 = await AdminChargesSearchViewModel.Create(_dbContext, vm);
                    return View(model1);
            }
            var model = await AdminChargesSearchViewModel.Create(_dbContext, vm);
            return View(model);
        } 

        public async Task<IActionResult> ExportCharges(DateTime beginDate, DateTime endDate, DateTime reportDate)
        {
            var p0 = new SqlParameter("@begin_date", System.Data.SqlDbType.DateTime);
            p0.Value = beginDate;         
            var p1 = new SqlParameter("@end_date", System.Data.SqlDbType.DateTime);
            p1.Value = endDate;
            var p2 = new SqlParameter("@rpt_date", System.Data.SqlDbType.DateTime);
            p2.Value = reportDate;
            // var model = await _dbContext.ExportCharges.FromSqlRaw($"EXEC mvc_export_charges_to_iif @begin_date, @end_date, @rpt_date", p0, p1, p2).ToListAsync();

            // var p0 = new SqlParameter("@begin_date", beginDate);
            // var p1 = new SqlParameter("@end_date", endDate);
            // var p2 = new SqlParameter("@rpt_date", reportDate);
            var model =  await _dbContext.ExportCharges.FromSqlRaw($"EXEC mvc_export_charges_to_iif @begin_date, @end_date, @rpt_date", p0, p1, p2).ToListAsync();

            StringBuilder sb = new StringBuilder();
            var s = "\t";
            sb.Append("!TRNS" + s + "TRNSTYPE" + s + "DATE" + s + "ACCNT" + s + "NAME" + s + "CLASS" + s + "AMOUNT" + s + "DOCNUM" + s + "MEMO" + s + "ADDR1");
            sb.Append("\n");
            sb.Append("!SPL" + s + "TRNSTYPE" + s + "DATE" + s + "ACCNT" + s + "NAME" + s + "CLASS" + s + "AMOUNT" + s + "DOCNUM" + s + "MEMO" + s + "INVITEM");
            sb.Append("\n");
            sb.Append("!ENDTRNS \n");

            var fileName = $"export{DateTime.Now.ToShortDateString()}.iif"; 

            byte[] byteArray = ASCIIEncoding.ASCII.GetBytes(sb.ToString());
            return File(Encoding.UTF8.GetBytes(sb.ToString()), "text/plain", fileName);

        }

        public async Task<IActionResult> Employees(bool showOnlyCurrent = true)
        {
            ViewBag.showOnlyCurrent = showOnlyCurrent;
            var model = await _dbContext.CCIAEmployees.Where(e => e.Current == showOnlyCurrent).OrderBy(e => e.LastName).ThenBy(e => e.FirstName).ToListAsync();
            return View(model);
        }  

        public async Task<IActionResult> EmployeeDetails(string id)    
        {
            var model = await _dbContext.CCIAEmployees
                .Where(e => e.Id == id)
                .Include(e => e.AssignedCrops)
                .ThenInclude(a => a.AssignedCrop)
                .FirstOrDefaultAsync();
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
            newEmployee.NewBlend = employee.NewBlend;

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
            employeeToUpdate.NewBlend = employee.NewBlend;


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
