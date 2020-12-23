using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PaidGame.Server.Models;
using PaidGame.Server.Models.Requests;

namespace PaidGame.Server
{
    public class AccountsManager
    {
        private ApplicationContext _db;

        public AccountsManager(ApplicationContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Добавить аккаунт в базу данных
        /// </summary>
        /// <param name="registerParams">Параметры добавляемого аккаунта</param>
        /// <returns></returns>
        public async Task<bool> AddAcountAsync(RegisterParams registerParams)
        {
            if (await GetAccountAsync(registerParams.ChatId) != null)
            {
                return false;
            }

            await _db.Accounts.AddAsync(new Account(registerParams.ChatId,
                registerParams.Password, registerParams.ChatId.ToString()));
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AccountExistsAsync(Account account)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(
                x => account.ChatId == x.ChatId);
            return acc != null;
        }

        public async Task<Account> GetAccountAsync(long chatId)
        {
            return await _db.Accounts.FirstOrDefaultAsync(x => x.ChatId == chatId);
        }

        public async Task<bool> VerifyLogPassAsync(LoginParams loginParams)
        {
            var acc = await _db.Accounts.FirstOrDefaultAsync(
                x => x.ChatId == loginParams.ChatId &&
                     x.Password == loginParams.Password);
            return acc != null;
        }

        public async Task<bool> ChangePasswordAsync(
            ChangePasswordParams changePasswordParams)
        {
            var account = await GetAccountAsync(changePasswordParams.ChatId);
            if (account == null) return false;
            account.Password = changePasswordParams.NewPassword;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
