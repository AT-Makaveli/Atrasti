using Atrasti.API.Handlers;
using Atrasti.API.Services.Firebase;
using Atrasti.API.Services;
using Atrasti.Data;
using Atrasti.Data.Models;
using Atrasti.Search;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;

namespace Atrasti.API
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
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Atrasti.API", Version = "v1"});
            });

            services.AddIdentityCore<AtrastiUser>(options =>
            {
                options.SignIn.RequireConfirmedEmail = false;
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireUppercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequiredLength = 8;
            });

            services.AddIdentity<AtrastiUser, AtrastiRole>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddDefaultTokenProviders()
                .InitializeDapperStores();
            services.ConfigureSearchModule();
            
            services.InitAtrastiServices(Configuration);
            services.InitAtrastiAuth(Configuration);
            services.SetupFirebase();
            services.SetupHandlers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Atrasti.API v1"));
            }

            //app.UseHttpsRedirection();

            app.UseRouting();

            ICountryInfoService countryInfoService = app.ApplicationServices.GetRequiredService<ICountryInfoService>();
            countryInfoService.LoadJson().Wait();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseStaticFiles();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}