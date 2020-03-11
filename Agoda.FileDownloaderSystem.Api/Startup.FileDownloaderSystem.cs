using Agoda.FileDownloaderSystem.DataAccess.DbContext;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Agoda.FileDownloaderSystem.Api
{
    public partial class Startup
    {
        private void FileDownloaderSystemDependencies(IServiceCollection services, AppSettings settings)
        {
            services.AddDbContext<FileDownloaderCommandsContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings.SqlServer.Commands,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                });
            });

            services.AddDbContext<FileDownloaderQueriesContext>(options =>
            {
                options.UseSqlServer(settings.ConnectionStrings.SqlServer.Queries,
                sqlServerOptionsAction: sqlOptions =>
                {
                    sqlOptions.EnableRetryOnFailure(
                    maxRetryCount: 10,
                    maxRetryDelay: TimeSpan.FromSeconds(30),
                    errorNumbersToAdd: null);
                });
            });
        }
    }
}
