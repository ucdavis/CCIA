using CCIA.Helpers;
using CCIA.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
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

        Task ApplicationReturnedForReview(Applications app);

        Task SeedLotAccepted(Seeds seed);

        Task SeedLotSubmitted(Seeds seed);

        Task BlendRequestApproved(BlendRequests blend);

        Task BlendRequestSubmitted(BlendRequests blend);
        Task SeedTransferSubmitted(SeedTransfers request);
        Task SeedTransferResponded(SeedTransfers request);

        Task TagApproved(Tags tag);

        Task TagApprovedForConditionerPrint(Tags tag);

        Task TagPrinted(Tags tag);

        Task TagFiled(Tags tag);

        Task TagSubmitted(Tags tag);

        Task OrgCreated(Organizations org);

        Task OrgUpdated(Organizations org);

        Task OECDCharged(OECD oecd);

        Task NFCSubmitted(Tags tag);

        void ResetPassword(Contacts contact);

    }

    public class NotificationService : INotificationService
    {
        private readonly CCIAContext _dbContext;
        

        public NotificationService(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void ResetPassword(Contacts contact)
        {
            var notification = new Notifications
            {
                ContactId = contact.Id,
                Email = contact.Email,
                Message = "Password reset"
            };
            _dbContext.Notifications.Add(notification);
        }
        
        public async Task ApplicationAccepted(Applications app)
        {
           
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();                       
            users = users.Distinct().ToList();
            
            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = "Application accepted",
                    IsWeekly = true,
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationReturnedForReview(Applications app)
        {
           
            var users = await _dbContext.Contacts.Where(c => (c.Id == app.UserDataentry.Value || (c.ApplicationNotices && c.OrgId == app.ApplicantId)) && !string.IsNullOrWhiteSpace(c.Email)).Select(c => c.Email).ToListAsync();                       
            users = users.Distinct().ToList();
            
            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    AppId = app.Id,
                    Message = "Application returned for review",
                    IsWeekly = false,
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task ApplicationSubmitted(Applications app)
        {
            var assignments = await _dbContext.CropAssignments.Where(c => c.CropId == app.CropId).Select(c => c.EmployeeId).ToListAsync();
            var inspectors = await _dbContext.CCIAEmployees.Where(e => assignments.Contains(e.Id) && e.Current && !string.IsNullOrWhiteSpace(e.UCDMailID)).Distinct().ToListAsync();            
            
            foreach (var employee in inspectors)
            {                
                var notification = new Notifications
                {
                    Email = $"{employee.UCDMailID}@ucdavis.edu",
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
                    Message = $"Application {app.PaperAppNum} Renewed; New AppID: {app.Id}",
                    IsWeekly = true,
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
                    Message = $"Application {app.PaperAppNum} Renewed (no seed); New AppID: {app.Id}",
                    IsWeekly = true,
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
                    Message = $"Application {app.PaperAppNum} Renewal Canceled",
                    IsWeekly = true,
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
                    Message = "Field Inspection Complete",
                    IsWeekly = true,
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
            var assignments = await _dbContext.CCIAEmployees.Where(e => e.SeedLotInform && !string.IsNullOrWhiteSpace(e.UCDMailID) && e.Current).ToListAsync();            
            
            foreach (var employee in assignments)
            {                
                var notification = new Notifications
                {
                    Email = $"{employee.UCDMailID}@ucdavis.edu",
                    SID = seed.Id,
                    Message = "Seed lot submitted",
                    IsAdmin = true,
                };
                _dbContext.Notifications.Add(notification);      
            }

        }

        public async Task SeedTransferSubmitted(SeedTransfers request)
        {            
            var assignments = await GetSeedTransferEmployeeInformList(request);
             
            var countyComm = await _dbContext.Organizations.Where(o => !string.IsNullOrWhiteSpace(o.Email) && o.AgCommissioner && (o.CountyId == request.OriginatingCountyId || (o.CountyId == request.PurchaserCountyId && request.PurchaserStateId == 102))).Distinct().ToListAsync();
            
            foreach (var employee in assignments)
            {                
                var notification = new Notifications
                {
                    Email = $"{employee.UCDMailID}@ucdavis.edu",
                    StId = request.Id,
                    Message = "Seed Transfer request submitted",
                    IsAdmin = true,
                };
                _dbContext.Notifications.Add(notification);      
            }
            foreach (var org in countyComm)
            {                
                var notification = new Notifications
                {
                    Email = org.Email,
                    StId = request.Id,
                    Message = "Seed Transfer request submitted",
                    IsAdmin = true,
                };
                _dbContext.Notifications.Add(notification);      
            }

        }

        public async Task SeedTransferResponded(SeedTransfers request)
        {
            var assignments = await GetSeedTransferEmployeeInformList(request);            
            
            foreach (var employee in assignments)
            {                
                var notification = new Notifications
                {
                    Email = $"{ employee.UCDMailID}@ucdavis.edu",
                    StId = request.Id,
                    Message = "Seed Transfer responded by Ag Commissioner",
                    IsAdmin = false,
                };
                _dbContext.Notifications.Add(notification);      
            }
            var users = await _dbContext.Contacts.Where(c => !string.IsNullOrWhiteSpace(c.Email) && (c.Id == request.CreatedById || (c.OrgId == request.OriginatingOrganizationId && ((c.ApplicationNotices && request.ApplicationId.HasValue && request.ApplicationId > 0)  || (c.SeedNotices && request.SeedsID.HasValue && request.SeedsID > 0) || (c.BlendNotices && request.BlendId.HasValue && request.BlendId > 0))))).Select(c => c.Email).ToListAsync();

            foreach (var user in users)
            {                
                var notification = new Notifications
                {
                    Email = user,
                    StId = request.Id,
                    Message = "Seed Transfer responded by Ag Commissioner",
                    IsAdmin = false,
                };
                _dbContext.Notifications.Add(notification);      
            }

            var countyComm = await _dbContext.Organizations.Where(o => !string.IsNullOrWhiteSpace(o.Email) && o.AgCommissioner && (o.CountyId == request.OriginatingCountyId || (o.CountyId == request.PurchaserCountyId && request.PurchaserStateId == 102))).Distinct().ToListAsync();
            foreach (var org in countyComm)
            {                
                var notification = new Notifications
                {
                    Email = org.Email,
                    StId = request.Id,
                    Message = "Seed Transfer responded by Ag Commissioner",
                    IsAdmin = false,
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

        public async Task BlendRequestSubmitted(BlendRequests blend)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewBlend && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    BlendId = blend.Id,
                    Message = "Blend submitted by conditioner"
                };
                _dbContext.Notifications.Add(notification);      
            }

        }

        public async Task TagApproved(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.TagPrint && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
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
            var admins = await _dbContext.CCIAEmployees.Where(e => e.TagPrint && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    TagId = tag.Id,
                    Message = "Tag printed and shipped"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task TagSubmitted(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewTag && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    TagId = tag.Id,
                    Message = "Tag submitted by conditioner"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task NFCSubmitted(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewTag && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    TagId = tag.Id,
                    SID = tag.SeedsID,
                    IsAdmin = true,
                    Message = $"NFC SID/Tag submitted by conditioner SID:{tag.SeedsID} TID:{tag.Id}"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task TagFiled(Tags tag)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.NewTag && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    TagId = tag.Id,
                    Message = "Tag filed and now complete"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OrgCreated(Organizations org)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.Admin && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    OrgId = org.Id,
                    Message = "Organization created"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OrgUpdated(Organizations org)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.Admin && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    OrgId = org.Id,
                    Message = "Organization updated"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        public async Task OECDCharged(OECD oecd)
        {
            var admins = await _dbContext.CCIAEmployees.Where(e => e.OECDInvoicePrinter && !string.IsNullOrEmpty(e.UCDMailID) && e.Current).Select(e => e.UCDMailID).ToListAsync();

            foreach (var user in admins)
            {                
                var notification = new Notifications
                {
                    Email = $"{user}@ucdavis.edu",
                    OecdId = oecd.Id,
                    Message = "OECD Certificate printed/charged"
                };
                _dbContext.Notifications.Add(notification);      
            }
        }

        private async Task<List<CCIAEmployees>> GetSeedTransferEmployeeInformList(SeedTransfers request)
        {
            var assignments = new List<CCIAEmployees>();
            
            if(request.ApplicationId != null && request.ApplicationId > 0)
            {
                var app = await _dbContext.Applications.Where(a => a.Id == request.ApplicationId).FirstOrDefaultAsync();
                if(app.AppType == AppTypes.GrainQA.GetDisplayName())
                {
                    assignments = await _dbContext.CCIAEmployees.Where(e => e.HeritageGrainQA && e.Current && !string.IsNullOrWhiteSpace(e.UCDMailID)).ToListAsync();
                }
                if(app.AppType == AppTypes.PrevarietyGermplasm.GetDisplayName())
                {
                    assignments = await _dbContext.CCIAEmployees.Where(e => e.PrevarietyGermplasm && e.Current && !string.IsNullOrWhiteSpace(e.UCDMailID)).ToListAsync();
                }
                if(app.AppType == AppTypes.Seed.GetDisplayName())
                {
                    var inspectors = await _dbContext.CropAssignments.Where(c => c.CropId == app.CropId).Select(c => c.EmployeeId).ToListAsync();
                    assignments = await _dbContext.CCIAEmployees.Where(e => inspectors.Contains(e.Id) && e.Current && !string.IsNullOrWhiteSpace(e.UCDMailID)).Distinct().ToListAsync(); 
                }

            }
            if(request.SeedsID != null && request.SeedsID > 0)
            {
                assignments = await _dbContext.CCIAEmployees.Where(e => e.SeedLotInform && e.Current && !string.IsNullOrWhiteSpace(e.UCDMailID)).ToListAsync(); 
            }
            return assignments;
        }
    }

    
}