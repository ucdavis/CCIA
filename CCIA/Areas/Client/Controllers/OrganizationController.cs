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
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using CCIA.Services;
using System.IO;
using CCIA.Models.DetailsViewModels;
using System.Security.Claims;

namespace CCIA.Controllers.Client
{

    public class OrganizationController : ClientController
    {
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        
        private readonly INotificationService _notification;

        public OrganizationController(CCIAContext dbContext, IFullCallService helper, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _notification = notificationService;

        }

        public async Task<IActionResult> Index()
        {
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await _helper.FullOrg().Where(o => o.Id == orgId).FirstOrDefaultAsync();
            return View(model);
        }

        public async Task<IActionResult> Membership()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await _dbContext.Organizations
                .Include(o => o.Employees)
                .Include(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Where(o => o.Id == orgId).FirstOrDefaultAsync();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Membership(Organizations org)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var orgToUpdate = await _dbContext.Organizations.Where(o => o.Id == orgId).FirstOrDefaultAsync();
            
            orgToUpdate.MemberType = org.MemberType;
            orgToUpdate.MemberYear = CertYearFinder.CertYear;
            if(org.MemberType == "Voting Member" || org.MemberType == "Non-voting Member")
            {
                orgToUpdate.Member = true;
            } else
            {
                orgToUpdate.Active = false;
            }
            
            orgToUpdate.LastMemberAgreement = DateTime.Now;
            orgToUpdate.RepresentativeContactId = org.RepresentativeContactId;
            if(!orgToUpdate.MemberSince.HasValue)
            {
                orgToUpdate.MemberSince = DateTime.Now;
            }
            await _dbContext.SaveChangesAsync();
            Message = "Agreement updated.";
            return RedirectToAction("Index","Home");
        }

        public async Task<IActionResult> Edit()
        {
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var model = await _dbContext.Organizations.Include(o => o.Employees).Where(o => o.Id == orgId).FirstOrDefaultAsync();            
            if(model == null)
            {
                ErrorMessage = "Org not found!";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Organizations org)
        {
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgToUpdate = await _dbContext.Organizations.Where(o => o.Id == id).FirstOrDefaultAsync();
            if(orgToUpdate == null || orgToUpdate.Id != org.Id)
            {
                ErrorMessage = "Org not found!";
                return  RedirectToAction(nameof(Index));
            }
            

            orgToUpdate.Email = org.Email;
            orgToUpdate.Phone = org.Phone;            
            orgToUpdate.Fax = org.Fax;
            orgToUpdate.Website = org.Website;            
            orgToUpdate.DateModified = DateTime.Now;
            orgToUpdate.RepresentativeContactId = org.RepresentativeContactId;

            if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Org Updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(org); 
            }

            return RedirectToAction(nameof(Index));  

        }

        public async Task<IActionResult> AddAddress()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, 0, orgId);
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> AddAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var addressToAdd = new OrganizationAddress();
            addressToAdd.Address = new Address();
            var newAddress = vm.orgAddress;
            var org = await _dbContext.Organizations.Where(o => o.Id == orgId).FirstAsync();
            var existingAddresses = await _dbContext.OrganizationAddress.Where(oa => oa.OrgId == orgId).AnyAsync();

