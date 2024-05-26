using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class OrderRepository : BaseRepository<int, Order>
    {
        public OrderRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Order> Get(int key)
        {
            return await _context.Orders.FirstOrDefaultAsync(c => c.OrderID == key) ?? throw new NotFoundException("Order");
        }
    }
}
