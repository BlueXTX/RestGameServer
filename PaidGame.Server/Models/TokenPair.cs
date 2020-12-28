using System.ComponentModel.DataAnnotations;

namespace PaidGame.Server.Models
{
    public class TokenPair
    {
        [Key] [Required] public long Id { get; set; }
        [Required] public string AccessToken { get; set; }
        [Required] public string RefreshToken { get; set; }

        public TokenPair(string accessToken, string refreshToken)
        {
            AccessToken = accessToken;
            RefreshToken = refreshToken;
        }
    }
}
