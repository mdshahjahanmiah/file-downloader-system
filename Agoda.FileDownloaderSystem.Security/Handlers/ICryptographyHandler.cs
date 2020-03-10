using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.Security.Handlers
{
    public interface ICryptographyHandler
    {
        string GeneratePasswordHash(string password);
        bool VerifyGeneratedHash(string password, string savedPasswordHash);
    }
}
