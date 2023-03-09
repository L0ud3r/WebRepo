using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using WebRepo.App.Interfaces;
using WebRepo.App.Services;

namespace WebRepo.Middleware
{
    public static class AuthenticateMiddlewareExtension
    {
        public static IApplicationBuilder UseCustomAuth(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticateMiddleware>();
        }

        public class AuthenticateMiddleware
        {
            private readonly RequestDelegate _next;

            public AuthenticateMiddleware(RequestDelegate next) 
            {
                _next = next;
            }

            public Task Invoke(HttpContext context, IUserService userService)
            {
                var token = context.Request.Headers.Authorization.FirstOrDefault();

                var user = userService.GetUserByToken(token).Result;

                if (user == null)
                    user = userService.GetLastTokenTemp().Result;

                if (user != null) {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Email, user.Email),
                        new Claim(ClaimTypes.Name, user.Username),
                        new Claim(ClaimTypes.Role, "User")
                    };

                    context.User = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));

                    context.SignInAsync(context.User).Wait();
                }

                //Caso esteja autenticado
                return _next(context);

                ////Caso não esteja autenticado
                //return Task.Run(() => new UnauthorizedResult());
            }
        }
    }
}
