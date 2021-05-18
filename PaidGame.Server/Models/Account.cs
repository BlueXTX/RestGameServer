using System;
using System.ComponentModel.DataAnnotations;
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
        /// Логин аккаунта
        /// </summary>

        [Required]
        public string Login { get; set; }

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
        public long BoosterId { get; set; }

        /// <summary>
        /// Id лиги, в которой состоит пользователь
        /// </summary>
        public int LeagueId { get; set; }

        /// <summary>
        /// Реферал-Id пригласившего человека 
        /// </summary>
        public long ReferralId { get; set; }

        public Account(string login,
            string password,
            string nickname,
            int lives,
            long referralId)
        {
            Login = login;
            Password = password;
            Nickname = nickname;
            Lives = lives;
            ReferralId = referralId;
            SocialNetworksList = new SocialNetworksList(login);
        }

        /// <summary>
        /// Конвертировать модель в AccountStats
        /// </summary>
        /// <returns>AccountStats с данными пользователя</returns>
        public AccountStats GetStats()
        {
            return new(Id, Login, Nickname, MoneyBalance, RealBalance, Score, Lives);
        }

        public Referral GetRefferal()
        {
            return new(Nickname);
        }

        /// <inheritdoc />
        public int CompareTo(Account other)
        {
            if (other != null)
            {
                return (int) (Score - other.Score);
            }

            return 0;
        }
    }
}