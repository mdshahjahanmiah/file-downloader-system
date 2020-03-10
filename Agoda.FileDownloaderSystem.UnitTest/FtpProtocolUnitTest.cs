using Agoda.FileDownloaderSystem.Api;
using Agoda.FileDownloaderSystem.DataObjects.Enums;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Agoda.FileDownloaderSystem.Domain.Managers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Net;
using Xunit;

namespace Agoda.FileDownloaderSystem.UnitTest
{
    public class FtpProtocolUnitTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        string url = "ftp://other.file.com/other";
        public FtpProtocolUnitTest()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }

        [Fact]
        public void GetHttpsProtocolNameFromSourceUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var protocolName = _fileManager.GetProtocolFromSource(url);
            Assert.Equal(protocolName, Protocol.ftp);

        }

        [Fact]
        public void UniquelyDeterminedFromUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var fileName = _fileManager.GetFileNameFromUrl(url);
            Assert.Equal("other", fileName);
        }

        [Fact]
        public void GetDataFromResponseAndWriteLocalDisk()
        {
            try
            {
                var _fileManager = _serviceProvider.GetService<IFileManager>();
                var _appSettings = _serviceProvider.GetService<AppSettings>();

                FtpWebRequest request = (FtpWebRequest)WebRequest.Create(url);
                request.Method = WebRequestMethods.Ftp.DownloadFile;
                request.KeepAlive = true;
                request.UsePassive = true;
                request.UseBinary = true; // use true for .zip file or false for a text file
                request.Credentials = new NetworkCredential(_appSettings.FtpNetworkCredential.UserName, _appSettings.FtpNetworkCredential.Password);

                WebResponse response = request.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(url, response, Protocol.ftp);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }

}
