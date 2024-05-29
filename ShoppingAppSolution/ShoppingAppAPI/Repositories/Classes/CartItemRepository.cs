using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CartItemRepository : Interfaces.IRepository<int, CartItem>
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
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<CartItem>> Get()
        {
            return await _context.CartItems.ToListAsync() ?? throw new NoAvailableItemException("CartItems");
        }

        public async Task<CartItem> Update(CartItem item)
        {
            _context.CartItems.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<CartItem> Get(int key)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == key) ?? throw new NotFoundException("CartItem");
        }
    }
}
