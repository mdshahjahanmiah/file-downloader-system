using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.Domain.Interfaces
{
    public interface IDownloader
    {
        ServerResponse DownloadDataFromHttpToLocalDisk(string sourceUrl,string protocol);
        ServerResponse DownloadDataFromFtpToLocalDisk(string sourceUrl,string protocol);
        ServerResponse DownloadDataFromSftpToLocalDisk(string sourceUrl, string protocol);
        ServerResponse DownloadDataFromHttpsToLocalDisk(string sourceUrl, string protocol);
        ServerResponse DownloadDataFromTcpToLocalDisk(string sourceUrl, string protocol);
        ServerResponse DownloadDataFromPipeToLocalDisk(string sourceUrl, string protocol);
        
    }
}
