using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Requests;

namespace PaidGame.Server
{
    public class TokensManager
    {
        private ApplicationContext _db;

        public TokensManager(ApplicationContext db)
        {
            _db = db;
        }

        public async Task<bool> AddTokenPairAsync(TokenPair pair)
        {
            if (await GetTokenPairAsync(pair.RefreshToken) != null) return false;
            await _db.TokenPairs.AddAsync(pair);
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="refreshTokenRequest"></param>
        /// <returns></returns>
        public async Task<bool> TokenPairExistsAsync(
            RefreshTokenRequest refreshTokenRequest)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(
                x => x.AccessToken == refreshTokenRequest.AccessToken &&
                     x.RefreshToken == refreshTokenRequest.RefreshToken) != null;
        }

        /// <summary>
        /// Проверить наличие пары токенов
        /// </summary>
        /// <param name="refreshToken">refresh token для проверки</param>
        /// <returns></returns>
        public async Task<bool> TokenPairExistsAsync(
            string refreshToken)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(
                x => x.RefreshToken == refreshToken) != null;
        }

        /// <summary>
        /// Найти пару токенов по refresh token'у
        /// </summary>
        /// <param name="refreshToken">refresh token для поиска</param>
        /// <returns></returns>
        public async Task<TokenPair> GetTokenPairAsync(string refreshToken)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(t =>
                t.RefreshToken == refreshToken);
        }
        
        /// <summary>
        /// Удалить refresh token из БД
        /// </summary>
        /// <param name="refreshToken">Токен для удаления</param>
        /// <returns></returns>
        public async Task DeleteTokenAsync(string refreshToken)
        {
            if (!await TokenPairExistsAsync(refreshToken)) return;

            _db.TokenPairs.Remove(await GetTokenPairAsync(refreshToken));
            await _db.SaveChangesAsync();
        }
    }
}
