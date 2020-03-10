using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Agoda.FileDownloaderSystem.Entities
{
    public class Protocol
    {
        [Key]
        public int ProtocolId { get; set; }
        public string Name { get; set; }
    }
}
