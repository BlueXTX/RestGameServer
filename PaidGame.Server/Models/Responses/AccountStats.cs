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
        /// 
        /// </summary>
        /// <param name="chatId">Id чата с аккаунтом</param>
        /// <param name="nickname">Никнейм аккаунта</param>
        /// <param name="moneyBalance">Баланс денег аккаунта</param>
        /// <param name="realBalance">Баланс реалов аккаунта</param>
        /// <param name="score">Количество очков аккаунта</param>
        public AccountStats(long chatId,
            string nickname,
            float moneyBalance,
            float realBalance,
            float score
        )
        {
            ChatId = chatId;
            Nickname = nickname;
            MoneyBalance = moneyBalance;
            RealBalance = realBalance;
            Score = score;
        }
    }
}
