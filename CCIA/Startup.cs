using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CCIA.Models;
using CCIA.Services;
using Microsoft.Extensions.Hosting;

namespace CCIA
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddDbContext<CCIAContext>();

            services.AddAuthentication( "Cookies") // Sets the default scheme to cookies
                .AddCookie( "Cookies", options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                });

            // services.AddIdentity<ApplicationUser, IdentityRole>()
            //     .AddEntityFrameworkStores<CCIAContext>()
            //     .AddDefaultTokenProviders();
            
            //services.ConfigureApplicationCookie(options => options.LoginPath = "/Account/Login");
            //services.AddAuthentication();

            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, Microsoft.Extensions.Hosting.IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }                     

            app.UseStaticFiles();
            app.UseCookiePolicy();
            
            app.UseAuthentication();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
