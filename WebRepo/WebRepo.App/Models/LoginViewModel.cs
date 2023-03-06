using System.ComponentModel.DataAnnotations;

namespace WebRepo.App.Models
{
    public class LoginViewModel
    {
        public string? Email { get; set; }
        public string? Password { get; set; }
    }
}
