using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ProductRepository : BaseRepository<int, Product>, IProductRepository
    {
        public ProductRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Product> Get(int key)
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).FirstOrDefaultAsync(c => c.ProductID == key) ?? throw new NotFoundException("Product");
        }

        public async override Task<IEnumerable<Product>> Get()
        {
            try
            {
                return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).ToListAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to Fetch Product Details!");
            }
        }

        public async Task<Product> GetProductByName(string productName)
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).FirstOrDefaultAsync(c => c.Name == productName);
        }
    }
}
