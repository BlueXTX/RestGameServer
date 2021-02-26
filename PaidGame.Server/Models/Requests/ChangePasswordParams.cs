using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запроса смены пароля
    /// </summary>
    public class ChangePasswordParams
    {
        /// <summary>
        /// Id чата с пользователем
        /// </summary>
        [Required]
        public long ChatId { get; set; }

        /// <summary>
        /// Старый пароль
        /// </summary>
        [Required]
        public string OldPassword { get; set; }

        /// <summary>
        /// Новый пароль
        /// </summary>
        [Required]
        public string NewPassword { get; set; }
        
        public ChangePasswordParams(long chatId, string oldPassword, string newPassword)
        {
            ChatId = chatId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
