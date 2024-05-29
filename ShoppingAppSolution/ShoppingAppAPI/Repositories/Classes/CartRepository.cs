using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CartRepository : ICartRepository
    {
        private readonly ShoppingAppContext _context;
        public CartRepository(ShoppingAppContext context)
        {
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
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Cart>> Get()
        {
            return await _context.Carts.ToListAsync() ?? throw new NoAvailableItemException("Carts");
        }

        public async Task<Cart> Update(Cart item)
        {
            _context.Carts.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Cart> Get(int key)
        {
            return await _context.Carts.Include(c => c.CartItems).FirstOrDefaultAsync(c => c.CartID == key);
        }

        public async Task<Cart> GetCartByCustomerID(int customerID)
        {
            return await _context.Carts.Include(c => c.CartItems).ThenInclude(ci=>ci.Product).FirstOrDefaultAsync(c => c.CustomerID == customerID);
        }
    }
}
