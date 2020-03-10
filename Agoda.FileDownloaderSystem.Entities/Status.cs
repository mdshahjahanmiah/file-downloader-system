using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agoda.FileDownloaderSystem.Entities
{
    public class Status
    {
        [Key]
        public int StatusId { get; set; }
        public string Name { get; set; }
    }
}
