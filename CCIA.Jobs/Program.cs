using System;
using CCIA.Services;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CCIA.Models;
using Microsoft.Extensions.Configuration;
using System.Linq;
using Microsoft.Data.SqlClient;
using System.Diagnostics;

namespace CCIA.Jobs
{
    class Program
    {
        public static IConfigurationRoot Configuration { get; set; }

        static void Main(string[] args)
        {   
            Configure();        
            Console.WriteLine("Hello World!");            
            
            var provider = ConfigureServices(); 
            var context = provider.GetService<CCIAContext>();
            var emailService = provider.GetService<IEmailService>();
            var fullcall = provider.GetService<IFullCallService>();
            var appNotices = context.Jobs.Where(a => a.JobTitle == "Weekly Application Updates" && a.DateNextJobStart < DateTime.Now).ToListAsync().GetAwaiter().GetResult();            
            if(appNotices.Count > 0)
            {
                Console.WriteLine("Running weekly app notices");                            
                emailService.SendWeeklyApplicationNotices(Configuration["EmailPassword"]).GetAwaiter().GetResult();  
                var p0 = new SqlParameter("@jobID",  appNotices.First().Id);
                context.Database.ExecuteSqlRaw($"EXEC mark_job_as_completed @jobID", p0);  
            } 
            var adminNotices = context.Jobs.Where(a => a.JobTitle == "Weekly Staff Emails" && a.DateNextJobStart < DateTime.Now).ToListAsync().GetAwaiter().GetResult();
            if(adminNotices.Count > 0)
            {
                Console.WriteLine("Running Staff Weekly Emails");
                emailService.SendWeeklyAdminNotices(Configuration["EmailPassword"]).GetAwaiter().GetResult(); 
                var p0 = new SqlParameter("@jobID",  adminNotices.First().Id);
                context.Database.ExecuteSqlRaw($"EXEC mark_job_as_completed @jobID", p0);
            }           
            emailService.SendNotices(Configuration["EmailPassword"]).GetAwaiter().GetResult();
                      
            Console.WriteLine("End?");
            //var test = Console.ReadLine();            

        }

        private static Microsoft.Extensions.DependencyInjection.ServiceProvider ConfigureServices()
        {
            IServiceCollection services = new ServiceCollection();
            services.AddOptions();           
           
            services.AddDbContextPool<CCIAContext>( o =>
            {
                o.UseSqlServer(Configuration.GetConnectionString("CCIACoreContext"), sqlOptions =>
                {                       
                        sqlOptions.UseNetTopologySuite();
                });
                
            });

            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IFullCallService, FullCallService>();

            return services.BuildServiceProvider();
        }

        protected static void Configure()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json");

            var environmentName = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            builder.AddEnvironmentVariables();
            builder.AddUserSecrets<Program>();
            Configuration = builder.Build();
        }
    }
}
