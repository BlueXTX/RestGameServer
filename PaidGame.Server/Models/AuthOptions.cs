using System.Text;
using Microsoft.IdentityModel.Tokens;

namespace PaidGame.Server.Models
{
    /// <summary>
    /// Опции генерации токенов
    /// </summary>
    public class AuthOptions
    {
        /// <summary>
        /// Создатель токена
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Потребитель токена
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Секретный ключ шифрования
        /// </summary>
        public string Secret { get; set; }
        public string RefreshSecret { get; set; }

        /// <summary>
        /// Время действия токена доступа в секундах
        /// </summary>
        public int AccessTokenLifetime { get; set; }

        /// <summary>
        /// Время действия токена обновления в секундах
        /// </summary>
        public int RefreshTokenLifetime { get; set; }
        
        /// <summary>
        /// Получить симметричный ключ шифрования
        /// </summary>
        /// <returns></returns>
        public SymmetricSecurityKey GetAccessSecurityKey()
        {
            return new(Encoding.ASCII.GetBytes(Secret));
        }
        
        public SymmetricSecurityKey GetRefreshSecurityKey()
        {
            return new(Encoding.ASCII.GetBytes(RefreshSecret));
        }
    }
}
