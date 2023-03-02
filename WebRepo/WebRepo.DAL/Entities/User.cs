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
        public string Username { get; set; }
        public string PasswordSalt { get; set; }
        public string PasswordHash { get; set; }
        public string Email { get; set; }
    }
}
