using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.Security.Handlers
{
    public interface IJwtTokenHandler
    {
        string GenerateJwtSecurityToken(string userId);
        string VerifyJwtSecurityToken(string token);
    }
}
