using System.Threading.Tasks;
using SingleSignOn.DataAccess.Entities;
using WebApiBaseLibrary.DataAccess.Repositories;

namespace SingleSignOn.DataAccess.Repositories
{
    public interface IAccountRepository : IRepository<Account>, IDatabaseRepository
    {
        public Account GetWithEmail(string email);
        public Task<Account> GetWithEmailAsync(string email);
        
        public bool ExistsWithEmail(string email);
        public Task<bool> ExistsWithEmailAsync(string email);
    }
}