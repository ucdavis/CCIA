using System.Linq;
using System.Threading.Tasks;
using CCIA.Models;
using Microsoft.EntityFrameworkCore;

namespace CCIA.Services
{
    public interface IEmailService
    {
        Task SendWeeklyApplicationNotices();
    }
    


    public class EmailService : IEmailService
    {
        private readonly CCIAContext _dbContext;

        public EmailService(CCIAContext dbContext)
        {
            _dbContext = dbContext;
        }
        
        public async Task SendWeeklyApplicationNotices()
        {
            var recipients =  await _dbContext.Applications.Where(a => a.Id==6486).FirstOrDefaultAsync();
            recipients.OriginalCertYear = 2022;
            await _dbContext.SaveChangesAsync();
        }

    }
}