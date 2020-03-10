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
    public class HttpsProtocolUnitTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        string url = "https://nvd.nist.gov/download/nvd-rss.xml";
        public HttpsProtocolUnitTest()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }

        [Fact]
        public void GetHttpsProtocolNameFromSourceUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var protocolName = _fileManager.GetProtocolFromSource(url);
            Assert.Equal(protocolName, Protocol.Https);

        }

        [Fact]
        public void UniquelyDeterminedFromUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var fileName = _fileManager.GetFileNameFromUrl(url);
            Assert.Equal("nvd-rss.xml", fileName);
        }

        [Fact]
        public void GetDataFromResponseAndWriteLocalDisk()
        {
            try
            {
                var _fileManager = _serviceProvider.GetService<IFileManager>();
                var _appSettings = _serviceProvider.GetService<AppSettings>();
                WebRequest request = WebRequest.Create(url);
                request.Method = WebRequestMethods.Http.Get;
                if (string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.UserName) || string.IsNullOrEmpty(_appSettings.HttpsNetworkCredential.Password))
                    request.Credentials = CredentialCache.DefaultCredentials;
                else request.Credentials = new NetworkCredential(_appSettings.HttpsNetworkCredential.UserName, _appSettings.HttpsNetworkCredential.Password);
                WebResponse response = request.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(url, response, Protocol.Https);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }

}
