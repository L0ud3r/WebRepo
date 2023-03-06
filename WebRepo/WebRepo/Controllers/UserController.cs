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
using NuGet.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Logging;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly ILogger<UserController> _logger;

        public UserController(IRepository<User> userRepository, ILogger<UserController> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        // GET: UserController
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return new JsonResult(_userRepository.Get().Where(x => x.Active == true).AsQueryable());
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            return new JsonResult(_userRepository.Get().FirstOrDefault(x => x.Id == id && x.Active == true));
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Username, Email, Password")] UserViewModel user)
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

        [HttpPatch]
        public async Task<IActionResult> Edit([Bind("Id,Username,Email,Password")] UserViewModel user)
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

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
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
        public async Task<IActionResult> Remove(int id)
        {
            return View();
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel data)
        {
            try
            {
                //Procurar na database user com email
                var user = _userRepository.Get().Where(x => x.Email == data.Email && x.Active == true).FirstOrDefault();

                if (user != null)
                {
                    if (!VerifyPasswordHash(data.Password, Convert.FromBase64String(user.PasswordHash), Convert.FromBase64String(user.PasswordSalt)))
                    {
                        return new JsonResult(false) { StatusCode = 400, Value = "Wrong Password" };
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "User"),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        AllowRefresh = true,
                        // Refreshing the authentication session should be allowed.

                        //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
                        // The time at which the authentication ticket expires. A 
                        // value set here overrides the ExpireTimeSpan option of 
                        // CookieAuthenticationOptions set with AddCookie.

                        IsPersistent = true,
                        // Whether the authentication session is persisted across 
                        // multiple requests. When used with cookies, controls
                        // whether the cookie's lifetime is absolute (matching the
                        // lifetime of the authentication ticket) or session-based.

                        IssuedUtc = DateTime.Now
                        // The time at which the authentication ticket was issued.

                        //RedirectUri = <string>
                        // The full path or absolute URI to be used as an http 
                        // redirect response value.
                    };

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    // Check the response headers for the Set-Cookie header
                    var responseCookies = Response.Headers["Set-Cookie"].ToString();

                    if (responseCookies.Contains(".AspNetCore.Application.Id"))
                    {
                        _logger.LogInformation("Authentication cookie registered successfully.");
                    }
                    else
                    {
                        _logger.LogWarning("Authentication cookie not found in response headers.");
                    }

                    return new JsonResult(true) { StatusCode = 200, Value = responseCookies };
                }

                return new JsonResult(false) { StatusCode = 400, Value = "Wrong Email" };
            }
            catch (Exception e)
            {
                return new JsonResult(e.Message);
            }
        }

        [HttpPost("Logout")]
        public async Task<IActionResult> Logout()
        {
            if (User.Identity.IsAuthenticated)
            {
                // Clear the existing external cookie
                await HttpContext.SignOutAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme);

                return new JsonResult(true) { StatusCode = 200, Value = "Logged out successfully" };
            }

            return new JsonResult(false) { StatusCode = 400, Value = "User already logged out" };
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
