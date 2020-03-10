using Renci.SshNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Agoda.FileDownloaderSystem.Domain.Interfaces
{
    public interface IFileManager
    {
        string GetProtocolFromSource(string source);
        void GetDataFromResponseAndWriteLocalDisk(string sourceUrl, WebResponse httpWebResponse,string protocol);
        void GetDataFromResponseAndWriteLocalDisk(string sourceUrl, SftpClient sftp, string protocol);
        string GetFileNameFromUrl(string url);
        IEnumerable<DataObjects.Domain.File> GetAllFileDetails();
    }
}
