using Agoda.FileDownloaderSystem.DataAccess.Repository;
using Agoda.FileDownloaderSystem.DataObjects.Profiles;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Agoda.FileDownloaderSystem.Domain.Managers;
using Agoda.FileDownloaderSystem.Domain.Services;
using Agoda.FileDownloaderSystem.Security.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Agoda.FileDownloaderSystem.Api
{
    public partial class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            var settings = GetAppConfigurationSection();
            ConfigureSingletonServices(services);
            ConfigureScopedServices(services);
            ConfigureTransientServices(services);
            ConfigureJwtAuthentication(services);
            FileDownloaderSystemDependencies(services, settings);
            SetupCrossOriginResourceSharing(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors();
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private AppSettings GetAppConfigurationSection() => Configuration.GetSection("AppSettings").Get<AppSettings>();

        private void ConfigureSingletonServices(IServiceCollection services)
        {
            services.AddSingleton(GetAppConfigurationSection());
        }

        private void ConfigureScopedServices(IServiceCollection services)
        {
            services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        }

        private void ConfigureTransientServices(IServiceCollection services)
        {
            services.AddTransient(typeof(IDownloader), typeof(FileDownloader));
            services.AddTransient(typeof(IFileManager), typeof(FileManager));
            services.AddTransient(typeof(IErrorMapper), typeof(ErrorMappingProfile));
            services.AddTransient(typeof(ICryptographyHandler), typeof(CryptographyHandler));
            services.AddTransient(typeof(IJwtTokenHandler), typeof(JwtTokenHandler));
        }

        private static void SetupCrossOriginResourceSharing(IServiceCollection services)
        {
            services.AddCors(options => options.AddPolicy("AllowAllHeaders", builder => builder
                                .AllowAnyOrigin()
                                .AllowAnyHeader()
                                .AllowAnyMethod()));
            services.AddHealthChecks();
        }

        private void ConfigureJwtAuthentication(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(GetAppConfigurationSection().Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };
            });
            IdentityModelEventSource.ShowPII = true;
        }

    }
}
