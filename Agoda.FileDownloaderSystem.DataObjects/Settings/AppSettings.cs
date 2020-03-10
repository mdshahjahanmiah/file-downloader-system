using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Settings
{
    public class AppSettings
    {
        public string DownloadedFileLocation { get; set; }
        public ConnectionStrings ConnectionStrings { get; set; }
        public string Version { get; set; }
        public string Secret { get; set; }
        public FtpNetworkCredential FtpNetworkCredential { get; set; }
        public SftpNetworkCredential SftpNetworkCredential { get; set; }
        public HttpsNetworkCredential HttpsNetworkCredential { get; set; }
        public int BufferSize { get; set; }
        public long VolumeOfData { get; set; }
        public double VelocityOfData { get; set; }
    }

    public class ConnectionStrings
    {
        public Database SqlServer { get; set; }
    }

    public class Database
    {
        public string Queries { get; set; }
        public string Commands { get; set; }
    }

    public class FtpNetworkCredential 
    { 
        public string UserName { get; set; }
        public string Password { get; set; }
    }
    public class HttpsNetworkCredential
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }

    public class SftpNetworkCredential : FtpNetworkCredential
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}
