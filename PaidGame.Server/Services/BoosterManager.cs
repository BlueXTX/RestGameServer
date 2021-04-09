using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;

namespace PaidGame.Server.Services
{
    public class BoosterManager
    {
        private ApplicationContext _db;

        public BoosterManager(ApplicationContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Добавить бустер на аккаунт
        /// </summary>
        /// <param name="account">Аккаунт для добавления</param>
        /// <param name="gamesCount">Кол-во игр</param>
        /// <param name="scoreMultiplier">Множитель счета</param>
        /// <returns></returns>
        public async Task AddBoosterToAccount(Account account,
            int gamesCount,
            float scoreMultiplier)
        {
            await _db.Boosters.AddAsync(new Booster(account.Login, gamesCount,
                scoreMultiplier));
            await _db.SaveChangesAsync();
            var booster =
                await _db.Boosters.FirstOrDefaultAsync(x => x.Login == account.Login);
            account.BoosterId = booster.Id;
            _db.Update(account);
            await _db.SaveChangesAsync();
        }
    }
}
