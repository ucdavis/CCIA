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
using CCIA.Helpers;
using System.Security.Cryptography;

namespace CCIA.Controllers
{
    [Authorize(Roles = "CoreStaff")]
    public class OrganizationsController : AdminController
    {

        // TODO Create org update in client area.
        private readonly CCIAContext _dbContext;
        private readonly IFullCallService _helper;
        private readonly INotificationService _notification;

        public OrganizationsController(CCIAContext dbContext, IFullCallService helper, INotificationService notificationService)
        {
            _dbContext = dbContext;
            _helper = helper;
            _notification = notificationService;
        }

        
        public async Task<IActionResult> Index(string term = "")
        {
            if(string.IsNullOrWhiteSpace(term))
            {
                return View(null);
            }
            
            var model = await _dbContext.Organizations
                .Include(o => o.Address)
                .ThenInclude(a => a.County)
                .Include(o => o.Address)
                .ThenInclude(a => a.StateProvince)
                .Where(o => EF.Functions.Like(o.Id.ToString(), "%" + term + "%") || EF.Functions.Like(o.Name, "%" + term + "%")).ToListAsync();

            return View(model);
        }

        [Authorize(Roles = "CoreStaff,Admin")]
        public async Task<IActionResult> InactiveEmployees(int id)
        {   
            var model = await _dbContext.Contacts.Where(c => c.OrgId == id && !c.Active).ToListAsync();
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

        [Authorize(Roles = "Admin")]
        public ActionResult Create ()
        {
            var model = new Organizations();
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create(Organizations org)
        {            
            var orgToCreate = new Organizations();            

            orgToCreate.Email = org.Email;
            orgToCreate.GermLab = org.GermLab;
            orgToCreate.Name = org.Name;
            orgToCreate.Phone = org.Phone;
            orgToCreate.Active = org.Active;
            orgToCreate.DiagnosticLab = org.DiagnosticLab;
            orgToCreate.Fax = org.Fax;
            orgToCreate.Website = org.Website;
            orgToCreate.AgCommissioner = org.AgCommissioner;
            orgToCreate.DateModified = DateTime.Now;
            orgToCreate.UserModified = User.FindFirstValue(ClaimTypes.Name);
            orgToCreate.Active = true;

             if(ModelState.IsValid){
                _dbContext.Add(orgToCreate);
                await _dbContext.SaveChangesAsync();
                Message = "Org Created";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(org); 
            }
            await _notification.OrgCreated(orgToCreate);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new { id = orgToCreate.Id });  

        }

        [Authorize(Roles = "CoreStaff,Admin")]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _dbContext.Organizations.Include(o => o.Employees).Where(o => o.Id == id).FirstOrDefaultAsync();            
            if(model == null)
            {
                ErrorMessage = "Org not found!";
                return  RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff,Admin")]
        public async Task<IActionResult> Edit(int id, Organizations org)
        {
            var orgToUpdate = await _dbContext.Organizations.Where(o => o.Id == id).FirstOrDefaultAsync();
            if(orgToUpdate == null || orgToUpdate.Id != org.Id)
            {
                ErrorMessage = "Org not found!";
                return  RedirectToAction(nameof(Index));
            }
            

            orgToUpdate.Email = org.Email;
            orgToUpdate.GermLab = org.GermLab;
            orgToUpdate.Name = org.Name;
            orgToUpdate.Phone = org.Phone;
            orgToUpdate.Active = org.Active;
            orgToUpdate.DiagnosticLab = org.DiagnosticLab;
            orgToUpdate.Fax = org.Fax;
            orgToUpdate.Website = org.Website;
            orgToUpdate.AgCommissioner = org.AgCommissioner;
            orgToUpdate.DateModified = DateTime.Now;
            orgToUpdate.Notes = org.Notes;
            orgToUpdate.UserModified = User.FindFirstValue(ClaimTypes.Name);
            
            if(User.IsInRole("Admin"))
            {
                orgToUpdate.MemberType = org.MemberType;
                orgToUpdate.RepresentativeContactId = org.RepresentativeContactId;
            }
            

             if(ModelState.IsValid){
                await _dbContext.SaveChangesAsync();
                Message = "Org Updated";
            } else {
                ErrorMessage = "Something went wrong.";                
                return View(org); 
            }

            return RedirectToAction(nameof(Details), new { id = org.Id });  



        }

        [Authorize(Roles = "ConditionerStatusUpdate")]
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
        [Authorize(Roles = "ConditionerStatusUpdate")]
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

        [Authorize(Roles = "ConditionerStatusUpdate")]
        public ActionResult NewStatus(int id)
        {
            var model = new CondStatus();
            model.OrgId = id;
            model.Year = CertYearFinder.ConditionerYear;

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ConditionerStatusUpdate")]
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

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAddress(int id)
        {
            var model = await AdminAddressEditCreateViewModel.Create(_dbContext, 0, id);
            
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            var addressToAdd = new OrganizationAddress();
            addressToAdd.Address = new Address();
            var newAddress = vm.orgAddress;
            var org = await _dbContext.Organizations.Where(o => o.Id == vm.OrgId).FirstAsync();
            var existingAddresses = await _dbContext.OrganizationAddress.Where(oa => oa.OrgId == vm.OrgId).AnyAsync();

             if(newAddress.Address.CountyId != 0 && !existingAddresses)
            {
                var county = await _dbContext.County.Where(c => c.CountyId == newAddress.Address.CountyId).FirstAsync();
                org.District = county.District;
                org.CountyId = county.CountyId;
                addressToAdd.Address.CountyId = newAddress.Address.CountyId;
                addressToAdd.Active = true;                
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
                var model = await AdminAddressEditCreateViewModel.Create(_dbContext, id);
                return View(model); 
            }
            return RedirectToAction(nameof(Details), new { id = vm.OrgId });
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAddress(int id)
        {
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
            if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            var otherAddress = await _dbContext.OrganizationAddress.Where(oa => oa.OrgId == orgAddress.OrgId && oa.Id != id).ToListAsync();
            orgAddress.Active = true;
            otherAddress.ForEach(a => a.Active = false);
            await _dbContext.SaveChangesAsync();

            return RedirectToAction(nameof(Details), new {id = orgAddress.OrgId});

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditAddress(int id, AdminAddressEditCreateViewModel vm)
        {
            var addressToEdit = await _dbContext.OrganizationAddress
                .Include(oa => oa.Address)
                .Where(a => a.Id == id).FirstOrDefaultAsync();
            var org = await _dbContext.Organizations.Where(o => o.Id == vm.OrgId).FirstAsync();
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

            return RedirectToAction(nameof(Details), new { id = vm.OrgId });

        }

        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var orgAddress = await _dbContext.OrganizationAddress.Include(c => c.Address).Where(c => c.Id == id).FirstOrDefaultAsync();
             if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            if(orgAddress.Active)
            {
                ErrorMessage = "Cannot delete Active Address! Set another address to active first";
                return RedirectToAction(nameof(Details), new {id = orgAddress.OrgId});
            }    
            return View(orgAddress);

        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> ConfirmDeleteAddress(int id, string Delete)
        {   
            var orgAddress = await _dbContext.OrganizationAddress.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(orgAddress == null)
            {
                ErrorMessage = "Address not found!";
                return RedirectToAction(nameof(Index));
            }
            var org = orgAddress.OrgId;
            if(orgAddress.Active)
            {
                ErrorMessage = "Cannot delete Active Address! Set another address to active first";
                return RedirectToAction(nameof(Details), new {id = org});
            }            
            if(Delete == "Delete")
            {
                _dbContext.Remove(orgAddress);
                await _dbContext.SaveChangesAsync();
                Message = "Address removed!";               
            }           

            return RedirectToAction(nameof(Details), new {id = org});
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
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
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> AddContactMapPermission (int contactId, string MapName)
        {
            var contact = await _dbContext.Contacts.Where(c => c.Id == contactId).AnyAsync();
            if(!contact)
            {
                ErrorMessage = "Contact not found";
                return RedirectToAction(nameof(Index));
            }
            var permissions = await _dbContext.ContactMaps.Where(p => p.ContactId == contactId && p.Map == MapName).FirstOrDefaultAsync();
            if(permissions != null)
            {
                permissions.Allow = true; 
                Message = "Permission updated";              ;
            } else
            {
                permissions = new ContactMaps();
                permissions.Map = MapName;
                permissions.ContactId = contactId;
                permissions.Allow = true;
                _dbContext.Add(permissions); 
                Message = "Permssion created";               
            }
            await _dbContext.SaveChangesAsync();
            return RedirectToAction(nameof(EmployeeDetails), new { id = contactId });
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> ToggleMapPermissions (int mapPermission)
        {
            var permission = await _dbContext.OrgMaps.Where(p => p.Id == mapPermission).FirstOrDefaultAsync();
            permission.Allow = !permission.Allow;
            await _dbContext.SaveChangesAsync();
            Message = "Perission toggled";
            return RedirectToAction(nameof(Details), new { id = permission.OrgId });
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> ToggleContactMapPermissions (int mapPermission)
        {
            var permission = await _dbContext.ContactMaps.Where(p => p.Id == mapPermission).FirstOrDefaultAsync();
            permission.Allow = !permission.Allow;
            await _dbContext.SaveChangesAsync();
            Message = "Perission toggled";
            return RedirectToAction(nameof(EmployeeDetails), new { id = permission.ContactId });
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
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
        [Authorize(Roles = "CoreStaff")]
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
            var employee = await AdminContactDetailsViewModel.Create(_dbContext, _helper, id);
            if(employee == null)
            {
                ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        [Authorize(Roles = "CoreStaff")]
        public IActionResult NewEmployee(int id)
        {
            var employee = new Contacts();
            employee.OrgId = id;
            return View(employee);
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> NewEmployee(int id, Contacts employee)
        {
            // TODO send email with instructions to set password
            var employeeToAdd = new Contacts();

            employeeToAdd.OrgId = id;
            employeeToAdd.Active = true;
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

        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> EditEmployee (int id)
        {
            var model = await _dbContext.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(model == null)
            {
                ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }
            return View(model);

        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> EditEmployee(int id, Contacts employee)
        {            
            var employeeToUpdate = await _dbContext.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();

            if(employeeToUpdate == null || employeeToUpdate.Id != employee.Id)
            {
                 ErrorMessage = "Employee/Contact not found!";
                return RedirectToAction(nameof(Index));
            }

            employeeToUpdate.FirstName = employee.FirstName;
            employeeToUpdate.Active = employee.Active;
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

        [Authorize(Roles = "CoreStaff")]
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
        [Authorize(Roles = "CoreStaff")]
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

        [Authorize(Roles = "CoreStaff")]
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
        [Authorize(Roles = "CoreStaff")]
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

        
        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> AddEmployeeAddress (int id)
        {
            var employeeAddress = await AdminContactAddressEditCreateViewModel.Create(_dbContext, 0, id);            
            return View(employeeAddress);
        }

        [HttpPost]
        [Authorize(Roles = "CoreStaff")]
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

        [Authorize(Roles = "CoreStaff")]
        public async Task<IActionResult> ResetPassword(int id)
        {
            var contact = await _dbContext.Contacts.Where(c => c.Id == id).FirstOrDefaultAsync();
            if(contact == null)
            {
                ErrorMessage = "Contact not found";
                return RedirectToAction(nameof(Index));
            }
            contact.Password = null;
            contact.PasswordHash = null;
            byte[] b = System.Security.Cryptography.RandomNumberGenerator.GetBytes(12);                                         
            contact.ResetPin = b;
            contact.ResetExpiration = DateTime.Now.AddDays(5);
            _notification.ResetPassword(contact);
            await _dbContext.SaveChangesAsync();
            Message = "Password reset and email instructions pending.";

            return RedirectToAction(nameof(EmployeeDetails), new { id = id});
        }

       
       
    }
}
