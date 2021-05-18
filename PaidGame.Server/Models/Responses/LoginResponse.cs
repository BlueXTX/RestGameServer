namespace PaidGame.Server.Models.Responses
{
    public class LoginResponse
    {
        /// <summary>
        /// Токен для авторизации на сервере
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Токен для обновления токена доступа
        /// </summary>
        public string RefreshToken { get; set; }

        public LoginResponse(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}