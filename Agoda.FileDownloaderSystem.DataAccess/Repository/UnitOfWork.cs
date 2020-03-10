using Agoda.FileDownloaderSystem.DataAccess.DbContext;

namespace Agoda.FileDownloaderSystem.DataAccess.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        public FileDownloaderCommandsContext CommandsContext { get; }
        public FileDownloaderQueriesContext QueriesContext { get; }
        public UnitOfWork(FileDownloaderCommandsContext commandsContext, FileDownloaderQueriesContext queriesContext)
        {
            CommandsContext = commandsContext;
            QueriesContext = queriesContext;
        }
        public void Commit()
        {
            CommandsContext.SaveChanges();
        }
        public void Dispose()
        {
            CommandsContext.Dispose();
            QueriesContext.Dispose();

        }
    }
}
