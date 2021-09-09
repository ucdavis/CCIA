using System;
using CCIA.Services;
using System.IO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using CCIA.Models;
using Microsoft.Extensions.Configuration;
using Thinktecture;
using System.Linq;

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
            var appNotices = context.Jobs.Where(a => a.JobTitle == "Weekly Application Updates" && a.DateNextJobStart < DateTime.Now).AnyAsync().GetAwaiter().GetResult();
            if(appNotices)
            {
                Console.WriteLine("Running weekly app notices");
                var emailService = provider.GetService<IEmailService>();            
                emailService.SendWeeklyApplicationNotices(Configuration["EmailPassword"]).GetAwaiter().GetResult();  
            }

                      
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
                        sqlOptions.AddRowNumberSupport();
                });
                
            });

            services.AddTransient<IEmailService, EmailService>();

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
