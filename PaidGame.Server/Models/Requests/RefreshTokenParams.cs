using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    /// <summary>
    /// Параметры запрос на обновление токена
    /// </summary>
    public class RefreshTokenRequest
    {
        /// <summary>
        /// Токен доступа пользователя
        /// </summary>
        [Required]
        public string AccessToken { get; set; }

        /// <summary>
        /// Рефреш токен пользователя
        /// </summary>
        [Required]
        public string RefreshToken { get; set; }

        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
