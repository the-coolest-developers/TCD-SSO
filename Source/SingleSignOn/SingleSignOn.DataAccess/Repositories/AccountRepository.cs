using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SingleSignOn.DataAccess.Context;
using SingleSignOn.DataAccess.Entities;
using WebApiBaseLibrary.DataAccess.Repositories;

namespace SingleSignOn.DataAccess.Repositories
{
    public class AccountRepository : EntityFrameworkBaseRepository<Account>, IAccountRepository
    {
        private readonly AccountContext _accountContext;

        public AccountRepository(AccountContext accountContext) : base(accountContext)
        {
            _accountContext = accountContext;
        }

        public bool ExistsWithEmail(string email)
        {
            return EntityDbSet.Any(account => account.Email == email);
        }

        public Task<bool> ExistsWithEmailAsync(string email)
        {
            return EntityDbSet.AnyAsync(account => account.Email == email);
        }
    }
}