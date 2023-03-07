using WebRepo.DAL.Default;

namespace WebRepo.DAL.Entities
{
    public class UserToken : DefaultEntity
    {
        public int UserId { get; set; }
        public string Token { get; set; }
        public DateTime Expire { get; set; }
        public virtual User User { get; set; }
    }
}
