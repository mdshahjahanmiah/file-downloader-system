using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Profiles
{
    public interface IErrorMapper
    {
        ServerResponse MapToError(ServerResponse response, string errorDetail);
    }
}
