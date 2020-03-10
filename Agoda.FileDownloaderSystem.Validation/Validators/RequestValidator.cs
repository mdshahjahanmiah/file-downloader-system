using Agoda.FileDownloaderSystem.DataObjects.Enums;
using System;

namespace Agoda.FileDownloaderSystem.Validation.Validators
{
    public class RequestValidator
    {
        public static bool CheckURLValid(string source, string protocol)
        {
            bool isValid = false;
            if (protocol == Protocol.Http) isValid = Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttp;
            else if (protocol == Protocol.Https) isValid = Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeHttps;
            else if (protocol == Protocol.ftp) isValid = Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == Uri.UriSchemeFtp;
            else if (protocol == Protocol.sftp) isValid = Uri.TryCreate(source, UriKind.Absolute, out Uri uriResult) && uriResult.Scheme == "sftp";
            return isValid;
        }
    }
}
