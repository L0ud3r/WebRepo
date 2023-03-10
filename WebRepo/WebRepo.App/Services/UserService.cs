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
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using WebRepo.App.Interfaces;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Http;

namespace WebRepo.App.Services
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserToken> _userTokenRepository;

        public UserService(IRepository<User> userRepository, IRepository<UserToken> userTokenRepository)
        {
            _userRepository = userRepository;
            _userTokenRepository = userTokenRepository;
        }

        public async Task<IQueryable> Get()
        {
            return _userRepository.Get().Where(x => x.Active == true).AsQueryable();
        }

        public async Task<User> GetbyId(int id)
        {
            return _userRepository.Get().FirstOrDefault(x => x.Id == id && x.Active == true);
        }

        public async Task<User> Create(User user, string password)
        {
            try
            {
                User newUser = new User();

                newUser.Username = user.Username;
                newUser.Email = user.Email;

                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                newUser.PasswordHash = Convert.ToBase64String(passwordHash);
                newUser.PasswordSalt = Convert.ToBase64String(passwordSalt);

                newUser.photoURL = "default_profile_photo.jpg";
                newUser.Active = true;
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;
                newUser.CreatedBy = 0;

                _userRepository.Insert(newUser);
                _userRepository.Save();

                var newUserCreated = _userRepository.Get().OrderBy(x => x.Id).LastOrDefault();

                string folderPath = Path.Combine(Directory.GetCurrentDirectory(), $"..\\WebRepo.DAL\\Photos\\{newUserCreated.Id}");

                if (!Directory.Exists(folderPath))
                {
                    Directory.CreateDirectory(folderPath);
                }

                return newUserCreated;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<User> Edit(User user, User userEdit)
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

        public async Task<User> ChangePhoto(string userEmail, IFormFile file)
        {
            try
            {
                var userEdit = await _userRepository.Get().Where(x => x.Email == userEmail).SingleOrDefaultAsync();

                if (userEdit == null)
                    return null;

                var directoryPath = Path.Combine(Directory.GetCurrentDirectory(), $"..\\WebRepo.DAL\\Photos\\{userEdit.Id}");
                var filePath = Path.Combine(directoryPath, file.FileName);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }
                if (!Directory.Exists(filePath))
                {
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                }

                userEdit.photoURL = file.FileName;
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

        public async Task<User> RecoverPassword(User user, User userEdit, string newPassword)
        {
            try
            {
                CreatePasswordHash(newPassword, out byte[] passwordHash, out byte[] passwordSalt);
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

        public async Task<bool> Delete(int id)
        {
            if (_userRepository.Exists(id))
            {
                _userRepository.Delete(id);
                _userRepository.Save();

                return true;
            }

            return false;
        }

        public async Task<User> Login(string mail, string password)
        {
            try
            {
                //Procurar na database user com email
                var user = _userRepository.Get().Where(x => x.Email == mail && x.Active == true).FirstOrDefault();

                if (!VerifyPasswordHash(password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)) || user == null)
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

        public async Task<string> GenerateToken(int userId)
        {
            var token = Guid.NewGuid().ToString();

            var newUserToken = new UserToken()
            {
                UserId = userId,
                Token = token,
                Expire = DateTime.Now.AddDays(1),
                Active = true
            };

            _userTokenRepository.Insert(newUserToken);
            _userTokenRepository.Save();

            return token;
        }

        public async Task<User> GetUserByToken(string token)
        {
            var user = await _userTokenRepository.Get().Where(x => x.Token == token && x.Expire > DateTime.Now && x.Active).Select(x => x.User).SingleOrDefaultAsync();

            return user;
        }

        public async Task<User> GetUserByEmail(string userEmail) {
            var user = await _userRepository.Get().Where(x => x.Email == userEmail && x.Active).SingleOrDefaultAsync();

            return user;
        }

        public async Task<User> GetLastTokenTemp()
        {
            var user = await _userTokenRepository.Get().Where(x => x.Expire > DateTime.Now && x.Active).Select(x => x.User).OrderBy(x => x.Id).LastOrDefaultAsync();

            return user;
        }
    }
}
