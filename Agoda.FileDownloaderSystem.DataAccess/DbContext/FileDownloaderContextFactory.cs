using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Agoda.FileDownloaderSystem.DataAccess.DbContext
{

    public class FileDownloaderContextFactory : IDesignTimeDbContextFactory<FileDownloaderCommandsContext>
    {
        readonly AppConfiguration appConfiguration = new AppConfiguration();
        
        public FileDownloaderCommandsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FileDownloaderCommandsContext>();            
            optionsBuilder.UseSqlServer(appConfiguration.ConnectionString);
            return new FileDownloaderCommandsContext(optionsBuilder.Options);
        }
    }
}
