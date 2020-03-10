using Newtonsoft.Json;

namespace Agoda.FileDownloaderSystem.DataObjects.Abstraction
{
    public class ServerResponse 
    {
        public static ServerResponse OK = new ServerResponse { RespCode = 200 };
        public static ServerResponse ERROR = new ServerResponse { RespCode = 500 };
        public static ServerResponse BadRequest = new ServerResponse { RespCode = 400 };
        public static ServerResponse Unauthorized = new ServerResponse { RespCode = 401 };
        public static ServerResponse Forbidden = new ServerResponse { RespCode = 403 };
        public static ServerResponse NotFound = new ServerResponse { RespCode = 404 };
        public int RespCode { get; set; }
        public string RespDesc { get; set; }
    }
}
