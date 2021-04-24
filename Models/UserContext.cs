using System;
using Microsoft.EntityFrameworkCore;

namespace Zembil.Models
{
    public class UserContext : DbContext
    {
        public UserContext(DbContextOptions<UserContext> options) : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<User> Users { get; set; }

    }

}
// postgresql://postgres:niko1122@localhost/Zembil