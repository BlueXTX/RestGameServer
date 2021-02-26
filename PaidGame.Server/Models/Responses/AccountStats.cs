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
        public long ChatId { get; set; }

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
        public int Lives { get; set; }

        /// <summary>
        /// Бустер аккаунта
        /// </summary>
        public List<Booster> Boosters { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="chatId">Id чата с аккаунтом</param>
        /// <param name="nickname">Никнейм аккаунта</param>
        /// <param name="moneyBalance">Баланс денег аккаунта</param>
        /// <param name="realBalance">Баланс реалов аккаунта</param>
        /// <param name="score">Количество очков аккаунта</param>
        /// <param name="lives">Количество жизней аккаунта</param>
        /// <param name="boosters">Бустеры аккаунта</param>
        public AccountStats(long chatId,
            string nickname,
            float moneyBalance,
            float realBalance,
            float score,
            int lives,
            List<Booster> boosters
        )
        {
            ChatId = chatId;
            Nickname = nickname;
            MoneyBalance = moneyBalance;
            RealBalance = realBalance;
            Score = score;
            Lives = lives;
            Boosters = boosters;
        }
    }
}
