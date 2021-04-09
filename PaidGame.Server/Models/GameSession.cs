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
        /// Логин пользователя, создавшего игру
        /// </summary>
        public string InitiatorLogin { get; set; }

        /// <summary>
        /// Дата начала игры
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата конца игры
        /// </summary>
        public DateTime FinishDate { get; set; }

        public GameSession(string initiatorLogin)
        {
            InitiatorLogin = initiatorLogin;
            StartDate = DateTime.Now;
        }
    }
}
