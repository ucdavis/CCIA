using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RazorLight;

namespace CCIA.Services
{
    public interface IEmailService
    {
        Task SendWeeklyApplicationNotices(string password);        

        Task SendNotices(string password);

        Task SendWeeklyAdminNotices(string password);

    }
    


    public class EmailService : IEmailService
    {
        private readonly CCIAContext _dbContext;        

        private SmtpClient _client;

        private readonly IFullCallService _helper;

        public EmailService(CCIAContext dbContext, IFullCallService helper)
        {
            _dbContext = dbContext;
            _helper = helper;
        }
        
         private void ConfigureSMTPClient(string password)
        {
            _client = new SmtpClient("pricklypear.plantsciences.ucdavis.edu",25) {Credentials = new NetworkCredential("ccia", password), EnableSsl = true};
        }
       
        public async Task SendNotices(string password)
        {
            await SendPendingAdminSeedNotices(password);
            await SendImmediateApplicationNotices(password);
            await SendPendingSeedNotices(password);
            await SendPendingAdminAppNotices(password);            
            await SendPendingAdminNFCNotices(password);
            await SendPendingAdminTagNotices(password);
            await SendPendingBlendNotices(password);
            await SendPendingTagNotices(password);
            await SendPendingOrganizationNotices(password);
            await SendPendingOECDNotices(password);
            await SendPendingSeedTransferSubmitNotices(password);
            await SendPendingSeedTransferResponseNotices(password);
            await SendPendingPasswordResetNotices(password);
        }

        public async Task SendWeeklyAdminNotices(string password)
        {
            await SendWeeklyStaffNotices(password);
            await SendAdminNotices(password);
        }
        
