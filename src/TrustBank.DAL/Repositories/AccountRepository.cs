using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;
using TrustBank.Core.RepositoriesInterface;
using TrustBank.Infrastructure.Data;
using TrustBank.Infrastructure.Repositories.Base;

namespace TrustBank.Infrastructure.Repositories
{
    public class AccountRepository : Repository<Account>, IAccountRepository
    {

        public AccountRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task DeleteAsync(string accountNumber)
        {
            var account = await _context.Accounts
                 .SingleAsync(x => x.AccountNumber == accountNumber);
            account.ClosureStatus = ClosureStatus.Y;
            await _context.SaveChangesAsync();
        }

        public async Task<Account> GetAccountByAccountNumberAsync(string accountNumber)
        {
            var account = await _context.Accounts.Include(x => x.Product)
                 .FirstOrDefaultAsync(x => x.AccountNumber == accountNumber);

            return account;
        }

    }
}
