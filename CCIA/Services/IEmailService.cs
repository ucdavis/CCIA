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

        Task SendPendingSeedNotices(string password);

        Task SendPendingBlendNotices(string password);

        Task SendPendingTagNotices(string password);

        Task SendPendingOrganizationNotices(string password);

        Task SendPendingOECDNotices(string password);

        Task SendPendingAdminAppNotices(string password);
        Task SendPendingAdminSeedNotices(string password);

        Task SendNotices(string password);
    }
    


    public class EmailService : IEmailService
    {
        private readonly CCIAContext _dbContext;        

        private SmtpClient _client;

        public EmailService(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }
        
         private void ConfigureSMTPClient(string password)
        {
            _client = new SmtpClient("ucdavis-edu.mail.protection.outlook.com",25) {Credentials = new NetworkCredential("ad3/jscub", password), EnableSsl = true};
        }

        public async Task SendNotices(string password)
        {
            await SendPendingSeedNotices(password);
            await SendPendingAdminAppNotices(password);
            await SendPendingAdminSeedNotices(password);
            await SendPendingAdminNFCNotices(password);
            await SendPendingAdminTagNotices(password);
            await SendPendingBlendNotices(password);
            await SendPendingTagNotices(password);
            await SendPendingOrganizationNotices(password);
            await SendPendingOECDNotices(password);
        }

        public  async Task SendWeeklyApplicationNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications =  await _dbContext.Notifications.Where(n => n.Pending && n.AppId != 0 && !n.IsAdmin).ToListAsync();
            if(notifications.Count == 0)
            {
                return;
            }
            var recipients = notifications.Select(n => n.Email).Distinct().ToList();           

            foreach(var address in recipients)
            {
                var thisNotices = notifications.Where(n => n.Email == address).ToList();                

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA application status changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    message.Body = "An application status has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/ApplicationWeeklyNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            } 
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();          
        }

        public async Task SendPendingAdminAppNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Application Notices"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "An Application has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/ApplicationWeeklyNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        public async Task SendPendingAdminNFCNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Seed Notices"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "An Seed Lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/NFCAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        public async Task SendPendingAdminSeedNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Seed Notices"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "An Seed Lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();  
        }

        public async Task SendPendingSeedNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA SID status changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "A SID lot has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/SeedClientNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   
        }

        public async Task SendPendingBlendNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Blend status changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "A Blend has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/BlendClientNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        public async Task SendPendingAdminTagNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Tag status changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "A Tag has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/TagAdminNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        public async Task SendPendingTagNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Tag status changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "A Tag has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/TagNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        public async Task SendPendingOrganizationNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA Organization changes"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
                    message.Body = "An org has been updated. Please visit CCIA website for details";
                    var htmlView = AlternateView.CreateAlternateViewFromString(await GetRazorEngine().CompileRenderAsync("/EmailTemplates/OrgNotices.cshtml", thisNotices), new ContentType(MediaTypeNames.Text.Html));
                    message.AlternateViews.Add(htmlView);
                    await _client.SendMailAsync(message);
                }
            }
            notifications.ForEach(n => {n.Pending = false; n.Sent = System.DateTime.Now;}); 
            await _dbContext.SaveChangesAsync();   

        }

        public async Task SendPendingOECDNotices(string password)
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

                using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "CCIA OECD Certificates printed/charged"})
                {
                    message.To.Add("jscubbage@ucdavis.edu");
                    //message.To.Add(address);
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