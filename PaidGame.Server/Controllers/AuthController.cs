using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Requests;

namespace PaidGame.Server.Controllers
{
    /// <summary>
    /// Контроллер для регистрации и авторизации
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IOptions<AuthOptions> _authOptions;
        private readonly AccountsManager _accountsManager;
        private readonly TokensManager _tokensManager;

        private long UserId => Convert.ToInt64(User.Claims
            .FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)
            ?.Value);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="authOptions">Опции авторизации</param>
        /// <param name="accountsManager">Менеджер аккаунтов</param>
        /// <param name="tokensManager">Менеджер токенов</param>
        public AuthController(IOptions<AuthOptions> authOptions,
            AccountsManager accountsManager,
            TokensManager tokensManager)
        {
            _authOptions = authOptions;
            _accountsManager = accountsManager;
            _tokensManager = tokensManager;
        }

        /// <summary>
        /// Зарегистрировать новый аккаунт
        /// </summary>
        /// <param name="registerParams">Параметры запроса регистрации</param>
        /// <returns></returns>
        [Route("Register")]
        [HttpPost]
        public async Task<IActionResult> Register(
            [FromBody] RegisterParams registerParams)
        {
            if (await _accountsManager.AddAcountAsync(registerParams))
            {
                return Ok("User registered successfully");
            }

            return Conflict();
        }

        /// <summary>
        /// Получить токен доступа для аккаунта
        /// </summary>
        /// <param name="loginParams">Параметры входа в аккаунт</param>
        /// <returns></returns>
        [Route("Login")]
        [HttpPost]
        public async Task<TokenPair> Login([FromBody] LoginParams loginParams)
        {
            if (await _accountsManager.VerifyLogPassAsync(loginParams))
            {
                return await GenerateJwtAsync(
                    await _accountsManager.GetAccountAsync(loginParams.ChatId));
            }

            return new TokenPair(string.Empty, string.Empty);
        }


        /// <summary>
        /// Сменить пароль аккаунта
        /// </summary>
        /// <param name="changePasswordParams">Параметры смены пароля аккаунта</param>
        /// <returns></returns>
        [Route("ChangePassword")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> ChangePassword(
            [FromBody] ChangePasswordParams changePasswordParams)
        {
            if (!await _accountsManager.VerifyLogPassAsync(
                new LoginParams(changePasswordParams.ChatId,
                    changePasswordParams.OldPassword)))
            {
                return Unauthorized("Invalid credentials");
            }

            if (await _accountsManager.ChangePasswordAsync(changePasswordParams))
            {
                return Ok("Password has been changed");
            }

            return Unauthorized("Invalid credentials");
        }


        /// <summary>
        /// Получить новый токен используя refresh token
        /// </summary>
        /// <returns>Новая пара токенов</returns>
        [Route("RefreshToken")]
        [HttpGet]
        public async Task<TokenPair> RefreshToken(string refreshToken)
        {
            if (!await _tokensManager.TokenPairExistsAsync(refreshToken))
                return new TokenPair(string.Empty, string.Empty);
            await _tokensManager.DeleteTokenAsync(refreshToken);
            return await GenerateJwtAsync(
                await _accountsManager.GetAccountAsync(UserId));
        }

        /// <summary>
        /// Отозвать пару токенов (refresh token и access token) 
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        [Route("RevokeToken")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> RevokeToken(string refreshToken)
        {
            if (!await _tokensManager.TokenPairExistsAsync(refreshToken))
            {
                return BadRequest("Token not found");
            }

            await _tokensManager.DeleteTokenAsync(refreshToken);
            return Ok("Token has been revoked");
        }

        private async Task<TokenPair> GenerateJwtAsync(Account account)
        {
            return await Task.Run(() => GenerateJwt(account));
        }

        private async Task<TokenPair> GenerateJwt(Account account)
        {
            var authParams = _authOptions.Value;
            var accessSecurityKey = authParams.GetAccessSecurityKey();
            var refreshSecurityKey = authParams.GetRefreshSecurityKey();
            var accessCredentials =
                new SigningCredentials(accessSecurityKey, SecurityAlgorithms.HmacSha256);

            var refreshCredentials =
                new SigningCredentials(refreshSecurityKey, SecurityAlgorithms.HmacSha256);

            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, account.ChatId.ToString()),
            };

            var accessToken = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(authParams.AccessTokenLifetime),
                accessCredentials);

            var refreshToken = new JwtSecurityToken(
                authParams.Issuer,
                authParams.Audience,
                claims,
                DateTime.Now,
                DateTime.Now.AddSeconds(authParams.RefreshTokenLifetime),
                refreshCredentials);

            var result = new TokenPair(
                new JwtSecurityTokenHandler().WriteToken(accessToken),
                new JwtSecurityTokenHandler().WriteToken(refreshToken));

            await _tokensManager.AddTokenPairAsync(result);
            return result;
        }
    }
}
