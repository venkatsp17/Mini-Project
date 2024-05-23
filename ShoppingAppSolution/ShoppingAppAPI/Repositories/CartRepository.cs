using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exeptions.CartExceptions;
using ShoppingAppAPI.Interfaces.Repositories;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories
{
    public class CartRepository : IRepository<int, Cart>
    {
        private readonly ShoppingAppContext _context;
        public CartRepository(ShoppingAppContext context) {
            _context = context;
        }

        public async Task<Cart> Add(Cart item)
        {
            _context.Carts.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Cart> Delete(int key)
        {
            var item = await Get(key);
            _context.Carts.Remove(item);
            await _context.SaveChangesAsync(true);
            return item;

        }

        public async Task<Cart> Get(int key)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartID == key) ?? throw new CartNotFoundException();
        }

        public async Task<IEnumerable<Cart>> Get()
        {
            var result = await _context.Carts.ToListAsync();
            if(result.Count == 0)
            {
                throw new NoAvailableCart();
            }
            return result;
        }

        public async Task<Cart> Update(Cart item)
        {
            var result = await Get(item.CartID);
            if(result == null)
            {
                throw new CartNotFoundException();
            }
            _context.Update(item);
            await _context.SaveChangesAsync(true);
            return item;
        }
    }
}
