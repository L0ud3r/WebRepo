using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Default;

namespace WebRepo.DAL.Entities
{
    public class User : DefaultEntity
    {
        public User()
        {
            Files = new HashSet<FileBlob>();
            UserTokens = new HashSet<UserToken>();
            VirtualDirectories = new HashSet<VirtualDirectory>();
        }

        public string Username { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
        public string? photoURL { get; set; }
        public virtual ICollection<FileBlob> Files { get; set; }
        public virtual ICollection<UserToken> UserTokens { get; set; }
        public virtual ICollection<VirtualDirectory> VirtualDirectories { get; set; }
    }
}
