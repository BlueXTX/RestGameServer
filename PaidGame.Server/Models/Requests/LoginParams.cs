using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса входа
    /// </summary>
    public class LoginParams
    {
        /// <summary>
        /// Логи пользователя
        /// </summary>
        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Пароль пользователя
        /// </summary>
        [Required]
        public string Password { get; set; }

        public LoginParams(string login, string password)
        {
            Login = login;
            Password = password;
        }
    }
}