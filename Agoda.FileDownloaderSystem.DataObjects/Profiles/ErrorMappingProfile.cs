using Agoda.FileDownloaderSystem.DataObjects.Abstraction;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Profiles
{
    public class ErrorMappingProfile : IErrorMapper
    {
       
        public ServerResponse MapToError(ServerResponse response, string errorDetail)
        {
            ServerResponse errorResponse = response;
            errorResponse.RespDesc = errorDetail;
            return errorResponse;
        }
    }
}
