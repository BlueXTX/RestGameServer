using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Requests;

namespace PaidGame.Server.Services
{
    /// <summary>
    /// Менеджер для работы с аккаунтами пользователей
    /// </summary>
    public class AccountsManager
    {
        private readonly ApplicationContext _db;
        private readonly LeaguesManager _leaguesManager;

        public AccountsManager(ApplicationContext db, LeaguesManager leaguesManager)
        {
            _db = db;
            _leaguesManager = leaguesManager;
        }

        /// <summary>
        /// Добавить аккаунт в базу данных
        /// </summary>
        /// <param name="registerParams">Параметры добавляемого аккаунта</param>
        /// <returns></returns>
        public async Task<bool> AddAccountAsync(RegisterParams registerParams)
        {
            if (await GetAccountAsync(registerParams.ChatId) != null)
            {
                return false;
            }

            await _db.Accounts.AddAsync(new Account(registerParams.ChatId,
                registerParams.Password, registerParams.ChatId.ToString(),
                new Booster(5, 2)));
            await _db.SaveChangesAsync();
            var createdAccount = await GetAccountAsync(registerParams.ChatId);
            await _leaguesManager.CalculateLeague(createdAccount);
            return true;
        }

        /// <summary>
        /// Проверить наличие аккаунта в БД
        /// </summary>
        /// <param name="account">Модель аккаунта</param>
        /// <returns></returns>
        public async Task<bool> AccountExistsAsync(Account account)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(
                x => account.ChatId == x.ChatId);
            return acc != null;
        }

        /// <summary>
        /// Получить модель аккаунта 
        /// </summary>
        /// <param name="chatId">Id чата с пользователем</param>
        /// <returns></returns>
        public async Task<Account> GetAccountAsync(long chatId)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        /// <summary>
        /// Верифицировать логин и пароль
        /// </summary>
        /// <param name="loginParams">Параметры верификации</param>
        /// <returns>Равны ли данные в запросе данным пользователя</returns>
        public async Task<bool> VerifyLogPassAsync(LoginParams loginParams)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(
                x => x.ChatId == loginParams.ChatId &&
                     x.Password == loginParams.Password);
            return acc != null;
        }

        /// <summary>
        /// Изменить пароль аккаунта
        /// </summary>
        /// <param name="changePasswordParams">Параметры изменения пароля</param>
        /// <returns></returns>
        public async Task<bool> ChangePasswordAsync(
            ChangePasswordParams changePasswordParams)
        {
            var account = await GetAccountAsync(changePasswordParams.ChatId);
            if (account == null) return false;
            account.Password = changePasswordParams.NewPassword;
            await _db.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Обновить данные модели аккаунта
        /// </summary>
        /// <param name="account">Модель для обновления</param>
        /// <returns></returns>
        public async Task UpdateAccountAsync(Account account)
        {
            _db.Accounts.Update(account);
            await _db.SaveChangesAsync();
        }

        /// <summary>
        /// Получить всех пользователей
        /// </summary>
        /// <returns></returns>
        public async Task<List<Account>> GetAllUsers()
        {
            return await _db.Accounts.ToListAsync();
        }
    }
}
