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

        [Authorize(Roles = "Billing")]
        public async Task<IActionResult> Corrections()
        {
            var model = await _dbContext.Charges
                .Include(c => c.Organization)
                .Where(c => (c.Correction || c.LinkType == "Turfgrass Certificate") && c.BatchNumber == null && c.ChargeAmount != 0 && c.ChargeAmount.HasValue).ToListAsync();
            return View(model);
        }

        [Authorize(Roles = "Billing")]
        [HttpPost]
        public async Task<IActionResult> MarkCorrections()
        {
            var model = await _dbContext.Charges
                .Include(c => c.Organization)
                .Where(c => (c.Correction || c.LinkType == "Turfgrass Certificate") && c.BatchNumber == null && c.ChargeAmount != 0 && c.ChargeAmount.HasValue).ToListAsync();
            model.ForEach(c => 
                {
                    c.BatchNumber = DateTime.Now.ToShortDateString();
                    c.DateApplied = DateTime.Now;                    
                });
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Corrections));
        }



        [Authorize(Roles = "Billing")]
        public async Task<IActionResult> Charges(AdminChargesSearchViewModel vm, string submit)
        {
            switch (submit)
            {
                case "Preview" :
                    break;
                case "Download" :
                    return RedirectToAction("ExportCharges", new {beginDate = vm.beginDate, endDate = vm.endDate, reportDate = vm.reportDate});
                case "Mark" :
                    var p0 = new SqlParameter("@begin_date", System.Data.SqlDbType.DateTime);
                    p0.Value = vm.beginDate;         
                    var p1 = new SqlParameter("@end_date", System.Data.SqlDbType.DateTime);
                    p1.Value = vm.endDate;
                    var p2 = new SqlParameter("@rpt_date", System.Data.SqlDbType.DateTime);
                    p2.Value = vm.reportDate;
                    await _dbContext.Database.ExecuteSqlRawAsync($"EXEC mvc_mark_export_charges_to_iif @begin_date, @end_date, @rpt_date", p0, p1, p2); 
                    Message = "Charges marked as complete";
                    break;
            }
            var model = await AdminChargesSearchViewModel.Create(_dbContext, vm);
            return View(model);
        } 

        

        [Authorize(Roles = "Billing")]
        public async Task<IActionResult> ExportCharges(DateTime beginDate, DateTime endDate, DateTime reportDate)
        {
            var p0 = new SqlParameter("@begin_date", System.Data.SqlDbType.DateTime);
            p0.Value = beginDate;         
            var p1 = new SqlParameter("@end_date", System.Data.SqlDbType.DateTime);
            p1.Value = endDate;
            var p2 = new SqlParameter("@rpt_date", System.Data.SqlDbType.DateTime);
            p2.Value = reportDate;
            var model =  await _dbContext.ExportCharges.FromSqlRaw($"EXEC mvc_export_charges_to_iif @begin_date, @end_date, @rpt_date", p0, p1, p2).ToListAsync();

            StringBuilder sb = new StringBuilder();
            bool fl = true;
            var s = "\t";
            sb.Append("!TRNS" + s + "TRNSTYPE" + s + "DATE" + s + "ACCNT" + s + "NAME" + s + "CLASS" + s + "AMOUNT" + s + "DOCNUM" + s + "MEMO" + s + "ADDR1");
            sb.Append(Environment.NewLine);
            sb.Append("!SPL" + s + "TRNSTYPE" + s + "DATE" + s + "ACCNT" + s + "NAME" + s + "CLASS" + s + "AMOUNT" + s + "DOCNUM" + s + "MEMO" + s + "INVITEM");
            sb.Append(Environment.NewLine);
            sb.Append("!ENDTRNS" + Environment.NewLine);

            foreach(var dr in model)
            {
                if(dr.TRNS == "TRNS" && !fl)
                {
                    sb.Append("ENDTRNS");
                    sb.Append(Environment.NewLine);
                }
                if(fl)
                {
                    fl = false;
                }
                sb.Append(dr.TRNS + s);
                sb.Append(dr.TrnsType + s);
                sb.Append(dr.Date.ToShortDateString() + s);
                sb.Append(dr.Accnt + s);
                sb.Append(dr.Name + s);
                sb.Append(dr.QBClass + s);
                sb.Append(dr.Amount + s);
                sb.Append(dr.DocNum + s);
                sb.Append(dr.Memo + s);
                if(dr.TRNS == "TRNS")
                {
                    sb.Append(dr.Name + s);
                } else
                {
                    sb.Append(dr.InvItem + s);
                }
                sb.Append(Environment.NewLine);
            }

            sb.Append("ENDTRNS" + Environment.NewLine);

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
            newEmployee.UCDMailID = employee.UCDMailID;
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
            newEmployee.AdminEmailSummary  = employee.AdminEmailSummary;
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
            employeeToUpdate.UCDMailID = employee.UCDMailID;
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
            employeeToUpdate.AdminEmailSummary  = employee.AdminEmailSummary;
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
