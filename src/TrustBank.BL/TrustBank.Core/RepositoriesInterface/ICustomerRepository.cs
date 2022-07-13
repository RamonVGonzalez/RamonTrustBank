
using System.Collections.Generic;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.RepositoriesInterface.Base;

namespace TrustBank.Core.RepositoriesInterface
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Task<List<Account>> GetAccountsByCustomerIdAsync(string id);

        Task<Address> GetAdressByCustomerIdAsync(string id);
    }
}
