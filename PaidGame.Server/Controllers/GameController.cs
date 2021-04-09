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
        private string AccountLogin =>
            User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;

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
            var user = await _accountsManager.GetAccountAsync(AccountLogin);
            if (user == default)
            {
                return Ok("User not found");
            }

            if (user.MoneyBalance <= 0)
            {
                return Ok("Not enough money");
            }

            if (user.Lives <= 0)
            {
                return Ok("Not enough lives");
            }

            var game = new GameSession(AccountLogin);
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
            var user = await _accountsManager.GetAccountAsync(AccountLogin);
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
            _db.Update(gameSession);
            await _db.SaveChangesAsync();
            await _leaguesManager.CalculateReward(user, resultParams.Score);
            return Ok();
        }
    }
}
