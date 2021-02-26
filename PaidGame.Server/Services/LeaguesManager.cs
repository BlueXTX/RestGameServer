using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PaidGame.Server.Models;

namespace PaidGame.Server.Services
{
    /// <summary>
    /// Менеджер для работы с рейтинговыми лигами
    /// </summary>
    public class LeaguesManager
    {
        private readonly IConfiguration _config;
        private readonly ApplicationContext _db;
        private List<League> _leagues = new List<League>();

        public LeaguesManager(IConfiguration config,
            ApplicationContext db)
        {
            _config = config;
            _db = db;
            Initialize();
        }

        private async Task Initialize()
        {
            _leagues = _config.GetSection("Leagues").Get<List<League>>();
            foreach (var league in _leagues)
            {
                if (await _db.Leagues.FirstOrDefaultAsync(
                    x => x.Name == league.Name) != default) continue;
                await _db.Leagues.AddAsync(league);
            }

            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Рассчитать и назначить лигу пользователя
        /// </summary>
        /// <param name="account">Аккаунт для рассчета</param>
        /// <returns></returns>
        public async Task CalculateLeague(Account account)
        {
            foreach (var league in _leagues.OrderByDescending(x => x.ScoreRequirement)
                .ToList())
            {
                if (account.Score >= league.ScoreRequirement)
                {
                    var leagueInDb = await _db.Leagues.FirstAsync(x =>
                        x.ScoreRequirement == league.ScoreRequirement);
                    account.LeagueId = leagueInDb.Id;
                    await _db.SaveChangesAsync();
                    break;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="account"></param>
        /// <param name="score"></param>
        /// <returns></returns>
        public async Task AddScoreAndRecalculateLeague(Account account, long score)
        {
            account.Score += score;
            await _db.SaveChangesAsync();
            await CalculateLeague(account);
        }

        /// <summary>
        /// Рассчитать награду за сессию
        /// </summary>
        /// <param name="account">Аккаунт для зачисления</param>
        /// <param name="score">Счет в сессии</param>
        /// <returns></returns>
        public async Task CalculateReward(Account account, long score)
        {
            account.Lives -= 1;
            account.MoneyBalance -=
                (float) score / 10;
            account.RealBalance +=
                (float) score / 10 * _leagues[account.LeagueId + 1].MoneyMultiplier;
        }
    }
}
