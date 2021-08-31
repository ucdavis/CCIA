using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace CCIA.Services
{
    public interface IEmailService
    {
        Task SendWeeklyApplicationNotices(string password);
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
            var recipients =  await _dbContext.Applications.Where(a => a.Id==6486).FirstOrDefaultAsync();
            recipients.OriginalCertYear = 1922;
            await _dbContext.SaveChangesAsync();
            using (var message = new MailMessage {From = new MailAddress("jscubbage@ucdavis.edu", "James Cubbage"), Subject = "Testing email send"})
            {
                message.To.Add("jscubbage@ucdavis.edu");
                message.Body = "Test";
                await _client.SendMailAsync(message);
            }
            
        }

    }
}