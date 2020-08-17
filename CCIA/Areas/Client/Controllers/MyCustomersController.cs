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

namespace CCIA.Controllers.Client
{
    public class MyCustomersController : ClientController
    {
        private readonly CCIAContext _dbContext;

        public MyCustomersController(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // GET: Application
        public async Task<IActionResult> Index()
        {          
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();        
            var model = await MyCustomersIndexViewModel.Create(_dbContext, orgId);            
            return View(model);
        }

        // GET: Application/Create
        public async Task<ActionResult> Create()
        {
            var model = await MyCustomerViewModel.Create(_dbContext);
            return View(model);
        }

        // POST: Application/Create
        [HttpPost]
        public async Task<ActionResult> Create(MyCustomerViewModel model)
        {
            var orgId = await _dbContext.Contacts.Where(c => c.Id == 1).Select(c => c.OrgId).SingleAsync();
            var myCustomer = new MyCustomers();
            myCustomer.OrganizationId = orgId;

            myCustomer.Name = model.MyCustomer.Name;
            myCustomer.Address1 = model.MyCustomer.Address1;
            myCustomer.Address2 = model.MyCustomer.Address2;
            myCustomer.City = model.MyCustomer.City;
            myCustomer.CountryId = model.MyCustomer.CountryId;

            // 10 is Canada
            // 58 is United States
            // if Canada or United states is selected, then state should be filled
            if (myCustomer.CountryId == 10 || myCustomer.CountryId == 58) {
                myCustomer.StateId = model.MyCustomer.StateId;
            } else {
                myCustomer.StateId = 0;
            }

            // if United States and California are selected, then county should be filled
            if (myCustomer.CountryId == 58 && myCustomer.StateId == 102) {
                myCustomer.CountyId = model.MyCustomer.CountyId;
            } else {
                myCustomer.CountyId = 0;
            }

            myCustomer.Zip = model.MyCustomer.Zip;
            myCustomer.Phone = model.MyCustomer.Phone;
            myCustomer.Email = model.MyCustomer.Email;

            // check ModelState before saving the changes
            if (ModelState.IsValid) {
                _dbContext.Add(myCustomer);
                await _dbContext.SaveChangesAsync();
                Message = $"{myCustomer.Name} has been successfully created.";
            } else {
                ErrorMessage = "Something went wrong.";
                return View(myCustomer);
            }

            return RedirectToAction(nameof(Details), new { id = myCustomer.Id });
        }

        // GET: Application/Details/5
        public async Task<IActionResult> Details(int id)
        {
            var model = await _dbContext.MyCustomers.Where(a => a.Id == id)
                .Include(e => e.State)
                .Include(e => e.County)
                .Include(e => e.Country)
                .Include(e => e.Organization)
                .FirstOrDefaultAsync();

            return View(model);
        }

        // GET: Application/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var model = await MyCustomerViewModel.Edit(_dbContext, id);
            return View(model);
        }

        // POST: Application/Edit/5
        [HttpPost]
        public async Task<ActionResult> Edit(int id, MyCustomerViewModel model)
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
            myCustomerToEdit.Name = model.MyCustomer.Name;
            myCustomerToEdit.Address1 = model.MyCustomer.Address1;
            myCustomerToEdit.Address2 = model.MyCustomer.Address2;
            myCustomerToEdit.City = model.MyCustomer.City;
            myCustomerToEdit.CountryId = model.MyCustomer.CountryId;

            // 10 is Canada
            // 58 is United States
            // if Canada or United states is selected, then state should be filled
            if (myCustomerToEdit.CountryId == 10 || myCustomerToEdit.CountryId == 58) {
                myCustomerToEdit.StateId = model.MyCustomer.StateId;
            } else {
                myCustomerToEdit.StateId = 0;
            }

            // if United States and California are selected, then county should be filled
            if (myCustomerToEdit.CountryId == 58 && myCustomerToEdit.StateId == 102) {
                myCustomerToEdit.CountyId = model.MyCustomer.CountyId;
            } else {
                 myCustomerToEdit.CountyId = 0;
            }

            myCustomerToEdit.Zip = model.MyCustomer.Zip;
            myCustomerToEdit.Phone = model.MyCustomer.Phone;
            myCustomerToEdit.Email = model.MyCustomer.Email;

            // check ModelState before saving the changes
            if(ModelState.IsValid) {
                await _dbContext.SaveChangesAsync();
                Message = "Edit Successful";
            } else {
                ErrorMessage = "Something went wrong.";
                return View(model);
            }

            return RedirectToAction(nameof(Details), new { id });
        }

        // GET: Application/Edit/5
        public async Task<ActionResult> Delete(int id)
        {
            var myCustomerToDelete = await MyCustomerViewModel.Edit(_dbContext, id);

            return View(myCustomerToDelete);
        }

        // POST: Application/Delete/5
        [HttpPost]
        [ActionName("Delete")]
        public async Task<ActionResult> DeletePost(int id)
        {
            var myCustomerToDelete = await _dbContext.MyCustomers.Where(m => m.Id == id)
                .FirstOrDefaultAsync();
                
            if (ModelState.IsValid)
            {
                _dbContext.Remove(myCustomerToDelete);
                await _dbContext.SaveChangesAsync();
                Message =  $"{myCustomerToDelete.Name} has been successfully deleted.";
            }
            else
            {
                ErrorMessage = "Something went wrong.";
                return View(myCustomerToDelete);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}