using System;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;

namespace PaidGame.Server.Services
{
    /// <summary>
    /// Контекст приложения
    /// </summary>
    public sealed class ApplicationContext : DbContext
    {
        /// <summary>
        /// Аккаунты пользователей
        /// </summary>
        public DbSet<Account> Accounts { get; set; }

        /// <summary>
        /// Пары токенов авторизации 
        /// </summary>
        public DbSet<TokenPair> TokenPairs { get; set; }

        /// <summary>
        /// Списки социальных сетей пользователей
        /// </summary>
        public DbSet<SocialNetworksList> SocialNetworksList { get; set; }
        
        public DbSet<Booster> Boosters { get; set; }

        /// <summary>
        /// Рейтинговые лиги
        /// </summary>
        public DbSet<League> Leagues { get; set; }

        /// <summary>
        /// Игровые сессии
        /// </summary>
        public DbSet<GameSession> GameSessions { get; set; }

        /// <inheritdoc />
        public ApplicationContext()
        {
            Database.EnsureCreated();
        }

        /// <inheritdoc />
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(
                "server=localhost;user=root;password=root;database=PaidGameDB;",
                new MySqlServerVersion(new Version(5, 7, 24)));
        }
    }
}
