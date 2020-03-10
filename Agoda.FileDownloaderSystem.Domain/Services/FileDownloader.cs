using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Renci.SshNet;
using System;
using System.IO;
using System.Net;
using WinSCP;

namespace Agoda.FileDownloaderSystem.Domain.Services
{
    public class FileDownloader : IDownloader
    {
        private readonly AppSettings _appSettings;
        private readonly IFileManager _fileManager;
        public FileDownloader(IFileManager fileManager, AppSettings appSettings)
        {
            _fileManager = fileManager;
            _appSettings = appSettings;
        }
        public ServerResponse DownloadDataFromFtpToLocalDisk(string sourceUrl,string protocol)
        {
            var serverResponse = ServerResponse.OK;
            try 
            {
                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(sourceUrl);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true; // use true for .zip file or false for a text file
                request.Credentials = new NetworkCredential(_appSettings.FtpNetworkCredential.UserName, _appSettings.FtpNetworkCredential.Password);

                WebResponse response = request.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, response, protocol);
                return serverResponse;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400,RespDesc = ex.Message };
            }
            
        }

        public ServerResponse DownloadDataFromHttpToLocalDisk(string sourceUrl, string protocol)
        {
            var response = ServerResponse.OK;
            try 
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(sourceUrl);
                httpRequest.Method = WebRequestMethods.Http.Get;
                WebResponse httpResponse = httpRequest.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, httpResponse, protocol);
                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }
            
        }

        public ServerResponse DownloadDataFromSftpToLocalDisk(string sourceUrl, string protocol)
        {
            var response = ServerResponse.OK;
            try 
            {
                using var sftp = new SftpClient(_appSettings.SftpNetworkCredential.Host, _appSettings.SftpNetworkCredential.Port, _appSettings.SftpNetworkCredential.UserName, _appSettings.SftpNetworkCredential.Password);
                sftp.Connect();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, sftp, protocol);
                sftp.Disconnect();
                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }

        }

        public ServerResponse DownloadDataFromHttpsToLocalDisk(string sourceUrl, string protocol)
        {
            var serverResponse = ServerResponse.OK;
            try
            {
                WebRequest request = WebRequest.Create(sourceUrl);
                request.Method = WebRequestMethods.Http.Get;
                if (string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.UserName) || string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.Password))
                    request.Credentials = CredentialCache.DefaultCredentials;
                else request.Credentials = new NetworkCredential(_appSettings.HttpsNetworkCredential.UserName, _appSettings.HttpsNetworkCredential.Password);
                WebResponse response = request.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, response, protocol);
                response.Close();
                return serverResponse; 
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }
        }
       
        public ServerResponse DownloadDataFromTcpToLocalDisk(string sourceUrl, string protocol)
        {
            throw new NotImplementedException();
        }
        
        public ServerResponse DownloadDataFromPipeToLocalDisk(string sourceUrl, string protocol)
        {
            throw new NotImplementedException();
        }
    }
}
