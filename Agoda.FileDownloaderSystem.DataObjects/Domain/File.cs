using System;
using System.Collections.Generic;
using System.Text;

namespace Agoda.FileDownloaderSystem.DataObjects.Domain
{
    public class File
    {
        public int FileId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime DownloadStartedDate { get; set; }
        public DateTime DownloadEndedDate { get; set; }
        public string Protocol { get; set; }
        public bool IsLargeData { get; set; }
        public bool IsSlow { get; set; }
        public int PercentageOfFailure { get; set; }
        public double ElapsedTime { get; set; }
        public double DownloadSpeed { get; set; }
        public int StatusId { get; set; }
        public int ProtocolId { get; set; }
        public string Status { get; set; }

    }
}
