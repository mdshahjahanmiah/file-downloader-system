using Agoda.FileDownloaderSystem.Utilities.EnumAbstration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Enums
{
    public sealed class Protocol : StringEnum
    {
        public Protocol(string value) : base(value)
        {
        }
        public const string Http = "http";
        public const string Https = "https";
        public const string ftp = "ftp";
        public const string tcp = "net.tcp";
        public const string pipe = "net.pipe";
        public const string sftp = "sftp";

        public static int GetKey(string value)
        {
            switch (value)
            {
                case Protocol.Http:
                    return 1;
                case Protocol.Https:
                    return 2;
                case Protocol.ftp:
                    return 3;
                case Protocol.sftp:
                    return 4;
                case Protocol.tcp:
                    return 5;
                case Protocol.pipe:
                    return 6;
            }
            return 0;
        }
    }
}
