using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса входа
    /// </summary>
    public class LoginParams
    {
        /// <summary>
        /// Id чата с пользователем
        /// </summary>
        [Required]
        public long ChatId { get; set; }

        /// <summary>
        /// Пароль пользователя
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
