using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using PaidGame.Server.Models.Responses;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Модель пользователя в БД
    /// </summary>
    public class Account : IComparable<Account>
    {
        /// <summary>
        /// Уникальный Id аккаунта
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Id чата с аккаунтом
        /// </summary>

        [Required]
        public long ChatId { get; set; }

        /// <summary>
        /// Никнейм аккаунта
        /// </summary>

        public string Nickname { get; set; }

        /// <summary>
        /// Пароль аккаунта
        /// </summary>
        [Required]
        public string Password { get; set; }

        /// <summary>
        /// Список социальных ссылок на социальные сети пользователя
        /// </summary>
        public SocialNetworksList SocialNetworksList { get; set; }

        /// <summary>
        /// Баланс денег аккаунта
        /// </summary>
        public float MoneyBalance { get; set; }

        /// <summary>
        /// Баланс реалов аккаунта
        /// </summary>
        public float RealBalance { get; set; }

        /// <summary>
        /// Количество очков аккаунта
        /// </summary>
        public float Score { get; set; }

        /// <summary>
        /// Количество жизней аккаунта
        /// </summary>
        public int Lives { get; set; }

        /// <summary>
        /// Активные бустеры аккаунта
        /// </summary>
        public Booster Booster { get; set; }

        /// <summary>
        /// Id лиги, в которой состоит пользователь
        /// </summary>
        public int LeagueId { get; set; }

        public Account(long chatId, string password, string nickname)
        {
            ChatId = chatId;
            Password = password;
            Nickname = nickname;
            SocialNetworksList = new SocialNetworksList(chatId);
        }

        public Account(long chatId, string password, string nickname, Booster booster)
        {
            ChatId = chatId;
            Password = password;
            Nickname = nickname;
            SocialNetworksList = new SocialNetworksList(chatId);
            Booster = booster;
        }

        /// <summary>
        /// Конвертировать модель в AccountStats
        /// </summary>
        /// <returns>AccountStats с данными пользователя</returns>
        public AccountStats GetStats()
        {
            return new(ChatId, Nickname, MoneyBalance, RealBalance, Score, Lives,
                new List<Booster>());
        }

        /// <inheritdoc />
        public int CompareTo(Account? other)
        {
            if (other != null)
            {
                return (int) (Score - other.Score);
            }

            return 0;
        }
    }
}
