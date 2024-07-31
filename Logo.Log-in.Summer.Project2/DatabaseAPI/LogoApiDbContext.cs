 using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace DatabaseAPI
{
    public class LogoApiDbContext : DbContext
    {
        public LogoApiDbContext(DbContextOptions<LogoApiDbContext> options) : base(options) { }

        public DbSet<Product> Products { get; set; }
    }
} 
