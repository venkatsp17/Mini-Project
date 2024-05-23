using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exeptions.CartItemExceptions;
using ShoppingAppAPI.Interfaces.Repositories;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories
{
    public class CartItemRepository : IRepository<int, CartItem>
    {
        private readonly ShoppingAppContext _context;
        public CartItemRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<CartItem> Add(CartItem item)
        {
            _context.CartItems.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem> Delete(int key)
        {
            var item = await Get(key);
            _context.CartItems.Remove(item);
            await _context.SaveChangesAsync(true);
            return item;

        }

        public async Task<CartItem> Get(int key)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == key) ?? throw new CartItemNotFoundException();
        }

        public async Task<IEnumerable<CartItem>> Get()
        {
            var result = await _context.CartItems.ToListAsync();
            if (result.Count == 0)
            {
                throw new NoAvailableCartItem();
            }
            return result;
        }

        public async Task<CartItem> Update(CartItem item)
        {
            var result = await Get(item.CartItemID);
            if (result == null)
            {
                throw new CartItemNotFoundException();
            }
            _context.Update(item);
            await _context.SaveChangesAsync(true);
            return item;
        }
    }
}
