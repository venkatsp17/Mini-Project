using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ProductRepository :  IProductRepository
    {
        private readonly ShoppingAppContext _context;
        public ProductRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Product> Add(Product item)
        {
            _context.Products.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Product> Delete(int key)
        {
            var item = await Get(key);
            _context.Products.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Product> Update(Product item)
        {
            _context.Products.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Product> Get(int key)
        {
            return await _context.Products.Include(p => p.Seller).Include(p => p.Reviews).FirstOrDefaultAsync(c => c.ProductID == key) ?? throw new NotFoundException("Product");
        }

        public async Task<IEnumerable<Product>> Get()
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
