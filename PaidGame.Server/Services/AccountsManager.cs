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
        private readonly BoosterManager _boosterManager;

        public AccountsManager(ApplicationContext db,
            LeaguesManager leaguesManager,
            BoosterManager boosterManager)
        {
            _db = db;
            _leaguesManager = leaguesManager;
            _boosterManager = boosterManager;
        }

        /// <summary>
        /// Добавить аккаунт в базу данных
        /// </summary>
        /// <param name="registerParams">Параметры добавляемого аккаунта</param>
        /// <returns></returns>
        public async Task<bool> AddAccountAsync(RegisterParams registerParams)
        {
            if (await GetAccountAsync(registerParams.Login) != null)
            {
                return false;
            }

            await _db.Accounts.AddAsync(new Account(registerParams.Login,
                registerParams.Password, registerParams.Login, 3));
            await _db.SaveChangesAsync();
            var acc = await GetAccountAsync(registerParams.Login);
            await _boosterManager.AddBoosterToAccount(acc, 5, 2);
            await _leaguesManager.CalculateLeague(acc);
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
                x => account.Login == x.Login);
            return acc != null;
        }

        /// <summary>
        /// Получить модель аккаунта 
        /// </summary>
        /// <param name="chatId">Id чата с пользователем</param>
        /// <returns></returns>
        public async Task<Account> GetAccountAsync(string login)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.Login == login);
        }

        /// <summary>
        /// Верифицировать логин и пароль
        /// </summary>
        /// <param name="loginParams">Параметры верификации</param>
        /// <returns>Равны ли данные в запросе данным пользователя</returns>
        public async Task<bool> VerifyLogPassAsync(LoginParams loginParams)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(
                x => x.Login == loginParams.Login &&
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
            var account = await GetAccountAsync(changePasswordParams.Login);
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
