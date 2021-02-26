using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Requests;
using PaidGame.Server.Services;

namespace PaidGame.Server.Controllers
{
    /// <summary>
    /// Контроллер игровых сессий
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GameController : ControllerBase
    {
        private long AccountChatId =>
            Convert.ToInt64(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

        private readonly ApplicationContext _db;
        private readonly AccountsManager _accountsManager;
        private readonly LeaguesManager _leaguesManager;

        /// <inheritdoc />
        public GameController(AccountsManager accountsManager,
            ApplicationContext db,
            LeaguesManager leaguesManager)
        {
            _db = db;
            _leaguesManager = leaguesManager;
            _accountsManager = accountsManager;
        }

        /// <summary>
        /// Запрос на начало игры
        /// </summary>
        /// <returns></returns>
        [Route("StartGame")]
        [HttpPost]
        public async Task<IActionResult> StartGame()
        {
            var user = await _accountsManager.GetAccountAsync(AccountChatId);
            if (user == default)
            {
                return BadRequest("User not found");
            }

            if (user.MoneyBalance <= 0)
            {
                return BadRequest("Not enough money");
            }

            var game = new GameSession(AccountChatId);
            await _db.GameSessions.AddAsync(game);
            await _db.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Запрос на окончание игры
        /// </summary>
        /// <param name="resultParams">Результат игры</param>
        /// <returns></returns>
        [Route("FinishGame")]
        [HttpPost]
        public async Task<IActionResult> FinishGame(GameResultParams resultParams)
        {
            var user = await _accountsManager.GetAccountAsync(AccountChatId);
            if (user == default)
            {
                return BadRequest("User not found");
            }

            var gameSession =
                _db.GameSessions.ToListAsync().Result
                    .OrderByDescending(x => x.StartDate).FirstOrDefault();

            if (gameSession == default)
            {
                return BadRequest("Game not found");
            }

            gameSession.FinishDate = DateTime.Now;
            await _leaguesManager.CalculateReward(user, resultParams.Score);
            await _leaguesManager.AddScoreAndRecalculateLeague(user, resultParams.Score);
            return Ok();
        }
    }
}
