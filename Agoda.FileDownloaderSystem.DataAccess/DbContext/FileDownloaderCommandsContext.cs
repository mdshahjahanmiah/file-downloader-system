using Agoda.FileDownloaderSystem.Entities;
using Microsoft.EntityFrameworkCore;

namespace Agoda.FileDownloaderSystem.DataAccess.DbContext
{
    public class FileDownloaderCommandsContext : Microsoft.EntityFrameworkCore.DbContext
    {
        public FileDownloaderCommandsContext(DbContextOptions<FileDownloaderCommandsContext> options) : base(options)
        {
            
        }
        public DbSet<Status> Status { get; set; }
        public DbSet<Protocol> Protocol { get; set; }
        public DbSet<File> File { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Seed Value
            modelBuilder.Entity<Status>().HasData(new { StatusId = 1, Name = "Completed" });
            modelBuilder.Entity<Status>().HasData(new { StatusId = 2, Name = "Failed" });


            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 1, Name = "http" });
            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 2, Name = "https" });
            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 3, Name = "ftp" });
            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 4, Name = "sftp" });
            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 5, Name = "net.tcp" });
            modelBuilder.Entity<Protocol>().HasData(new { ProtocolId = 6, Name = "net.pipe" });

            #endregion 
        }
    }
}
