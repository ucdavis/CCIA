using CCIA.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Services
{
    public interface INotificationService
    {
        Task ApplicationAccepted(Applications app);

        Task ApplicationSubmitted(Applications app);

        Task ApplicationRenewed(Applications app);

        Task ApplicationRenewNoSeed(Applications app);

        Task ApplicationRenewalCancelled(RenewFields renew);

        Task ApplicationFIRComplete(Applications app);

        Task SeedLotAccepted(Seeds seed);

        Task SeedLotSubmitted(Seeds seed);

        Task BlendRequestApproved(BlendRequests blend);

        Task TagApproved(Tags tag);

        Task TagApprovedForConditionerPrint(Tags tag);

        Task TagPrinted(Tags tag);

        Task TagFiled(Tags tag);

        Task TagSubmitted(Tags tag);

        Task OrgCreated(Organizations org);

        Task OrgUpdated(Organizations org);

        Task OECDCharged(OECD oecd);

    }

    public class NotificationService : INotificationService
    {
        private readonly CCIAContext _dbContext;
        

        public NotificationService(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task ApplicationAccepted(Applications app)
        {
           
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();           
            // Don't email org. Ask orgs to register that email as employee and assign roles as appropriate
            //users.Add(await _dbContext.Organizations.Where(o => o.Id == app.ApplicantId && !string.IsNullOrWhiteSpace(o.Email)).Select(o => o.Email).FirstOrDefaultAsync());
            users = users.Distinct().ToList();
            
            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = "Application accepted"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationSubmitted(Applications app)
        {
            var assignments = await _dbContext.CropAssignments.Where(c => c.CropId == app.CropId).Select(c => c.EmployeeId).ToListAsync();
            var inspectors = await _dbContext.CCIAEmployees.Where(e => assignments.Contains(e.Id)).Distinct().ToListAsync();            
            
            foreach (var employee in inspectors)
            {                
                var notification = new Notifications
                {
                    Email = employee.Email,
                    AppId = app.Id,
                    Message = "Application accepted",
                    IsAdmin = true,
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationRenewed(Applications app)
        {
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = $"Application {app.PaperAppNum} Renewed; New AppID: {app.Id}"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationRenewNoSeed(Applications app)
        {
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = $"Application {app.PaperAppNum} Renewed (no seed); New AppID: {app.Id}"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationRenewalCancelled(RenewFields renew)
        {
            var app = await _dbContext.Applications.Where(a => a.Id == renew.AppId).FirstOrDefaultAsync();
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = $"Application {app.PaperAppNum} Renewal Canceled"
                };
                _dbContext.Notifications.Add(notification);      
            }

        }

        public async Task ApplicationFIRComplete(Applications app)
        {
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = "Field Inspection Complete"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task SeedLotAccepted(Seeds seed)
        {
            var users = await _dbContext.Contacts.Where(c => (c.Id == seed.UserEntered.Value || (c.SeedNotices && c.OrgId == seed.ApplicantId || (c.SeedNotices && c.OrgId == seed.ConditionerId))) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    SID = seed.Id,
                    Message = "Seed Lot Accepted"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task SeedLotSubmitted(Seeds seed)
        {
            var assignments = await _dbContext.CCIAEmployees.Where(e => e.SeedLotInform).ToListAsync();            
            
            foreach (var employee in assignments)
            {                
                var notification = new Notifications
                {
                    Email = employee.Email,
                    SID = seed.Id,
                    Message = "Seed lot submitted",
                    IsAdmin = true,
                };
                _dbContext.Notifications.Add(notification);      
            }

        }

        public async Task BlendRequestApproved(BlendRequests blendRequest)
        {
            var users = await _dbContext.Contacts.Where(c => (c.Id == blendRequest.UserEntered || (c.BlendNotices && c.OrgId == blendRequest.ConditionerId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    BlendId = blendRequest.Id,
                    Message = "Blend Request Approved"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task TagApproved(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.TagPrint && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    TagId = tag.Id,
                    Message = "Tag Approved and waiting to be printed"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task TagApprovedForConditionerPrint(Tags tag)
        {
             var users = await _dbContext.Contacts.Where(c => (c.Id == tag.UserEntered.Value || (c.TagNotices && c.OrgId == tag.TaggingOrg )) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    TagId = tag.Id,
                    Message = "Tag request Approved for conditioner printing"
                };
                _dbContext.Notifications.Add(notification);      
            }            
        }


        public async Task TagPrinted(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.TagPrint && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    TagId = tag.Id,
                    Message = "Tag printed and shipped"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

         public async Task TagSubmitted(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewTag && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    TagId = tag.Id,
                    Message = "Tag submitted by conditioner"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task TagFiled(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewTag && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    TagId = tag.Id,
                    Message = "Tag filed and now complete"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OrgCreated(Organizations org)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.Admin && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    OrgId = org.Id,
                    Message = "Organization created"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OrgUpdated(Organizations org)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.Admin && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    OrgId = org.Id,
                    Message = "Organization updated"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OECDCharged(OECD oecd)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.OECDInvoicePrinter && !string.IsNullOrEmpty(e.UCDMaildID)).Select(e => e.Email).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    OecdId = oecd.Id,
                    Message = "OECD Certificate printed/charged"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }
    }

    
}