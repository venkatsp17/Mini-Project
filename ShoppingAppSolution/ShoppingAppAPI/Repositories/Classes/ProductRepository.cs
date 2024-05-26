using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ProductRepository : BaseRepository<int, Product>
    {
        public ProductRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Product> Get(int key)
        {
            return await _context.Products.FirstOrDefaultAsync(c => c.ProductID == key) ?? throw new NotFoundException("Product");
        }
    }
}
