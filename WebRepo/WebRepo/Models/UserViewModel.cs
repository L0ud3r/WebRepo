using System.ComponentModel.DataAnnotations;

namespace WebRepo.Models
{
    public class UserViewModel : LoginViewModel
    {
        public int? Id { get; set; }
        public string? Username { get; set; }
    }
}
