using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.Json;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using Microsoft.EntityFrameworkCore;
using NuGet.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.Extensions.Logging;
using WebRepo.App.Services;
using WebRepo.App.Models;
using WebRepo.App.Interfaces;
using System.Net;
using WebRepo.App.Migrations;

namespace WebRepo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IRepository<User> _userRepository;
        private readonly IUserService _userService;
        private readonly ILogger<UserController> _logger;

        public UserController(IRepository<User> userRepository, ILogger<UserController> logger, IUserService userService)
        {
            _userRepository = userRepository;
            _logger = logger;
            _userService = userService;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var usersList = await _userService.Get();

            return new JsonResult(usersList);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Details(int id)
        {
            var user = await _userService.GetbyId(id);

            return new JsonResult(user);
        }

        [HttpGet("token")]
        public async Task<IActionResult> GetUserByToken()
        {
            var token = HttpContext.Request.Headers.Authorization.FirstOrDefault();

            var userInfo = _userService.GetUserByToken(token).Result;

            return new JsonResult(userInfo);
        }

        [HttpPost]
        public async Task<IActionResult> Create([Bind("Username, Email, Password")] UserViewModel user)
        {
            if(_userRepository.Get().Where(x => x.Username == user.Username) != null)
                return new JsonResult(false) { StatusCode = 400, Value = "Username already exists!" };
            else if (_userRepository.Get().Where(x => x.Email == user.Email) != null)
                 return new JsonResult(false) { StatusCode = 400, Value = "Email already exists!" };

            User newUser = new User();

            newUser.Username = user.Username;
            newUser.Email = user.Email;
            string password = user.Password;

            var userCreated = await _userService.Create(newUser, password);

            if (userCreated == null)
                return new JsonResult(false) { StatusCode = 400, Value = "Error on creating user" };

            return new JsonResult(true) { StatusCode = 200, Value = userCreated };
        }

        [HttpPatch]
        public async Task<IActionResult> Edit([Bind("Id,Username,Email")] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                User userEdit = await _userService.GetbyId((int)user.Id);

                if (userEdit == null)
                    return NotFound();

                User editUser = new User();

                editUser.Username = user.Username;
                editUser.Email = user.Email;

                var userEditted = await _userService.Edit(editUser, userEdit);

                if (userEditted == null)
                    return new JsonResult(false) { StatusCode = 400, Value = "Error on editting user" };

                return new JsonResult(true) { StatusCode = 200, Value = userEditted };
            }
            return BadRequest();
        }

        [HttpPatch("recoverpassword")]
        public async Task<IActionResult> RecoverPassword([Bind("Id,Email,Password")] UserViewModel user)
        {
            if (ModelState.IsValid)
            {
                User userEdit = await _userService.GetbyId((int)user.Id);

                if (userEdit == null)
                    return NotFound();

                User currentUser = new User();

                currentUser.Email = user.Email;
                string newPassword = user.Password;

                var userEditted = await _userService.RecoverPassword(currentUser, userEdit, newPassword);

                if(userEditted == null)
                    return new JsonResult(false) { StatusCode = 400, Value = "Error on recovering password" };

                return new JsonResult(true) { StatusCode = 200, Value = userEditted };
            }
            return BadRequest();
        }

        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _userService.Delete(id);

            if(result == false)
                return new JsonResult(false) { StatusCode = 404, Value = result };

            return new JsonResult(true) { StatusCode = 200, Value = result };
        }

        // GET: UserController/Delete/5
        [HttpDelete("remove")]
        public async Task<IActionResult> Remove(int id)
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([Bind("Email,Password")] LoginViewModel data)
        {
            var user = await _userService.Login(data.Email, data.Password);

            if (user == null)
                return NotFound();

            string token = await _userService.GenerateToken(user.Id);

            return new JsonResult(true) { StatusCode = 200, Value = new { Success = true, token = token } };

            //var claims = new List<Claim>
            //{
            //    new Claim(ClaimTypes.Email, user.Email),
            //    new Claim(ClaimTypes.Name, user.Username),
            //    new Claim(ClaimTypes.Role, "User")
            //};

            //var claimsIdentity = new ClaimsIdentity(
            //    claims, CookieAuthenticationDefaults.AuthenticationScheme);

            //var authProperties = new AuthenticationProperties
            //{
            //    AllowRefresh = true,
            //    // Refreshing the authentication session should be allowed.

            //    //ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            //    // The time at which the authentication ticket expires. A 
            //    // value set here overrides the ExpireTimeSpan option of 
            //    // CookieAuthenticationOptions set with AddCookie.

            //    IsPersistent = true,
            //    // Whether the authentication session is persisted across 
            //    // multiple requests. When used with cookies, controls
            //    // whether the cookie's lifetime is absolute (matching the
            //    // lifetime of the authentication ticket) or session-based.

            //    IssuedUtc = DateTime.Now
            //    // The time at which the authentication ticket was issued.

            //    //RedirectUri = <string>
            //    // The full path or absolute URI to be used as an http 
            //    // redirect response value.
            //};

            //await HttpContext.SignInAsync(
            //    CookieAuthenticationDefaults.AuthenticationScheme,
            //    new ClaimsPrincipal(claimsIdentity),
            //    authProperties);

            //// Check the response headers for the Set-Cookie header
            //var responseCookies = Response.Headers["Set-Cookie"].ToString();

            //if (responseCookies.Contains(".AspNetCore.Application.Id"))
            //{
            //    _logger.LogInformation("Authentication cookie registered successfully.");

            //    // Split the cookie into individual fields
            //    var fields = responseCookies.Split("; ");

            //    foreach (var field in fields)
            //    {
            //        Console.WriteLine(field);
            //    }

            //    return new JsonResult(true) { StatusCode = 200, Value = fields };
            //}
                    
            //_logger.LogWarning("Authentication cookie not found in response headers.");

            //return new JsonResult(false) { StatusCode = 404, Value = "Authentication cookie not found in response headers." };
        }

        //METER EXPIRE DATE PARA ONTEM
        [HttpPost("logout")]
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
    }
}
