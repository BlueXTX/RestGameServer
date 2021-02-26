using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Модель пары токенов
    /// </summary>
    public class TokenPair
    {
        /// <summary>
        /// Id
        /// </summary>
        [Key]
        [Required]
        public long Id { get; set; }

        /// <summary>
        /// Токен доступа
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// Рефреш токен
        /// </summary>
        [Required] public string RefreshToken { get; set; }

        public TokenPair(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
