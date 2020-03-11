using Agoda.FileDownloaderSystem.Api;
using Agoda.FileDownloaderSystem.DataObjects.Enums;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using System.Net;
using Xunit;

namespace Agoda.FileDownloaderSystem.UnitTest
{
    public class HttpProtocolUnitTest 
    {
        private DependencyResolverHelpercs _serviceProvider;
        private readonly string url = "http://www.techcoil.com/ph/img/logo.png";
        public HttpProtocolUnitTest()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }

        [Fact]
        public void GetHttpProtocolNameFromSourceUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var protocolName = _fileManager.GetProtocolFromSource(url);
            Assert.Equal(protocolName, Protocol.Http);

        }

        [Fact]
        public void UniquelyDeterminedFileNameFromUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var fileName = _fileManager.GetFileNameFromUrl(url);
            Assert.Equal("logo.png", fileName);
        }

        [Fact]
        public void GetDataFromHttpResponseAndWriteLocalDisk()
        {
            try
            {
                var _fileManager = _serviceProvider.GetService<IFileManager>();
                var _appSettings = _serviceProvider.GetService<AppSettings>();

                HttpWebRequest httpRequest = (HttpWebRequest)WebRequest.Create(url);
                httpRequest.Method = WebRequestMethods.Http.Get;
                WebResponse httpResponse = httpRequest.GetResponse();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(url, httpResponse, Protocol.Http);
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }

}
