using WebBlogApi.Models;
using Microsoft.EntityFrameworkCore;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlogApi.Data
{
    public class WebBlogApiDbContext: DbContext
    {
        public WebBlogApiDbContext(DbContextOptions<WebBlogApiDbContext> options) :base(options) { }

      
        public DbSet<User> Users { get; set; }
        public DbSet<Blogs> Blogs { get; set; }
       
    }
}
