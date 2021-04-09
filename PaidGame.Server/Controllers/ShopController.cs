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
        private readonly BoosterManager _boosterManager;

        /// <inheritdoc />
        public ShopController(AccountsManager accountsManager,
            IConfiguration config,
            BoosterManager boosterManager)
        {
            _accountsManager = accountsManager;
            _config = config;
            _boosterManager = boosterManager;
        }

        private string AccountLogin =>
            User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier).Value;

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

            var user = await _accountsManager.GetAccountAsync(AccountLogin);
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
            var user = await _accountsManager.GetAccountAsync(AccountLogin);
            if (user == null) return BadRequest();
            float boosterCost = _config.GetSection("Game").GetValue<float>("BoosterCost");
            if (!(user.MoneyBalance >= boosterCost))
                return BadRequest("Not enough money");
            user.MoneyBalance -= boosterCost;
            await _boosterManager.AddBoosterToAccount(user, 5, 2);
            await _accountsManager.UpdateAccountAsync(user);
            return Ok("Booster bought");
        }
    }
}
