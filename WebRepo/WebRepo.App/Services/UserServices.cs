using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using System.Security.Cryptography;
using WebRepo.App.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace WebRepo.App.Services
{
    public class UserServices
    {
        public static async Task<IQueryable> Get(IRepository<User> _userRepository)
        {
            return _userRepository.Get().Where(x => x.Active == true).AsQueryable();
        }

        public static async Task<User> GetbyId(IRepository<User> _userRepository, int id)
        {
            return _userRepository.Get().FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public static async Task<User> Create(IRepository<User> _userRepository, UserViewModel user)
        {
            try
            {
                User newUser = new User();

                newUser.Username = user.Username;
                newUser.Email = user.Email;

                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                newUser.PasswordHash = Convert.ToBase64String(passwordHash);
                newUser.PasswordSalt = Convert.ToBase64String(passwordSalt);

                newUser.Active = true;
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;
                newUser.CreatedBy = 0;

                _userRepository.Insert(newUser);
                _userRepository.Save();

                return _userRepository.Get().OrderBy(x => x.Id).LastOrDefault();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<User> Edit(IRepository<User> _userRepository, UserViewModel user, User userEdit)
        {
            try
            {
                userEdit.Username = user.Username;
                userEdit.Email = user.Email;

                userEdit.UpdatedDate = DateTime.Now;

                _userRepository.Update(userEdit);
                _userRepository.Save();

                return userEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<User> RecoverPassword(IRepository<User> _userRepository, UserViewModel user, User userEdit)
        {
            try
            {
                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                userEdit.PasswordHash = Convert.ToBase64String(passwordHash);
                userEdit.PasswordSalt = Convert.ToBase64String(passwordSalt);

                _userRepository.Update(userEdit);
                _userRepository.Save();

                return userEdit;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static async Task<bool> Delete(IRepository<User> _userRepository, int id)
        {
            if (_userRepository.Exists(id))
            {
                _userRepository.Delete(id);
                _userRepository.Save();

                return true;
            }

            return false;
        }

        public static async Task<User> Login(IRepository<User> _userRepository, LoginViewModel data)
        {
            try
            {
                //Procurar na database user com email
                var user = _userRepository.Get().Where(x => x.Email == data.Email && x.Active == true).FirstOrDefault();

                if (!VerifyPasswordHash(data.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)) || user == null)
                {
                    return null;
                }

                return user;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public static void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        public static bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
    }
}
