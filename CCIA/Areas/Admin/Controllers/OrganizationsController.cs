using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CCIA.Models;
using Microsoft.AspNetCore.Authorization;
using CCIA.Services;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CCIA.Controllers
{
    public class OrganizationsController : AdminController
    {

        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;

        public OrganizationsController(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }

        // TODO: Add Roles - Seasonal Field get nothing in here. View (can edit phone, fax, website). EditOrg has full edit & create. CondStatus allows you to update conditioner status settings.

        public async Task<IActionResult> Index(string term = "")
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return View(null);
            }
            var model = await _helper.FullOrg().Where(o => EF.Functions.Like(o.Id.ToString(), "%" + term + "%") || EF.Functions.Like(o.Name, "%" + term + "%")).ToListAsync();

            return View(model);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await AdminOrgDetailsViewModel.Create(_dbContext, _helper, id);            
            if(model.org == null)
            {
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> EditStatus(int id)
        {
            var model = await _dbContext.CondStatus.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Status entry not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStatus(int id, CondStatus update)
        {
            var statusToUpdate = await _dbContext.CondStatus.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(statusToUpdate == null || update == null || statusToUpdate.Id != update.Id)
            {
                ErrorMessage = "Status not found or not able to update";
                return RedirectToAction(nameof(Index));
            }

            statusToUpdate.Status = update.Status;
            statusToUpdate.AllowPretag = update.AllowPretag;
            statusToUpdate.PrintSeries = update.PrintSeries;
            statusToUpdate.RequestCciaPrintSeries = update.RequestCciaPrintSeries;
            statusToUpdate.DatePretagApproved = update.DatePretagApproved;
            statusToUpdate.DateUpdated = DateTime.Now;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Status Updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(update); 
            }

            return RedirectToAction(nameof(Details), new { id = statusToUpdate.OrgId });  
        }

        public ActionResult NewStatus(int id)
        {
            var model = new CondStatus();
            model.OrgId = id;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> NewStatus(int id, CondStatus newStatus)
        {
            var statusToCreate = new CondStatus();
            statusToCreate.Year = newStatus.Year;
            statusToCreate.Status = newStatus.Status;
            statusToCreate.AllowPretag = newStatus.AllowPretag;
            statusToCreate.PrintSeries = newStatus.PrintSeries;
            statusToCreate.RequestCciaPrintSeries = newStatus.RequestCciaPrintSeries;
            statusToCreate.DatePretagApproved = newStatus.DatePretagApproved;
            statusToCreate.DateUpdated = DateTime.Now;
            statusToCreate.OrgId = newStatus.OrgId;

             if(ModelState.IsValid){
                 _dbContext.Add(statusToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Status Created";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(newStatus); 
            }

            return RedirectToAction(nameof(Details), new { id = newStatus.OrgId }); 
        }

        public async Task<IActionResult> AddAddress(int id)
        {
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, 0, id);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            var addressToAdd = new Address();
            var newAddress = vm.address;
            var org = await _dbContext.Organizations.Where(o => o.Id == vm.OrgId).FirstAsync();

             if(newAddress.CountyId != 0)
            {
                var county = await _dbContext.County.Where(c => c.CountyId == newAddress.CountyId).FirstAsync();
                org.District = county.District;
                org.CountyId = county.CountyId;
                addressToAdd.CountyId = newAddress.CountyId;                
            } 

            addressToAdd.Address1 = newAddress.Address1;
            addressToAdd.Address2 = newAddress.Address2;
            addressToAdd.Address3 = newAddress.Address3;
            addressToAdd.City = newAddress.City;
            addressToAdd.CountryId = newAddress.CountryId;
            addressToAdd.DateModified = DateTime.Now;
            addressToAdd.PostalCode = newAddress.PostalCode;
            addressToAdd.StateProvinceId = newAddress.StateProvinceId;
            addressToAdd.UserModified =  User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){    
                _dbContext.Add(addressToAdd);           
                await _dbContext.SaveChangesAsync();
                org.AddressId = addressToAdd.Id;
                await _dbContext.SaveChangesAsync();
                Message = "Address added";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }
            return RedirectToAction(nameof(Details), new { id = vm.OrgId });
        }

        public async Task<IActionResult> EditAddress(int id)
        {
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
            if(model.address == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            var addressToEdit = await _dbContext.Address.Where(a => a.Id == id).FirstOrDefaultAsync();
            var update = vm.address;
            if(addressToEdit == null || update == null)
            {
                ErrorMessage = "Address not found, no update!";
                return RedirectToAction(nameof(Index));
            }

            if(addressToEdit.CountyId != update.CountyId)
            {
                var county = await _dbContext.County.Where(c => c.CountyId == update.CountyId).FirstAsync();
                var org = await _dbContext.Organizations.Where(o => o.Id == vm.OrgId).FirstAsync();
                if(update.CountyId != 0)
                {
                    org.District = county.District;
                    org.CountyId = county.CountyId;
                    addressToEdit.CountyId = update.CountyId;
                } else
                {
                    org.District = null;
                    org.CountyId = null;
                    addressToEdit.CountyId = null;
                }                
            }

            addressToEdit.Address1 = update.Address1;
            addressToEdit.Address2 = update.Address2;
            addressToEdit.Address3 = update.Address3;
            addressToEdit.City = update.City;
            addressToEdit.CountryId = update.CountryId;
            addressToEdit.DateModified = DateTime.Now;
            addressToEdit.PostalCode = update.PostalCode;
            addressToEdit.StateProvinceId = update.StateProvinceId;
            addressToEdit.UserModified =  User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){               
                await _dbContext.SaveChangesAsync();
                Message = "Address Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Details), new { id = vm.OrgId });

        }

        [HttpPost]
        public async Task<IActionResult> AddMapPermission (int org_Id, string MapName)
        {
            var org = await _dbContext.Organizations.Where(o => o.Id == org_Id).AnyAsync();
            if(!org)
            {
                ErrorMessage = "Org not found";
                return RedirectToAction(nameof(Index));
            }
            var permissions = await _dbContext.OrgMaps.Where(p => p.OrgId == org_Id && p.Map == MapName).FirstOrDefaultAsync();
            if(permissions != null)
            {
                permissions.Allow = true; 
                Message = "Permission updated";              ;
            } else
            {
                permissions = new OrgMaps();
                permissions.Map = MapName;
                permissions.OrgId = org_Id;
                permissions.Allow = true;
                _dbContext.Add(permissions); 
                Message = "Permssion created";               
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = org_Id });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMapPermissions (int mapPermission)
        {
            var permission = await _dbContext.OrgMaps.Where(p => p.Id == mapPermission).FirstOrDefaultAsync();
            permission.Allow = !permission.Allow;
            await _dbContext.SaveChangesAsync();
            Message = "Perission toggled";
            return RedirectToAction(nameof(Details), new { id = permission.OrgId });
        }

        [HttpPost]
        public async Task<IActionResult> AddMapCropPermission (int org_Id, int CropId)
        {
            var org = await _dbContext.Organizations.Where(o => o.Id == org_Id).AnyAsync();
            if(!org)
            {
                ErrorMessage = "Org not found";
                return RedirectToAction(nameof(Index));
            }
            var permissions = await _dbContext.OrgMapCrops.Where(p => p.OrgId == org_Id && p.CropId == CropId).FirstOrDefaultAsync();
            if(permissions != null)
            {
                permissions.Allow = true; 
                Message = "Permission updated";              ;
            } else
            {
                permissions = new OrgMapCrops();
                permissions.CropId = CropId;
                permissions.OrgId = org_Id;
                permissions.Allow = true;
                _dbContext.Add(permissions); 
                Message = "Permssion created";               
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(Details), new { id = org_Id });
        }

        [HttpPost]
        public async Task<IActionResult> ToggleMapCropPermissions (int mapPermission)
        {
            var permission = await _dbContext.OrgMapCrops.Where(p => p.Id == mapPermission).FirstOrDefaultAsync();
            permission.Allow = !permission.Allow;
            await _dbContext.SaveChangesAsync();
            Message = "Perission toggled";
            return RedirectToAction(nameof(Details), new { id = permission.OrgId });
        }

        public async Task<IActionResult> EmployeeDetails (int id)
        {
            var employee = await _helper.FullContact().Where(c => c.Id == id).FirstOrDefaultAsync();
            if(employee == null)
            {
                ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        public async Task<IActionResult> EditEmployeeAddress(int id)
        {
            var employeeAddress = await AdminContactAddressEditCreateViewModel.Create(_dbContext, id);
             if(employeeAddress.address == null)
            {
                ErrorMessage = "Employee Address not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeAddress);
        }

        [HttpPost]
        public async Task<IActionResult> EditEmployeeAddress(int id, AdminContactAddressEditCreateViewModel vm)
        {
            var employeeAddressToUpdate = await _dbContext.ContactAddress.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefaultAsync();
            var updateAddress = vm.address;
            if(employeeAddressToUpdate == null)
            {
                ErrorMessage = "Employee Address not found!";
                return RedirectToAction(nameof(Index));
            }

            employeeAddressToUpdate.Mailing = updateAddress.Mailing;
            employeeAddressToUpdate.Billing = updateAddress.Billing;
            employeeAddressToUpdate.Delivery = updateAddress.Delivery;
            employeeAddressToUpdate.PhysicalLoc = updateAddress.PhysicalLoc;
            employeeAddressToUpdate.Address.Address1 = updateAddress.Address.Address1;
            employeeAddressToUpdate.Address.Address2 = updateAddress.Address.Address2;
            employeeAddressToUpdate.Address.Address3 = updateAddress.Address.Address3;
            employeeAddressToUpdate.Address.City = updateAddress.Address.City;
            employeeAddressToUpdate.Address.CountryId = updateAddress.Address.CountryId;
            employeeAddressToUpdate.Address.CountyId = updateAddress.Address.CountyId;
            employeeAddressToUpdate.Address.StateProvinceId = updateAddress.Address.StateProvinceId;
            employeeAddressToUpdate.Address.PostalCode = updateAddress.Address.PostalCode;
            employeeAddressToUpdate.DateModified = DateTime.Now;

            if(ModelState.IsValid){               
                await _dbContext.SaveChangesAsync();
                Message = "Address Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminContactAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = employeeAddressToUpdate.ContactId });            
        }

        public async Task<IActionResult> DeleteEmployeeAddress(int id)
        {
            var employeeAddress = await _dbContext.ContactAddress.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefaultAsync();
             if(employeeAddress == null)
            {
                ErrorMessage = "Employee Address not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeAddress);

        }

        [HttpPost]
        public async Task<IActionResult> ConfirmDeleteEmployeeAddress(int id, string Delete)
        {   
            var employeeAddress = await _dbContext.ContactAddress.Where(c => c.Id == id).FirstOrDefaultAsync();
             if(employeeAddress == null)
            {
                ErrorMessage = "Employee Address not found!";
                return RedirectToAction(nameof(Index));
            }
            var contactId = employeeAddress.ContactId;
            if(Delete == "Delete")
            {
                _dbContext.Remove(employeeAddress);
                await _dbContext.SaveChangesAsync();
                Message = "Address removed!";               
            }           

            return RedirectToAction(nameof(EmployeeDetails), new {id = contactId});
        }

        public async Task<IActionResult> AddEmployeeAddress (int id)
        {
            var employeeAddress = await AdminContactAddressEditCreateViewModel.Create(_dbContext, 0, id);            
            return View(employeeAddress);
        }

        [HttpPost]
        public async Task<IActionResult> AddEmployeeAddress(int id, AdminContactAddressEditCreateViewModel vm)
        {
            var newContactAddress = new ContactAddress();
            var newAddress = new Address();

            newContactAddress.ContactId = id;
            var updateAddress = vm.address;
            
            newContactAddress.Mailing = updateAddress.Mailing;
            newContactAddress.Billing = updateAddress.Billing;
            newContactAddress.Delivery = updateAddress.Delivery;
            newContactAddress.PhysicalLoc = updateAddress.PhysicalLoc;
            newContactAddress.DateModified = DateTime.Now;

            newAddress.Address1 = updateAddress.Address.Address1;
            newAddress.Address2 = updateAddress.Address.Address2;
            newAddress.Address3 = updateAddress.Address.Address3;
            newAddress.City = updateAddress.Address.City;
            newAddress.CountryId = updateAddress.Address.CountryId;
            newAddress.CountyId = updateAddress.Address.CountyId;
            newAddress.StateProvinceId = updateAddress.Address.StateProvinceId;
            newAddress.PostalCode = updateAddress.Address.PostalCode;

            newContactAddress.Address = newAddress;

            if(ModelState.IsValid){               
                _dbContext.Add(newContactAddress);
                await _dbContext.SaveChangesAsync();
                Message = "Address Added";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminContactAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = id });
        }

       
       
    }
}
