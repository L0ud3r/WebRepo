using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using WebRepo.App.Data;
using WebRepo.DAL.Entities;
using WebRepo.Infra;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using System.Text;
using Microsoft.AspNetCore.Builder;
using WebRepo.App.Services;
using WebRepo.App.Interfaces;
using Microsoft.Extensions.Options;
using WebRepo.Middleware;
using Microsoft.AspNetCore.Http.Features;

namespace WebRepo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var cookiePolicyOptions = new CookiePolicyOptions
            {
                MinimumSameSitePolicy = SameSiteMode.Strict,
            };

            builder.Services.AddDbContext<WebRepoAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebRepoContext") ?? throw new InvalidOperationException("Connection string 'WebRepoContext' not found.")));

            /** Adicionar abaixo as Scopes (repositorios) de cada entidade da database **/

            builder.Services.AddScoped<IRepository<User>, Repository<User, WebRepoAppContext>>();
            builder.Services.AddScoped<IRepository<FileBlob>, Repository<FileBlob, WebRepoAppContext>>();
            builder.Services.AddScoped<IRepository<UserToken>, Repository<UserToken, WebRepoAppContext>>();
            builder.Services.AddScoped<IRepository<VirtualDirectory>, Repository<VirtualDirectory, WebRepoAppContext>>();

            builder.Services.AddScoped<IFileService, FileService>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVirtualDirectoryService, VirtualDirectoryService>();

            builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            /**                                                         **/

            // Add services to the container.

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
                c.AddPolicy("AllowAngularOrigin",
                builder => builder.WithOrigins("http://localhost:4200")
                   .AllowCredentials()
                   .AllowAnyHeader()
                   .AllowAnyMethod());
            });

            builder.Services.AddControllers().AddNewtonsoftJson(
                options => {
                    options.SerializerSettings.MaxDepth = 3;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            builder.Services.Configure<FormOptions>(options =>
            {
                options.ValueLengthLimit = int.MaxValue;
                options.MultipartBodyLengthLimit = long.MaxValue;
            });

            builder.Services.AddLogging(loggingBuilder =>
            {
                loggingBuilder.AddConsole();
                loggingBuilder.AddDebug();
            });

            /** Adicionar abaixo authentication schema **/

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
            .AddCookie(options =>
            {
                options.Cookie.Name = ".AspNetCore.Application.Id";
                options.ExpireTimeSpan = TimeSpan.FromMinutes(60);
                options.SlidingExpiration = true;
                options.AccessDeniedPath = "/Forbidden/";
                options.Cookie.Path = "/";
                options.Cookie.HttpOnly = false;
            });

            //builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie();

            /**                                        **/

            //builder.Services.AddSession(options => {
            //    options.IdleTimeout = TimeSpan.FromDays(30);
            //    options.Cookie.HttpOnly = true;
            //});

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

            //app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());


            app.UseCors("AllowAngularOrigin");

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseCookiePolicy(cookiePolicyOptions);

            app.UseRouting();

            app.UseAuthentication();

            //app.UseSession();

            app.UseAuthorization();

            app.UseCustomAuth();

            app.MapDefaultControllerRoute();

            app.MapControllers();

            app.Run();

            // Retrieve the ILogger instance
            var logger = app.Services.GetRequiredService<ILogger<Program>>();
            logger.LogInformation("Application started");
        }
    }
}