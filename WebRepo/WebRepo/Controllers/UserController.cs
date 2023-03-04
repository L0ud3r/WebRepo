using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using WebRepo.Models;
using Microsoft.EntityFrameworkCore;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private IRepository<User> _userRepository;

        public UserController(IRepository<User> userRepository)
        {
            _userRepository = userRepository;
        }


        // GET: UserController
        [HttpGet]
        public IActionResult Get()
        {
            return new JsonResult(_userRepository.Get());
        }

        // GET: UserController/Details/5
        [HttpGet("{id}")]
        public IActionResult Details(int id)
        {
            return new JsonResult(_userRepository.Get().FirstOrDefault(x => x.Id == id && x.Active == true));
        }

        // GET: UserController/Create
        [HttpPost]
        public IActionResult Create([Bind("Username, Email, Password")] UserViewModel user)
        {
            User newUser = new User();

            try
            {
                newUser.Username = user.Username;
                newUser.Email = user.Email;

                string password = "password";

                CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);
                newUser.PasswordHash = Convert.ToBase64String(passwordHash);
                newUser.PasswordSalt = Convert.ToBase64String(passwordSalt);

                newUser.Active = true;
                newUser.CreatedDate = DateTime.Now;
                newUser.UpdatedDate = DateTime.Now;
                newUser.CreatedBy = 0;

                _userRepository.Insert(newUser);
                _userRepository.Save();

                string jsonObject = JsonSerializer.Serialize(
                    _userRepository.Get().OrderBy(x => x.Id).LastOrDefault(),
                    new JsonSerializerOptions
                    {
                        ReferenceHandler = ReferenceHandler.Preserve
                    });

                return new JsonResult(jsonObject);
            }
            catch(Exception ex)
            {
                return new JsonResult(ex.Message);
            }
            
        }

        // GET: UserController/Edit/5
        [HttpPatch]
        public IActionResult Edit([Bind("Id,Username,Email,Password")] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    User userEdit = _userRepository.Get().FirstOrDefault(x => x.Id == user.Id && x.Active == true);

                    if (userEdit == null)
                        return NotFound();

                    userEdit.Username = user.Username;
                    userEdit.Email = user.Email;

                    userEdit.UpdatedDate = DateTime.Now;

                    _userRepository.Update(userEdit);
                    _userRepository.Save();

                    string jsonObject = JsonSerializer.Serialize(
                        userEdit,
                        new JsonSerializerOptions
                        {
                            ReferenceHandler = ReferenceHandler.Preserve
                        });

                    return Json(jsonObject);
                }
                catch (DbUpdateConcurrencyException)
                {
                    return NotFound();
                }
            }
            return BadRequest();
        }

        // GET: UserController/Delete/5
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            if (_userRepository.Exists(id))
            {
                _userRepository.Delete(id);
                _userRepository.Save();

                return new JsonResult("Success");
            }

            return NotFound();
        }

        // GET: UserController/Delete/5
        [HttpDelete("Remove")]
        public IActionResult Remove(int id)
        {
            return View();
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
