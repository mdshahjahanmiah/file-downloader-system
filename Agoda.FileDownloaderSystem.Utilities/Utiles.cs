using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.Utilities
{
    public class Utiles
    {
        public static double ConvertBytesToMegabytes(long bytes) => (bytes / 1024f) / 1024f;
    }
}
