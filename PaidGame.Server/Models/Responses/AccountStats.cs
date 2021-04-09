using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PaidGame.Server.Models.Responses
{
    public class AccountStats
    {
        /// <summary>
        /// Id чата с аккаунтом
        /// </summary>
        [NotNull]
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Никнейм аккаунта
        /// </summary>
        [NotNull]
        [Required]
        public string Nickname { get; set; }

        /// <summary>
        /// Баланс денег аккаунта
        /// </summary>
        [NotNull]
        [Required]
        public float MoneyBalance { get; set; }

        /// <summary>
        /// Баланс реалов аккаунта
        /// </summary>
        [NotNull]
        [Required]
        public float RealBalance { get; set; }

        /// <summary>
        /// Количество очков аккаунта
        /// </summary>
        [NotNull]
        [Required]
        public float Score { get; set; }

        /// <summary>
        /// Количество жизней аккаунта
        /// </summary>
        [NotNull]
        [Required]
        public int Lives { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login">Id чата с аккаунтом</param>
        /// <param name="nickname">Никнейм аккаунта</param>
        /// <param name="moneyBalance">Баланс денег аккаунта</param>
        /// <param name="realBalance">Баланс реалов аккаунта</param>
        /// <param name="score">Количество очков аккаунта</param>
        /// <param name="lives">Количество жизней аккаунта</param>
        public AccountStats(string login,
            string nickname,
            float moneyBalance,
            float realBalance,
            float score,
            int lives
        )
        {
            Login = login;
            Nickname = nickname;
            MoneyBalance = moneyBalance;
            RealBalance = realBalance;
            Score = score;
            Lives = lives;
        }
    }
}
