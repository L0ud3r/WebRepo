using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebRepo.DAL.Entities;

namespace WebRepo.App.Interfaces
{
    public interface IUserService
    {
        Task<IQueryable> Get();
        Task<User> GetbyId(int id);
        Task<User> Create(User user, string password);
        Task<User> Edit(User user, User userEdit);
        Task<User> RecoverPassword(User user, User userEdit, string newPassword);
        Task<bool> Delete(int id);
        Task<User> Login(string email, string password);
        Task<string> GenerateToken(int userId);
        Task<User> GetUserByToken(string token);
        Task<User> GetUserByEmail(string userEmail);
        Task<User> GetLastTokenTemp();
    }
}
