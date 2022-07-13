using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.RepositoriesInterface.Base;

namespace TrustBank.Core.RepositoriesInterface
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetAccountByAccountNumberAsync(string accountNumber);
        Task DeleteAsync(string accountNumber);

    }
}
