using Microsoft.EntityFrameworkCore;
using SingleSignOn.DataAccess.Entities;

namespace SingleSignOn.DataAccess.Context
{
    public class AccountContext : DbContext
    {
        public DbSet<Account> Accounts { get; set; }

        public AccountContext(DbContextOptions<AccountContext> options) : base(options)
        {
        }
    }
}