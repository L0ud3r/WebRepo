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
            VirtualDirectories = new HashSet<VirtualDirectory>();
            FileBlobs = new HashSet<FileBlob>();
        }

        public string Name { get; set; }
        public virtual User User { get; set; }
        public virtual VirtualDirectory? ParentDirectory { get; set; }
        public virtual ICollection<VirtualDirectory> VirtualDirectories { get; set; }
        public virtual ICollection<FileBlob> FileBlobs { get; set; }
    }
}
