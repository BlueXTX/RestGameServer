using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса входа
    /// </summary>
    public class LoginParams
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

        public LoginParams(long chatId, string password)
        {
            ChatId = chatId;
            Password = password;
        }
    }
}
