using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Default;

namespace WebRepo.DAL.Entities
{
    public class FileCdn : DefaultEntity
    {
        public string FileName { get; set; }
        public string ContentType { get; set; }
        public long Size { get; set; }
        public string CdnUrl { get; set; }
        public string? FileUrl { get; set; }
        public string? CdnProvider { get; set; }
        public string? CdnAccessKey { get; set; }
        public string? CdnSecretKey { get; set; }
        public string? CdnRegion { get; set; }
    }
}
