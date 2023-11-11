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
        public UsersDbContext() { }

        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<LoginCreds> LoginCreds { get; set; }
        public virtual DbSet<RefreshToken> RefreshToken { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            //EnsureHasData(builder);
            builder.Entity<User>().ToTable("UserList", "dbo");
            builder.Entity<User>()
        .Property(u => u.Id)
        .ValueGeneratedOnAdd();

            builder.Entity<LoginCreds>().HasNoKey().ToTable("LoginCreds", "dbo");
            builder.Entity<RefreshToken>().ToTable("RefreshToken", "dbo");
            base.OnModelCreating(builder);
        }


       
    }

}

