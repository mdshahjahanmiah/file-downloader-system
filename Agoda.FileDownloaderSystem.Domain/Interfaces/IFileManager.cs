using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Agoda.FileDownloaderSystem.Domain.Interfaces
{
    public interface IFileManager
    {
        string GetProtocolFromSource(string source);
        Task GetDataFromResponseAndWriteLocalDisk(string sourceUrl, WebResponse httpWebResponse,string protocol);
        Task GetDataFromResponseAndWriteLocalDisk(string sourceUrl, SftpClient sftp, string protocol);
        string GetFileNameFromUrl(string url);
        IEnumerable<DataObjects.Domain.File> GetAllFileDetails();
    }
}
