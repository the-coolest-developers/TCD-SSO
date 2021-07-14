using System.Threading.Tasks;
using SingleSignOn.DataAccess.Entities;
using WebApiBaseLibrary.DataAccess.Repositories;

namespace SingleSignOn.DataAccess.Repositories
{
    public interface IAccountRepository : IRepository<Account>
    {
        public bool ExistsWithEmail(string email);
        public Task<bool> ExistsWithEmailAsync(string email);
    }
}