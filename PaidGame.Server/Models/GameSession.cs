using System;
using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Модель игровой сессии
    /// </summary>
    public class GameSession
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        public long Id { get; set; }

        /// <summary>
        /// Id пользователя, создавшего игру
        /// </summary>
        public long InitiatorChatId { get; set; }

        /// <summary>
        /// Дата начала игры
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата конца игры
        /// </summary>
        public DateTime FinishDate { get; set; }

        public GameSession(long initiatorChatId)
        {
            InitiatorChatId = initiatorChatId;
            StartDate = DateTime.Now;
        }
    }
}
