using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CCIA.Helpers;
using CCIA.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CCIA.Models.IndexViewModels;

namespace CCIA.Controllers
{
    public class MyCustomersController : SuperController
    {
        private readonly CCIAContext _dbContext;

        public MyCustomersController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index(bool isFromDelete = false, string name = null)
        {          
            if (isFromDelete)
                Message = name + " has been successfully deleted.";

            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();        
            var model = await MyCustomersIndexViewModel.Create(_dbContext, orgId);            
            return View(model);
        }

        // GET: Application/Create
        public async Task<ActionResult> Create()
        {
            var model = new MyCustomers();            
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(MyCustomers myCustomer)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();

            myCustomer.OrganizationId = orgId;

            // These 3 initializations are needed somehow for dbContext.Add() to work
            // country code can't be null and dates are invalid errors
            myCustomer.Country.Code = "";
            myCustomer.County.DateModified = DateTime.Now;
            myCustomer.State.DateModified = DateTime.Now;

            // check ModelState before saving the changes
            if (ModelState.IsValid) {
                _dbContext.Add(myCustomer);
                await _dbContext.SaveChangesAsync();
            } else {
                ErrorMessage = "Something went wrong.";
                return View(myCustomer);
            }

            return RedirectToAction(nameof(Details), new { id = myCustomer.Id, isFromCreate = true });
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id, bool isFromEdit = false, bool isFromCreate = false)
        {
            var model = await _dbContext.MyCustomers.Where(a => a.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            if (isFromEdit)
                Message = "Edit Successful";

            if (isFromCreate)
                Message = model.Name +" has been successfully created.";

            return View(model);
        }

        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _dbContext.MyCustomers.Where(a => a.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            return View(model);
        }

        // POST: Application/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, MyCustomers myCustomer)
        {
            // get the MyCustomer from database
            var myCustomerToEdit = await _dbContext.MyCustomers.Where(m => m.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            // if it's null, display error message.
            if (myCustomerToEdit == null) {
                ErrorMessage = "Customer not found. Please try again.";
                return RedirectToAction(nameof(Index));
            }

            // if MyCustomer does exist in database. then update value
            myCustomerToEdit.Name = myCustomer.Name;
            myCustomerToEdit.Address1 = myCustomer.Address1;
            myCustomerToEdit.Address2 = myCustomer.Address2;
            myCustomerToEdit.City = myCustomer.City;
            myCustomerToEdit.County.CountyName = myCustomer.County.CountyName;
            myCustomerToEdit.State.StateProvinceName = myCustomer.State.StateProvinceName;
            myCustomerToEdit.Country.Name = myCustomer.Country.Name;
            myCustomerToEdit.Zip = myCustomer.Zip;
            myCustomerToEdit.Phone = myCustomer.Phone;
            myCustomerToEdit.Email = myCustomer.Email;

            // check ModelState before saving the changes
            if(ModelState.IsValid) {
                await _dbContext.SaveChangesAsync();
            } else {
                ErrorMessage = "Something went wrong.";
                return View(myCustomer);
            }

            return RedirectToAction(nameof(Details), new { id = myCustomer.Id, isFromEdit = true });
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Delete(int id)
        {
            var myCustomerToDelete = await _dbContext.MyCustomers.Where(m => m.Id == id)
                .FirstOrDefaultAsync();

            string name = myCustomerToDelete.Name;

            if (ModelState.IsValid)
            {
                _dbContext.Remove(myCustomerToDelete);
                await _dbContext.SaveChangesAsync();
            }
            else
            {
                ErrorMessage = "Something went wrong.";
                return View(myCustomerToDelete);
            }

            return RedirectToAction(nameof(Index), new { isFromDelete = true, name });
        }
    }
}