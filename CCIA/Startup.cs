﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using CCIA.Models;
using CCIA.Services;
using Microsoft.Extensions.Hosting;
using AspNetCore.Security.CAS;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using System.Threading.Tasks;

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
                })                
                .AddCAS(o =>
                {
                    o.CasServerUrlBase = Configuration["CasBaseUrl"];   // Set in `appsettings.json` file.
                    o.SignInScheme = "Cookies";
                    o.Events.OnTicketReceived = async context => {
                        var identity = (ClaimsIdentity) context.Principal.Identity;
                        if (identity == null)
                        {
                            return;
                        }

                        // kerb comes across in name & name identifier
                        var kerb = identity?.FindFirst(ClaimTypes.NameIdentifier).Value;

                        if (string.IsNullOrWhiteSpace(kerb)) return;

                        // var identityService = services.BuildServiceProvider().GetService<IIdentityService>();

                        // var user = await identityService.GetByKerberos(kerb);

                        // if (user == null)
                        // {
                        //     throw new InvalidOperationException("Could not retrieve user information from IAM");
                        // }
                        

                        // identity.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
                        // identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id));

                        // identity.RemoveClaim(identity.FindFirst(ClaimTypes.Name));
                        // identity.AddClaim(new Claim(ClaimTypes.Name, user.Id));

                        // identity.AddClaim(new Claim(ClaimTypes.GivenName, user.FirstName));
                        // identity.AddClaim(new Claim(ClaimTypes.Surname, user.LastName));
                        // identity.AddClaim(new Claim("name", user.Name));
                        // identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));
                        identity.RemoveClaim(identity.FindFirst(ClaimTypes.NameIdentifier));
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, kerb));
                        identity.AddClaim(new Claim(ClaimTypes.Role, "Employee"));

                        context.Principal.AddIdentity(identity);

                        await Task.FromResult(0); 
                    };
                });
           
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
