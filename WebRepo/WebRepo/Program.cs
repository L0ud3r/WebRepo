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

namespace WebRepo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddDbContext<WebRepoAppContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("WebRepoContext") ?? throw new InvalidOperationException("Connection string 'WebRepoContext' not found.")));

            /** Adicionar abaixo as Scopes (repositorios) de cada entidade da database **/

            builder.Services.AddScoped<IRepository<User>, Repository<User, WebRepoAppContext>>();
            builder.Services.AddScoped<IRepository<FileCdn>, Repository<FileCdn, WebRepoAppContext>>();
            builder.Services.AddScoped<IRepository<FileBlob>, Repository<FileBlob, WebRepoAppContext>>();

            /**                                                         **/


            // Add services to the container.

            builder.Services.AddCors(c =>
            {
                c.AddPolicy("AllowOrigin", options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            });

            builder.Services.AddControllers().AddNewtonsoftJson(
                options => {
                    options.SerializerSettings.MaxDepth = 3;
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });

            /** Adicionar abaixo authentication schema **/



            /**                                        **/

            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromDays(30);
                options.Cookie.HttpOnly = true;
            });

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDistributedMemoryCache();

            var app = builder.Build();

            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();

            app.UseSession();

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}