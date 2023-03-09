using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.App.Migrations;
using WebRepo.DAL.Default;

namespace WebRepo.DAL.Entities
{
    public class VirtualDirectory : DefaultEntity
    {
        public VirtualDirectory()
        {
            FileBlobs = new HashSet<FileBlob>();
        }

        public string Name { get; set; }
        public int? ParentDirectory { get; set; }
        public virtual User User { get; set; }
        public virtual ICollection<FileBlob> FileBlobs { get; set; }
    }
}
