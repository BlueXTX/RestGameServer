using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using PaidGame.Server.Models;
using PaidGame.Server.Services;

namespace PaidGame.Server.Controllers
{
    /// <summary>
    /// Контроллер магазина
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ShopController : ControllerBase
    {
        private readonly AccountsManager _accountsManager;
        private readonly IConfiguration _config;

        /// <inheritdoc />
        public ShopController(AccountsManager accountsManager,
            IConfiguration config)
        {
            _accountsManager = accountsManager;
            _config = config;
        }

        private long AccountChatId =>
            Convert.ToInt64(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

        /// <summary>
        /// Купить попытку игры
        /// </summary>
        /// <param name="amount">Количество жизней для покупки</param>
        /// <returns></returns>
        [Route("BuyGameChance")]
        [HttpGet]
        public async Task<IActionResult> BuyGameChance(int amount)
        {
            if (amount <= 0)
            {
                return BadRequest("Invalid amount");
            }

            var user = await _accountsManager.GetAccountAsync(AccountChatId);
            if (user == null) return BadRequest();
            float liveCost = _config.GetSection("Game").GetValue<float>("LiveCost");
            if (!(user.MoneyBalance >= liveCost * amount))
                return BadRequest("Not enough money");
            user.MoneyBalance -= liveCost * amount;
            user.Lives += amount;
            await _accountsManager.UpdateAccountAsync(user);
            return Ok("Live bought");
        }

        /// <summary>
        /// Купить бустер
        /// </summary>
        /// <returns></returns>
        [Route("BuyBooster")]
        [HttpGet]
        public async Task<IActionResult> BuyBooster()
        {
            var user = await _accountsManager.GetAccountAsync(AccountChatId);
            if (user == null) return BadRequest();
            float boosterCost = _config.GetSection("Game").GetValue<float>("BoosterCost");
            if (!(user.MoneyBalance >= boosterCost))
                return BadRequest("Not enough money");
            user.MoneyBalance -= boosterCost;
            user.Booster = new Booster(5, 2);
            await _accountsManager.UpdateAccountAsync(user);
            return Ok("Booster bought");
        }
    }
}