        private async Task SendWeeklyStaffNotices(string password)
        {
            ConfigureSMTPClient(password);
            var model = await AdminEmailViewModel.CreateBase(_dbContext);
            foreach(var empl in model.EmployeesToEmail)
            {
               await AdminEmailViewModel.GetAppsForEmployee(model,_dbContext, _helper, empl.Id);

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Weekly Application Summary"})
                {  
                    //message.To.Add("jscubbage@ucdavis.edu");                 
                    message.To.Add(empl.Email);
                    message.Body = "Summary of applications in last week";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/AdminWeeklyNotices.cshtml", model), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
        }

        private async Task SendAdminNotices(string password)
        {
            ConfigureSMTPClient(password);
            var model = await AdminEmailViewModel.CreateBase(_dbContext);
            
            await AdminEmailViewModel.GetAppsForAdmin(model,_dbContext, _helper);
            var admins = await _dbContext.CCIAEmployees.Where(e => e.AdminEmailSummary).ToListAsync();

            using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Weekly Application Summary"})
            {  
                //message.To.Add("jscubbage@ucdavis.edu");
                foreach(var emp in admins)
                {
                   message.To.Add(emp.Email);
                }
                message.Body = "Summary of applications in last week";
                var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/AdminWeeklyNotices.cshtml", model), new ContentType(MediaTypeNames.Text.Html));
                message.AlternateViews.Add(htmlView);
                await _client.SendMailAsync(message);
            }
            
        }

        public  async Task SendWeeklyApplicationNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications =  await _dbContext.Notifications.Where(n => n.Pending && n.AppId != 0 && !n.IsAdmin && n.IsWeekly).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            var recipients = notifications.Select(n => n.Email).Distinct().ToList();           

            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA application status changes"})
                {
                   // message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An application status has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/ApplicationWeeklyNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            } 
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();          
        }

        public  async Task SendImmediateApplicationNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications =  await _dbContext.Notifications.Where(n => n.Pending && n.AppId != 0 && !n.IsAdmin && !n.IsWeekly).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            var recipients = notifications.Select(n => n.Email).Distinct().ToList();           

            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA application status changes"})
                {
                   // message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An application status has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/ApplicationWeeklyNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            } 
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();          
        }


        private async Task SendPendingAdminAppNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.AppId != 0 && n.IsAdmin).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Application Notices"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An Application has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/ApplicationWeeklyNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));                    
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        private async Task SendPendingPasswordResetNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.ContactId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            
            foreach(var notice in notifications)
            {

                var contact = await _dbContext.Contacts.Where(c => c.Id == notice.ContactId).FirstOrDefaultAsync();
                var contactAndOrg = new ContactsAndOrg
                {
                    contact = contact,
                    Org = await _dbContext.Organizations.Where(o => o.Id == contact.OrgId).FirstOrDefaultAsync()
                };
                
                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Account Password Reset Instructions"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");                    
                    message.To.Add(contact.Email);
                    message.Body = "Reset password instructions. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/PasswordReset.cshtml", contactAndOrg), new ContentType(MediaTypeNames.Text.Html));                    
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        private async Task SendPendingAdminNFCNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.SID != 0 && n.IsAdmin && n.TagId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Seed Notices"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An Seed Lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/NFCAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        private async Task SendPendingAdminSeedNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.SID != 0 && n.IsAdmin && n.TagId == 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Seed Notices"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An Seed Lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        private async Task SendPendingSeedNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.SID != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA SID status changes"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "A SID lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedClientNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   
        }

        private async Task SendPendingBlendNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.BlendId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Blend status changes"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "A Blend has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/BlendClientNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private async Task SendPendingAdminTagNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.TagId != 0 && n.IsAdmin).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Tag status changes"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "A Tag has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/TagAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private async Task SendPendingTagNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.TagId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Tag status changes"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "A Tag has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/TagNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private async Task SendPendingSeedTransferSubmitNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.StId != 0 && n.IsAdmin).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            
            var stIdNotifications = notifications.Select(n => n.StId).Distinct().ToList();
            foreach(var thisNotice in stIdNotifications)
            {
                var message = new MailMessage();
                message.From = new MailAddress("ccia@ucdavis.edu");
                message.Subject = "CCIA New Seed Transfer Submitted";
                var seedTransfer = await _dbContext.SeedTransfers
                    .Include(st => st.OriginatingOrganization)
                    .Include(st => st.OriginatingCounty)
                    .Include(st => st.PurchaserCounty)
                    .Where(st => st.Id == thisNotice).FirstOrDefaultAsync();
                var recipients = notifications.Where(n => n.StId == thisNotice).ToList();                
                foreach(var address in recipients)
                {
                    message.To.Add(address.Email);
                    //message.To.Add("jscubbage@ucdavis.edu");                    
                }
                message.Body = "A new Seed Transfer request has been made. Please visit CCIA website for details";
                var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedTransferAdminNotice.cshtml", seedTransfer), new ContentType(MediaTypeNames.Text.Html));
                message.AlternateViews.Add(htmlView);
                await _client.SendMailAsync(message);
            }            
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private async Task SendPendingSeedTransferResponseNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.StId != 0 && !n.IsAdmin).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            
            var stIdNotifications = notifications.Select(n => n.StId).Distinct().ToList();
            foreach(var thisNotice in stIdNotifications)
            {
                var message = new MailMessage();
                message.From = new MailAddress("ccia@ucdavis.edu");
                message.Subject = "CCIA New Seed Transfer Submitted";
                var seedTransfer = await _dbContext.SeedTransfers
                    .Include(st => st.OriginatingOrganization)
                    .Include(st => st.OriginatingCounty)
                    .Include(st => st.PurchaserCounty)
                    .Include(st => st.AgricultureCommissionerContactRespond)
                    .Where(st => st.Id == thisNotice).FirstOrDefaultAsync();
                var recipients = notifications.Where(n => n.StId == thisNotice).ToList();                
                foreach(var address in recipients)
                {
                    message.To.Add(address.Email);
                    //message.To.Add("jscubbage@ucdavis.edu");                    
                }
                message.Body = "A Seed Transfer request has been responded to. Please visit CCIA website for details";
                var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedTransferResponseNotice.cshtml", seedTransfer), new ContentType(MediaTypeNames.Text.Html));
                message.AlternateViews.Add(htmlView);
                await _client.SendMailAsync(message);
            }            
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   
        }

        private async Task SendPendingOrganizationNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.OrgId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA Organization changes"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An org has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/OrgNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private async Task SendPendingOECDNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications = await _dbContext.Notifications.Where(n => n.Pending && n.OecdId != 0).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }

            var recipients = notifications.Select(n => n.Email).Distinct().ToList();
            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("ccia@ucdavis.edu"), Subject = "CCIA OECD Certificates printed/charged"})
                {
                    //message.To.Add("jscubbage@ucdavis.edu");
                    message.To.Add(address);
                    message.Body = "An org has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/OECDAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        private RazorLightEngine GetRazorEngine()
        {
            var path = Path.GetFullPath(".");

            var engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(path)
                .UseMemoryCachingProvider()
                .Build();

            return engine;
        }

    }
}