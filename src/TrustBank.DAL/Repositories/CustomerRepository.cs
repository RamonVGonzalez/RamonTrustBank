using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.RepositoriesInterface;
using TrustBank.Infrastructure.Data;
using TrustBank.Infrastructure.Repositories.Base;

namespace TrustBank.Infrastructure.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        public CustomerRepository(ApplicationDbContext context) : base(context)
        {

        }

        public async Task<List<Account>> GetAccountsByCustomerIdAsync(string id)
        {
            var customerInDB = await _context.Customers
                .Include(x => x.Accounts)
                .ThenInclude(x => x.Product)
                .FirstOrDefaultAsync(c => c.Id.ToLower() == id.ToLower());

            var accounts = customerInDB.Accounts;

            return accounts;

        }

        public async Task<Address> GetAdressByCustomerIdAsync(string id)
        {
            var customerInDb = await _context.Customers
                .Where(c => c.Id.ToLower() == id.ToLower())
                .Include(x => x.Address)
                .SingleOrDefaultAsync();

            var address = customerInDb.Address;

            return address;
        }

        public override async Task<Customer> GetByIdAsync(string id)
        {
            var customerInDb = await _context.Customers
                .Where(c => c.Id.ToLower() == id.ToLower())
                .Include(x => x.Address)
                .Include(x => x.Accounts)
                .ThenInclude(x => x.Product)
                .SingleOrDefaultAsync();

            return customerInDb;

        }

    }
}
