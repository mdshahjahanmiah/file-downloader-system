using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Renci.SshNet;
using System;
using System.Net;
using System.Threading.Tasks;

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
        public async Task<ServerResponse> DownloadDataFromFtpToLocalDisk(string sourceUrl,string protocol)
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

                WebResponse response = await request.GetResponseAsync();
                await _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, response, protocol);
                return serverResponse;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400,RespDesc = ex.Message };
            }
            
        }

        public async Task<ServerResponse> DownloadDataFromHttpToLocalDisk(string sourceUrl, string protocol)
        {
            var response = ServerResponse.OK;
            try 
            {
                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(sourceUrl);
                httpRequest.Method = WebRequestMethods.Http.Get;
                WebResponse httpResponse = await httpRequest.GetResponseAsync();
                await _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, httpResponse, protocol);
                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }
            
        }

        public async Task<ServerResponse> DownloadDataFromSftpToLocalDisk(string sourceUrl, string protocol)
        {
            var response = ServerResponse.OK;
            try 
            {
                using var sftp = new SftpClient(_appSettings.SftpNetworkCredential.Host, _appSettings.SftpNetworkCredential.Port, _appSettings.SftpNetworkCredential.UserName, _appSettings.SftpNetworkCredential.Password);
                sftp.Connect();
                await _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, sftp, protocol);
                sftp.Disconnect();
                return response;
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }

        }

        public async Task<ServerResponse> DownloadDataFromHttpsToLocalDisk(string sourceUrl, string protocol)
        {
            var serverResponse = ServerResponse.OK;
            try
            {
                WebRequest request = WebRequest.Create(sourceUrl);
                request.Method = WebRequestMethods.Http.Get;
                if (string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.UserName) || string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.Password))
                    request.Credentials = CredentialCache.DefaultCredentials;
                else request.Credentials = new NetworkCredential(_appSettings.HttpsNetworkCredential.UserName, _appSettings.HttpsNetworkCredential.Password);
                WebResponse response = await request.GetResponseAsync();
                await _fileManager.GetDataFromResponseAndWriteLocalDisk(sourceUrl, response, protocol);
                response.Close();
                return serverResponse; 
            }
            catch (Exception ex)
            {
                return new ServerResponse() { RespCode = 400, RespDesc = ex.Message };
            }
        }
       
        public async Task<ServerResponse> DownloadDataFromTcpToLocalDisk(string sourceUrl, string protocol)
        {
            return new ServerResponse() { RespCode = 400, RespDesc = "Not Implemented Yet." };
        }
        
        public async Task<ServerResponse> DownloadDataFromPipeToLocalDisk(string sourceUrl, string protocol)
        {
            return new ServerResponse() { RespCode = 400, RespDesc = "Not Implemented Yet." };
        }
    }
}
