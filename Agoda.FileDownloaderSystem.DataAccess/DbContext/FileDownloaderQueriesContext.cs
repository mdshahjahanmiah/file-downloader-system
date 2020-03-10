using Agoda.FileDownloaderSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agoda.FileDownloaderSystem.DataAccess.DbContext
{
    public class FileDownloaderQueriesContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public FileDownloaderQueriesContext(DbContextOptions<FileDownloaderQueriesContext> options) : base(options)
        {
        }
        public DbSet<Status> Status { get; set; }
        public DbSet<Protocol> Protocol { get; set; }
        public DbSet<File> File { get; set; }
    }
}