            if(newAddress.Address.CountyId != 0 && !existingAddresses)
            {
                var county = await _dbContext.County.Where(c => c.CountyId == newAddress.Address.CountyId).FirstAsync();
                org.District = county.District;
                org.CountyId = county.CountyId;
                addressToAdd.Address.CountyId = newAddress.Address.CountyId;
                addressToAdd.Active = true;   
                await _notification.OrgUpdated(org);
            } else
            {
                addressToAdd.Active = false;
            }
            addressToAdd.Billing = newAddress.Billing;
            addressToAdd.Mailing = newAddress.Mailing;
            addressToAdd.Delivery = newAddress.Delivery;
            addressToAdd.Physical = newAddress.Physical;
            addressToAdd.Facility = newAddress.Facility;
            addressToAdd.OrgId = vm.OrgId;
            addressToAdd.Address.Address1 = newAddress.Address.Address1;
            addressToAdd.Address.Address2 = newAddress.Address.Address2;
            addressToAdd.Address.Address3 = newAddress.Address.Address3;
            addressToAdd.Address.City = newAddress.Address.City;
            addressToAdd.Address.CountryId = newAddress.Address.CountryId;
            addressToAdd.Address.DateModified = DateTime.Now;
            addressToAdd.Address.PostalCode = newAddress.Address.PostalCode;
            addressToAdd.Address.StateProvinceId = newAddress.Address.StateProvinceId;
            addressToAdd.Address.UserModified =  User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){    
                _dbContext.Add(addressToAdd);           
                await _dbContext.SaveChangesAsync();               
                Message = "Address added";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminAddressEditCreateViewModel.Create(_dbContext, orgId);
                return View(model); 
            }
            return RedirectToAction(nameof(Index));
        }

        
        public async Task<IActionResult> EditAddress(int id)
        {
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
            if(model.orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        public async Task<IActionResult> SetActive(int id)
        {
            var orgAddress = await _dbContext.OrganizationAddress.Where(oa => oa.Id == id).FirstOrDefaultAsync();
            var org = await _dbContext.Organizations.Where(o => o.Id == orgAddress.OrgId).FirstOrDefaultAsync();
            if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            var otherAddress = await _dbContext.OrganizationAddress.Where(oa => oa.OrgId == orgAddress.OrgId && oa.Id != id).ToListAsync();
            orgAddress.Active = true;
            otherAddress.ForEach(a => a.Active = false);
            await _notification.OrgUpdated(org);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Index));

        }

        [HttpPost]        
        public async Task<IActionResult> EditAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var addressToEdit = await _dbContext.OrganizationAddress
                .Include(oa => oa.Address)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
            var org = await _dbContext.Organizations.Where(o => o.Id == orgId).FirstAsync();
            var update = vm.orgAddress;
            if(addressToEdit == null || update == null)
            {
                ErrorMessage = "Address not found, no update!";
                return RedirectToAction(nameof(Index));
            }

            if(addressToEdit.Address.CountyId != update.Address.CountyId && addressToEdit.Active)
            {
                var county = await _dbContext.County.Where(c => c.CountyId == update.Address.CountyId).FirstAsync();                
                if(update.Address.CountyId != 0)
                {
                    org.District = county.District;
                    org.CountyId = county.CountyId;
                    addressToEdit.Address.CountyId = update.Address.CountyId;
                } else
                {
                    org.District = null;
                    org.CountyId = null;
                    addressToEdit.Address.CountyId = null;
                }                
            }

            addressToEdit.Mailing = update.Mailing;
            addressToEdit.Billing = update.Billing;
            addressToEdit.Delivery = update.Delivery;
            addressToEdit.Physical = update.Physical;
            addressToEdit.Facility = update.Facility;
            addressToEdit.Address.Address1 = update.Address.Address1;
            addressToEdit.Address.Address2 = update.Address.Address2;
            addressToEdit.Address.Address3 = update.Address.Address3;
            addressToEdit.Address.City = update.Address.City;
            addressToEdit.Address.CountryId = update.Address.CountryId;
            addressToEdit.Address.DateModified = DateTime.Now;
            addressToEdit.Address.PostalCode = update.Address.PostalCode;
            addressToEdit.Address.StateProvinceId = update.Address.StateProvinceId;
            addressToEdit.Address.UserModified =  User.FindFirstValue(ClaimTypes.Name);

            if(ModelState.IsValid){     
                await _notification.OrgUpdated(org);          
                await _dbContext.SaveChangesAsync();
                Message = "Address Updated";
            } else {
                ErrorMessage = "Something went wrong.";
                var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }

            return RedirectToAction(nameof(Index));

        }

        public async Task<IActionResult> DeleteAddress(int id)
        {            
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var orgAddress = await _dbContext.OrganizationAddress.Include(c => c.Address).Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
             if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            if(orgAddress.Active)
            {
                ErrorMessage = "Cannot delete Active Address! Set another address to active first";
                return RedirectToAction(nameof(Index));
            }    
            return View(orgAddress);

        }

        [HttpPost]       
        public async Task<IActionResult> ConfirmDeleteAddress(int id, string Delete)
        {   
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var orgAddress = await _dbContext.OrganizationAddress.Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
            if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }            
            if(orgAddress.Active)
            {
                ErrorMessage = "Cannot delete Active Address! Set another address to active first";
                return RedirectToAction(nameof(Index));
            }            
            if(Delete == "Delete")
            {
                var org = await _dbContext.Organizations.Where(o => o.Id == orgAddress.OrgId).FirstOrDefaultAsync();
                await _notification.OrgUpdated(org);
                _dbContext.Remove(orgAddress);
                await _dbContext.SaveChangesAsync();
                Message = "Address removed!";               
            }           

            return RedirectToAction(nameof(Index));
        }

         public async Task<IActionResult> EmployeeDetails (int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var employee = await _helper.FullContact().Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
            if(employee == null)
            {
                ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        
        public async Task<IActionResult> NewEmployee()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var employee = new Contacts();
            employee.OrgId = orgId;
            return View(employee);
        }

        [HttpPost]
        
        public async Task<IActionResult> NewEmployee(Contacts employee)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            // TODO send email with instructions to set password
            var employeeToAdd = new Contacts();

            employeeToAdd.OrgId = orgId;
            employeeToAdd.FirstName = employee.FirstName;
            employeeToAdd.Title = employee.Title;
            employeeToAdd.BusinessPhone = employee.BusinessPhone;
            employeeToAdd.FaxNumber = employee.FaxNumber;
            employeeToAdd.Email = employee.Email;
            employeeToAdd.MiddleInitial = employee.MiddleInitial;
            employeeToAdd.FormOfAddr = employee.FormOfAddr;
            employeeToAdd.BusinessPhoneExtension = employee.BusinessPhoneExtension;
            employeeToAdd.HomePhone = employee.HomePhone;
            employeeToAdd.AllowApps = employee.AllowApps;
            employeeToAdd.LastName = employee.LastName;
            employeeToAdd.Suffix = employee.Suffix;
            employeeToAdd.MobilePhone = employee.MobilePhone;
            employeeToAdd.PagerNumber = employee.PagerNumber;
            employeeToAdd.AllowSeeds = employee.AllowSeeds;

            if(ModelState.IsValid){
                _dbContext.Add(employeeToAdd);               
                await _dbContext.SaveChangesAsync();
                Message = "Employee created";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(employee); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = employeeToAdd.Id }); 
            
        }

        
        public async Task<IActionResult> EditEmployee (int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var model = await _dbContext.Contacts.Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }

        [HttpPost]
        
        public async Task<IActionResult> EditEmployee(int id, Contacts employee)
        {     
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }       
            var employeeToUpdate = await _dbContext.Contacts.Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();

            if(employeeToUpdate == null || employeeToUpdate.Id != employee.Id)
            {
                 ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.Title = employee.Title;
            employeeToUpdate.BusinessPhone = employee.BusinessPhone;
            employeeToUpdate.FaxNumber = employee.FaxNumber;
            employeeToUpdate.Email = employee.Email;
            employeeToUpdate.MiddleInitial = employee.MiddleInitial;
            employeeToUpdate.FormOfAddr = employee.FormOfAddr;
            employeeToUpdate.BusinessPhoneExtension = employee.BusinessPhoneExtension;
            employeeToUpdate.HomePhone = employee.HomePhone;
            employeeToUpdate.AllowApps = employee.AllowApps;
            employeeToUpdate.AllowOrgUpdate = employee.AllowOrgUpdate;
            employeeToUpdate.LastName = employee.LastName;
            employeeToUpdate.Suffix = employee.Suffix;
            employeeToUpdate.MobilePhone = employee.MobilePhone;
            employeeToUpdate.PagerNumber = employee.PagerNumber;
            employeeToUpdate.AllowSeeds = employee.AllowSeeds;
            employeeToUpdate.ApplicationNotices = employee.ApplicationNotices;
            employeeToUpdate.SeedNotices = employee.SeedNotices;
            employeeToUpdate.TagNotices = employee.TagNotices;
            employeeToUpdate.OECDNotices = employee.OECDNotices;
            employeeToUpdate.BlendNotices = employee.BlendNotices;
            employeeToUpdate.BulkSalesNotices = employee.BulkSalesNotices;
            employeeToUpdate.TransferNotices = employee.TransferNotices;

            if(ModelState.IsValid){                               
                await _dbContext.SaveChangesAsync();
                Message = "Employee updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(employee); 
            }
            return RedirectToAction(nameof(EmployeeDetails), new { id = employeeToUpdate.Id }); 
            
        }

        
        public async Task<IActionResult> EditEmployeeAddress(int id)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var employeeAddress = await AdminContactAddressEditCreateViewModel.Create(_dbContext, id);
            var contact = await _dbContext.Contacts.Where(c => c.Id == employeeAddress.ContactId && c.OrgId == orgId).FirstOrDefaultAsync();

            if(employeeAddress.address == null || contact == null)
            {
                ErrorMessage = "Employee Address not found or address does not belong to one of your employees!";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeAddress);
        }

        [HttpPost]
        
        public async Task<IActionResult> EditEmployeeAddress(int id, AdminContactAddressEditCreateViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }
            var employeeAddressToUpdate = await _dbContext.ContactAddress.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefaultAsync();
            var updateAddress = vm.address;
            var contact = await _dbContext.Contacts.Where(c => c.Id == employeeAddressToUpdate.ContactId && c.OrgId == orgId).FirstOrDefaultAsync();
            if(employeeAddressToUpdate == null || contact == null)
            {
                ErrorMessage = "Employee Address not found or address does not belong to one of your employees!";
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
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }           
            
            var employeeAddress = await _dbContext.ContactAddress.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefaultAsync();
            var contact = await _dbContext.Contacts.Where(c => c.Id == employeeAddress.ContactId && c.OrgId == orgId).FirstOrDefaultAsync();
             if(employeeAddress == null || contact == null)
            {
                ErrorMessage = "Employee Address not found or address does not belong to one of your employees!";
                return RedirectToAction(nameof(Index));
            }
            return View(employeeAddress);

        }

        [HttpPost]
        
        public async Task<IActionResult> ConfirmDeleteEmployeeAddress(int id, string Delete)
        {   
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }     
            var employeeAddress = await _dbContext.ContactAddress.Where(c => c.Id == id).FirstOrDefaultAsync();
            var contact = await _dbContext.Contacts.Where(c => c.Id == employeeAddress.ContactId && c.OrgId == orgId).FirstOrDefaultAsync();
             if(employeeAddress == null || contact == null)
            {
                ErrorMessage = "Employee Address not found or address does not belong to one of your employees!";
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
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }              
            var employeeAddress = await AdminContactAddressEditCreateViewModel.Create(_dbContext, 0, id);    
            var contact = await _dbContext.Contacts.Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
             if(contact == null)
            {
                ErrorMessage = "Employee does not belong to your Org!";
                return RedirectToAction(nameof(Index));
            }        
            return View(employeeAddress);
        }

        [HttpPost]
        
        public async Task<IActionResult> AddEmployeeAddress(int id, AdminContactAddressEditCreateViewModel vm)
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            if(!(await CheckOrgPermission()))
            {
                ErrorMessage = "You do not have permission to update Org info.";
                return RedirectToAction(nameof(Index), "Home");
            }   
            var contact = await _dbContext.Contacts.Where(c => c.Id == id && c.OrgId == orgId).FirstOrDefaultAsync();
            if(contact == null)
            {
                ErrorMessage = "Employee does not belong to your Org!";
                return RedirectToAction(nameof(Index));
            }   
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
            newAddress.DateModified = DateTime.Now;
            newAddress.UserModified = User.FindFirstValue(ClaimTypes.Name);

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


        private async Task<bool> CheckOrgPermission()
        {
            var orgId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "orgId").Value);
            var contactId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "contactId").Value);
            return await _dbContext.Contacts.Where(c => c.Id == contactId && c.OrgId == orgId && c.AllowOrgUpdate).AnyAsync();
        }
    }
}