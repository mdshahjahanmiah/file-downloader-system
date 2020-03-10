using Agoda.FileDownloaderSystem.Api;
using Agoda.FileDownloaderSystem.DataObjects.Settings;
using Agoda.FileDownloaderSystem.Security.Handlers;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Xunit;

namespace Agoda.FileDownloaderSystem.UnitTest
{
    public class SecurityLibraryTest
    {
        private DependencyResolverHelpercs _serviceProvider;
        public SecurityLibraryTest()
        {
            var webHost = WebHost.CreateDefaultBuilder().UseStartup<Startup>().Build();
            _serviceProvider = new DependencyResolverHelpercs(webHost);
        }
        [Fact]
        public void GeneratePasswordHashSuccessMethod()
        {
            var _cryptographyHandler = _serviceProvider.GetService <ICryptographyHandler>();
            var password = "123456";
            var hashPassword = _cryptographyHandler.GeneratePasswordHash(password);
            Assert.True(!string.IsNullOrEmpty(hashPassword));
        }
        [Fact]
        public void VerifyGeneratedHashSuccessMethod()
        {
            var _cryptographyHandler = _serviceProvider.GetService<ICryptographyHandler>();
            var password = "123456";
            var savedPasswordHash = "9SX59yDbWpfRpbGfTqNnqw2y8AA6E+TEvu5aWCx3fl+bRblA";
            Assert.True(_cryptographyHandler.VerifyGeneratedHash(password, savedPasswordHash));
        }
        [Fact]
        public void GenerateJwtSecurityTokenSuccessMethod()
        {
            var appSettings = _serviceProvider.GetService<AppSettings>();
            string userId = "1";
            var token = new JwtTokenHandler(appSettings).GenerateJwtSecurityToken(userId);
            Assert.True(!string.IsNullOrEmpty(token));
        }
    }
}
