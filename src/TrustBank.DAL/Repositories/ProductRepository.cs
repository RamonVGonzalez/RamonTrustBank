using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using TrustBank.Core.Models;
using TrustBank.Core.Models.Enums;
using TrustBank.Core.RepositoriesInterface;
using TrustBank.Infrastructure.Data;
using TrustBank.Infrastructure.Repositories.Base;

namespace TrustBank.Infrastructure.Repositories
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context)
            : base (context)
        {
           
        }
        public async Task DeleteAsync(string id)
        {
            var product = await _context.Products
                 .SingleAsync(x => x.Id.ToLower() == id.ToLower());

            product.ClosureStatus = ClosureStatus.Y;
            await _context.SaveChangesAsync();
        }
    }
}
