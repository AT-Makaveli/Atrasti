using System;
using Atrasti.Data;
using Atrasti.Data.Models;
using Atrasti.Middleware;
using Atrasti.Search;
using Atrasti.Services;
using Atrasti.Utils;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Atrasti
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
            services.AddTransient<SearchAlgorithm>();

            services.AddIdentityCore<AtrastiUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = true;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            });

            services.AddIdentity<AtrastiUser, AtrastiRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddDefaultTokenProviders()
                .InitializeDapperStores();

            services.Configure<EmailConfiguration>(Configuration.GetSection("EmailConfiguration"));
            services.AddTransient<IEmailSender, EmailSender>();

            services.AddControllersWithViews();
            services.AddSignalR();
            services.AddDistributedMemoryCache();

            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
            });
            
            services.AddTransient<ContinentBlockMiddleWare>();

            services.ConfigureSearchModule();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseForwardedHeaders();
            app.UseSession();
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseMiddleware<ContinentBlockMiddleWare>();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();
            app.UseAuthentication();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}");
                
                endpoints.MapControllerRoute(
                    name: "About",
                    pattern: "About",
                    defaults: new { controller = "Home", action = "About" });

                endpoints.MapControllerRoute(
                    name: "Construction",
                    pattern: "Construction",
                    defaults: new {controller = "Home", action = "Construction"});
                
                endpoints.MapControllerRoute(
                    name: "Popular",
                    pattern: "Popular/{type?}",
                    defaults: new { controller = "Home", action = "Popular" });
                
                endpoints.MapControllerRoute(
                    name: "Contact",
                    pattern: "Contact",
                    defaults: new { controller = "Home", action = "Contact" });
                
                endpoints.MapControllerRoute(
                    name: "Profile",
                    pattern: "Profile/{id?}",
                    defaults: new { controller = "Profile", action = "Index" });
                
                endpoints.MapControllerRoute(
                    name: "Login",
                    pattern: "Login",
                    defaults: new { controller = "Login", action = "Index" });
                
                endpoints.MapControllerRoute(
                    name: "Register",
                    pattern: "Register",
                    defaults: new { controller = "Register", action = "Index" });
                
                endpoints.MapControllerRoute(
                    name: "Invite",
                    pattern: "Invite/{id?}",
                    defaults: new { controller = "Invite", action = "Index" });
                
                endpoints.MapControllerRoute(
                    name: "Policies",
                    pattern: "Policies",
                    defaults: new { controller = "Policies", action = "PrivacyPolicy" });

                //endpoints.MapHub<ChatHub>("/chathub");
            });
        }
    }
}