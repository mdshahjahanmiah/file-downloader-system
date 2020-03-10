using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Agoda.FileDownloaderSystem.Api
{
    public static class ActionsUrls
    {
        public const string DownloadFile = "DownloadFile";
        public const string GetDownloadedFiles = "GetDownloadedFiles";
       
    }
    public static class ServiceConsumesType
    {
        public const string Json = "application/json";
        public const string Xml = "application/xml";
    }
}
