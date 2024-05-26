using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CartItemRepository : BaseRepository<int, CartItem>
    {
        public CartItemRepository(ShoppingAppContext context) : base(context)
        {

        }
        public async override Task<CartItem> Get(int key)
        {
            return await _context.CartItems.FirstOrDefaultAsync(c => c.CartItemID == key) ?? throw new NotFoundException("CartItem");
        }
    }
}
