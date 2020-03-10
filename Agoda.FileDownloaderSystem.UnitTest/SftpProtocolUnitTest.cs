using Agoda.FileDownloaderSystem.Api;
using Agoda.FileDownloaderSystem.DataObjects.Enums;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Domain.Interfaces;
using Agoda.FileDownloaderSystem.Domain.Managers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Renci.SshNet;
using System;
using System.Net;
using Xunit;

namespace Agoda.FileDownloaderSystem.UnitTest
{
    public class SftpProtocolUnitTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        string url = "sftp://and.also.this/ending";
        public SftpProtocolUnitTest()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }

        [Fact]
        public void GetHttpsProtocolNameFromSourceUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var protocolName = _fileManager.GetProtocolFromSource(url);
            Assert.Equal(protocolName, Protocol.sftp);

        }

        [Fact]
        public void UniquelyDeterminedFromUrl()
        {
            var _fileManager = _serviceProvider.GetService<IFileManager>();
            var fileName = _fileManager.GetFileNameFromUrl(url);
            Assert.Equal("ending", fileName);
        }

        [Fact]
        public void GetDataFromResponseAndWriteLocalDisk()
        {
            try
            {
                var _fileManager = _serviceProvider.GetService<IFileManager>();
                var _appSettings = _serviceProvider.GetService<AppSettings>();

                using var sftp = new SftpClient(_appSettings.SftpNetworkCredential.Host, _appSettings.SftpNetworkCredential.Port, _appSettings.SftpNetworkCredential.UserName, _appSettings.SftpNetworkCredential.Password);
                sftp.Connect();
                _fileManager.GetDataFromResponseAndWriteLocalDisk(url, sftp, Protocol.sftp);
                sftp.Disconnect();
                Assert.True(true);
            }
            catch
            {
                Assert.True(false);
            }
        }
    }

}
