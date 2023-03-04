using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using WebRepo.DAL;
using WebRepo.DAL.Entities;

namespace WebRepo.App.Data
{
    public class WebRepoAppContext : DbContext
    {
        public WebRepoAppContext (DbContextOptions<WebRepoAppContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = default!;
        public DbSet<FileCdn> FilesCdn { get; set; } = default!;
        public DbSet<FileBlob> Files { get; set; } = default!;
    }
}
