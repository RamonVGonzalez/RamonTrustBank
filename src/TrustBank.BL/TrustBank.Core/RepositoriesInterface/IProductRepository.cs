
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.RepositoriesInterface.Base;

namespace TrustBank.Core.RepositoriesInterface
{
    public interface IProductRepository : IRepository<Product>
    {
        Task DeleteAsync(string Id);
    }
}
