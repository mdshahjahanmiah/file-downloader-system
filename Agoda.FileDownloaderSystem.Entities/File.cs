using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agoda.FileDownloaderSystem.Entities
{
    public class File
    {
        [Key]
        public int FileId { get; set; }
        public string Source { get; set; }
        public string Destination { get; set; }
        public DateTime DownloadStartedDate { get; set; }
        public DateTime DownloadEndedDate { get; set; }
        public bool IsLargeData { get; set; }
        public bool IsSlow { get; set; }
        public int PercentageOfFailure { get; set; }
        public double ElapsedTime{get;set;}
        public double DownloadSpeed { get; set; }
        public int StatusId { get; set; }
        public Status Status { get; set; }
        public int ProtocolId { get; set; }
        public Protocol Protocol { get; set; }
    }
}
