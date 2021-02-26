using System;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Responses;
using PaidGame.Server.Services;

namespace PaidGame.Server.Controllers
{
    /// <summary>
    /// Контроллер для обработки данных игрока
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StatsController : ControllerBase
    {
        private readonly ApplicationContext _db;

        private long AccountChatId =>
            Convert.ToInt64(User.Claims.Single(c => c.Type == ClaimTypes.NameIdentifier)
                .Value);

        private readonly AccountsManager _accountsManager;

        /// <inheritdoc />
        public StatsController(ApplicationContext db, AccountsManager accountsManager)
        {
            _db = db;
            _accountsManager = accountsManager;
        }

        /// <summary>
        /// Получить статистику аккаунта
        /// </summary>
        /// <returns></returns>
        [Route("GetUserStats")]
        [HttpGet]
        public async Task<AccountStats> GetUserStats()
        {
            var account = await _accountsManager.GetAccountAsync(AccountChatId);
            return account.GetStats();
        }

        /// <summary>
        /// Загрузить аватар для аккаунта
        /// </summary>
        /// <param name="avatar">Фото в формате jpg</param>
        /// <returns></returns>
        [Route("UploadUserAvatar")]
        [HttpPost]
        public async Task<IActionResult> UploadUserAvatar(IFormFile avatar)
        {
            if (Path.GetExtension(avatar.FileName) == ".jpg")
            {
                await using var fs = new FileStream(
                    AppContext.BaseDirectory + "/avatars/" + AccountChatId + ".jpg",
                    FileMode.Create);
                await avatar.CopyToAsync(fs);
                return Ok("Avatar uploaded");
            }

            return BadRequest();
        }

        /// <summary>
        /// Получить аватар аккаунта
        /// </summary>
        /// <returns>Аватар</returns>
        [Route("GetUserAvatar")]
        [HttpGet]
        public async Task<FileResult> GetUserAvatar()
        {
            string path = AppContext.BaseDirectory +
                          "/avatars/" + AccountChatId +
                          ".jpg";
            if (!Directory.Exists(path))
            {
                var emptyAvatar =
                    new FileStream(AppContext.BaseDirectory + "/avatars/unknown.png",
                        FileMode.Open, FileAccess.Read);
                return new FileStreamResult(emptyAvatar, "image/png");
            }

            var avatar = new FileStream(path, FileMode.Open, FileAccess.Read);
            return new FileStreamResult(avatar, "image/jpeg");
        }

        /// <summary>
        /// Получить социальные сети аккаунта
        /// </summary>
        /// <returns></returns>
        [Route("GetAccountSocials")]
        [HttpGet]
        public async Task<SocialNetworksList> GetAccountSocials()
        {
            return await _db.SocialNetworksList.FirstOrDefaultAsync(
                s => s.Id == AccountChatId);
        }

        /// <summary>
        /// Сменить никнейм аккаунта
        /// </summary>
        /// <param name="newNickname">Новый никнейм</param>
        /// <returns></returns>
        [Route("ChangeAccountNickname")]
        [HttpPut]
        public async Task<IActionResult> ChangeAccountNickname(
            [FromBody] string newNickname)
        {
            var account = await _accountsManager.GetAccountAsync(AccountChatId);
            account.Nickname = newNickname;
            await _db.SaveChangesAsync();
            return Ok("Nickname successfully changed");
        }
    }
}
