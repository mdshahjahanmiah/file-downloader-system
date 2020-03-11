using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Agoda.FileDownloaderSystem.Domain.Interfaces
{
    public interface IDownloader
    {
        Task<ServerResponse> DownloadDataFromHttpToLocalDisk(string sourceUrl,string protocol);
        Task<ServerResponse> DownloadDataFromFtpToLocalDisk(string sourceUrl,string protocol);
        Task<ServerResponse> DownloadDataFromSftpToLocalDisk(string sourceUrl, string protocol);
        Task<ServerResponse> DownloadDataFromHttpsToLocalDisk(string sourceUrl, string protocol);
        Task<ServerResponse> DownloadDataFromTcpToLocalDisk(string sourceUrl, string protocol);
        Task<ServerResponse> DownloadDataFromPipeToLocalDisk(string sourceUrl, string protocol);
        
    }
}
