using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataAccess.DbContext
{
   
    public class FileDownloaderContextFactory : IDesignTimeDbContextFactory<FileDownloaderCommandsContext>
    {
        public FileDownloaderCommandsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FileDownloaderCommandsContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-PQJAHQS;Database=FileDownloaderSystem;Trusted_Connection=True;Integrated Security=true;") ;
            return new FileDownloaderCommandsContext(optionsBuilder.Options);
        }
    }

    public class DependencyResolverHelpercs
    {
        private readonly IWebHost _webHost;
        public DependencyResolverHelpercs(IWebHost WebHost) => _webHost = WebHost;

        public T GetService<T>()
        {
            using (var serviceScope = _webHost.Services.CreateScope())
            {
                var services = serviceScope.ServiceProvider;
                try
                {
                    var scopedService = services.GetRequiredService<T>();
                    return scopedService;
                }
                catch (Exception e)
                {
                    throw;
                }
            };
        }
    }
}
