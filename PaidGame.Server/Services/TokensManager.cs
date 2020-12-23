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

        public async Task<bool> TokenPairExistsAsync(
            RefreshTokenRequest refreshTokenRequest)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(
                x => x.AccessToken == refreshTokenRequest.AccessToken &&
                     x.RefreshToken == refreshTokenRequest.RefreshToken) != null;
        }

        public async Task<bool> TokenPairExistsAsync(
            string refreshToken)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(
                x => x.RefreshToken == refreshToken) != null;
        }

        public async Task<TokenPair> GetTokenPairAsync(string refreshToken)
        {
            return await _db.TokenPairs.FirstOrDefaultAsync(t =>
                t.RefreshToken == refreshToken);
        }

        public async Task<bool> DeleteTokenAsync(string refreshToken)
        {
            if (!await TokenPairExistsAsync(refreshToken)) return false;

            _db.TokenPairs.Remove(await GetTokenPairAsync(refreshToken));
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
