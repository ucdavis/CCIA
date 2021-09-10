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

        public  async Task SendWeeklyApplicationNotices(string password)
        {
            ConfigureSMTPClient(password);
            var notifications =  await _dbContext.Notifications.Where(n => n.Pending && n.AppId != 0).ToListAsync();
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