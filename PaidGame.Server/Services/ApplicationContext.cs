using System;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;

namespace PaidGame.Server
{
    public class ApplicationContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }
        public DbSet<TokenPair> TokenPairs { get; set; }
        public DbSet<SocialNetworksList> SocialNetworksList { get; set; }

        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=root;database=PaidGameDB;",
                new MySqlServerVersion(new Version(5, 7, 24)));
        }
    }
}
