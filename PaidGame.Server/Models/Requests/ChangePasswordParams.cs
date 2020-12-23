using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    public class ChangePasswordParams
    {
        [Required] public long ChatId { get; set; }

        [Required] public string OldPassword { get; set; }
        [Required] public string NewPassword { get; set; }

        public ChangePasswordParams(long chatId, string oldPassword, string newPassword)
        {
            ChatId = chatId;
            OldPassword = oldPassword;
            NewPassword = newPassword;
        }
    }
}
