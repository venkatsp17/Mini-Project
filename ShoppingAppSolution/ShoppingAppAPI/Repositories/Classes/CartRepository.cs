using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CartRepository : BaseRepository<int, Cart>
    {
        public CartRepository(ShoppingAppContext context) : base(context)
        {

        }

        public async override Task<Cart> Get(int key)
        {
            return await _context.Carts.FirstOrDefaultAsync(c => c.CartID == key) ?? throw new NotFoundException("Cart");
        }

    }
}
