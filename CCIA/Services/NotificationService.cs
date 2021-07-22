using CCIA.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace CCIA.Services
{
    public interface INotificationService
    {
        Task ApplicationAccepted(Applications app);

        Task ApplicationRenewed(Applications app);

        Task ApplicationRenewNoSeed(Applications app);

        Task ApplicationRenewalCancelled(RenewFields renew);

        Task ApplicationFIRComplete(Applications app);

        Task SeedLotAccepted(Seeds seed);

    }

    public class NotificationService : INotificationService
    {
        private readonly CCIAContext _dbContext;
        

        public NotificationService(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }

        // Assume we email all Team KeyMasters & DepartmentalAdmins
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
    }

    
}