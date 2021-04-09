using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса регистрации
    /// </summary>
    public class RegisterParams
    {
        /// <summary>
        /// Логин аккаунта 
        /// </summary>

        [Required]
        public string Login { get; set; }

        /// <summary>
        /// Пароль аккаунта
        /// </summary>
        [Required]
        public string Password { get; set; }
    }
}
