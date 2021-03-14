using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CCIA.Models;
using CCIA.Services;
using Microsoft.Extensions.Hosting;
using System.Security.Claims;
using System.Threading.Tasks;
using System;

namespace CCIA
{
    public class Startup
    {
        public Startup(IConfiguration configuration, IWebHostEnvironment env)
        {
            Configuration = configuration;
            Env = env;
        }

        public IConfiguration Configuration { get; }
        public IWebHostEnvironment Env { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddScoped<IIdentityService, IdentityService>();
                     
            services.AddControllersWithViews(); 

            IMvcBuilder builder = services.AddRazorPages(); 

            #if DEBUG
                if (Env.IsDevelopment())
                {
                    builder.AddRazorRuntimeCompilation();
                }
            #endif        
       
          
            services.AddDbContext<CCIAContext>();
           

            services.AddAuthentication( "Cookies") // Sets the default scheme to cookies
                .AddCookie( "Cookies", options =>
                {
                    options.AccessDeniedPath = "/account/denied";
                    options.LoginPath = "/account/login";
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
                    options.SlidingExpiration = true;                    
                })          
                .AddCAS("CAS",o =>
                {
                    o.CasServerUrlBase = Configuration["CasBaseUrl"];   // Set in `appsettings.json` file.
                    o.SignInScheme = "Cookies";                   
                    o.Events.OnCreatingTicket  = async context => {
                        var identity = (ClaimsIdentity) context.Principal.Identity;
                        var assertion = context.Assertion;
                        if (identity == null)
                        {
                            return;
                        }

                        var kerb = assertion.PrincipalName;

                        if (string.IsNullOrWhiteSpace(kerb)) return;

                        var identityService = services.BuildServiceProvider().GetService<IIdentityService>();

                        var user = await identityService.GetByKerberos(kerb);

                        if (user == null)
                        {
                            return;
                        }                        

                        var existingClaim = identity.FindFirst(ClaimTypes.Name);
                        if(existingClaim != null)
                        {
                            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
                        }                        
                        identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));

                        identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
                        identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                        identity.AddClaim(new Claim("name", user.Name));
                        identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        existingClaim = identity.FindFirst(ClaimTypes.NameIdentifier);
                        if(existingClaim != null)
                        {
                            identity.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
                        }
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, kerb));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));
                        if(!user.SeasonalEmployee)
                        {
                            identity.AddClaim(new Claim(ClaimTypes.Role, "AllowEmulate"));
                        }

                        context.Principal.AddIdentity(identity);

                        await Task.FromResult(0); 
                    };
                });
           
            // Add application services.
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddScoped<IFullCallService, FullCallService>();

            
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }                     

            app.UseStaticFiles();
            
            app.UseAuthentication();  
            app.UseCookiePolicy();
            app.UseRouting();
            app.UseAuthorization();
            

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapAreaControllerRoute(
                   name: "Client_route",
                   areaName: "Client",
                   pattern:  "client/{controller}/{action=Index}/{id?}"
                );

                endpoints.MapAreaControllerRoute(
                    name: "Admin_route",
                   areaName: "Admin",
                   pattern:  "admin/{controller}/{action=Index}/{id?}"
                );

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            
        }
    }
}
