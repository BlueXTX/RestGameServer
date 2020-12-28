using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models.Requests
{
    public class RefreshTokenRequest
    {
        [Required] public string AccessToken { get; set; }
        [Required] public string RefreshToken { get; set; }

        public RefreshTokenRequest(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
