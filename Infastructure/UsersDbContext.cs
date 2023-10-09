using Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infastructure
{
    public class UsersDbContext : DbContext
    {
        public UsersDbContext(DbContextOptions<UsersDbContext> options) : base(options) { }

        public virtual DbSet<User> User { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //EnsureHasData(builder);
            builder.Entity<User>().ToTable("UserList", "dbo");
            base.OnModelCreating(builder);
        }


       
    }

}

