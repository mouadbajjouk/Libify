using Libify.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Libify.Data
{
    public class LibifyContext : IdentityDbContext<ApplicationUser> //it is using DbContext
    {
        public LibifyContext(DbContextOptions<LibifyContext> options) : base(options)
        {

        }

        public DbSet<Books> Books { get; set; }

        public DbSet<Language> Language { get; set; }

        public DbSet<BookGallery> BookGallery { get; set; }
    }
}
