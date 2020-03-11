using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Agoda.FileDownloaderSystem.DataAccess.DbContext
{

    public class FileDownloaderContextFactory : IDesignTimeDbContextFactory<FileDownloaderCommandsContext>
    {
        public FileDownloaderCommandsContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<FileDownloaderCommandsContext>();
            optionsBuilder.UseSqlServer("Server=DESKTOP-TBD1CDL;Database=FileDownloaderSystem;Trusted_Connection=True;Integrated Security=true;") ;
            return new FileDownloaderCommandsContext(optionsBuilder.Options);
        }
    }
}
