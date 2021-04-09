using System;
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
                    _db.Update(account);
                    await _db.SaveChangesAsync();
                    break;
                }
            }
        }

        /// <summary>
        /// Рассчитать награду за сессию
        /// </summary>
        /// <param name="account">Аккаунт для зачисления</param>
        /// <param name="score">Счет в сессии</param>
        /// <returns></returns>
        public async Task CalculateReward(Account account, float score)
        {
            float scoreCost = _config.GetSection("Game").GetValue<float>("ScoreCost");

            var booster =
                await _db.Boosters.FirstOrDefaultAsync(x => x.Login == account.Login);
            if (booster != default)
            {
                if (booster.GamesCount > 0)
                {
                    booster.GamesCount -= 1;

                    _db.Update(booster);
                }
                else
                {
                    _db.Remove(booster);
                    account.BoosterId = 0;
                    await _db.SaveChangesAsync();
                }
            }

            account.Lives -= 1;
            account.MoneyBalance -= score * scoreCost;

            if (booster != default)
            {
                account.RealBalance +=
                    score * scoreCost * _leagues[Math.Clamp(account.LeagueId - 1, 0, _leagues.Count - 1)].MoneyMultiplier *
                    booster.ScoreMultiplier;
                account.Score += score * booster.ScoreMultiplier;
            }
            else
            {
                account.RealBalance +=
                    score * scoreCost *
                    _leagues[Math.Clamp(account.LeagueId - 1, 0, _leagues.Count - 1)]
                        .MoneyMultiplier;
                account.Score += score;
            }

            _db.Update(account);
            await _db.SaveChangesAsync();
            await CalculateLeague(account);
        }
    }
}
