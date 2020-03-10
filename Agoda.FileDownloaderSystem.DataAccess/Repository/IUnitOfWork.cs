using Agoda.FileDownloaderSystem.DataAccess.DbContext;
using System;

namespace Agoda.FileDownloaderSystem.DataAccess.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        FileDownloaderCommandsContext CommandsContext { get; }
        FileDownloaderQueriesContext QueriesContext { get; }
        void Commit();
    }
}
