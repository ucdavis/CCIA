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
    }

    
}