using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса регистрации
    /// </summary>
    public class RegisterParams
    {
        /// <summary>
        /// Id чата с аккаунтом 
        /// </summary>

        [Required]
        public long ChatId { get; set; }

        /// <summary>
        /// Пароль аккаунта
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
