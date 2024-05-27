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

        public async override Task<IEnumerable<Order>> Get()
        {
            try
            {
                return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Customer).ToListAsync() ?? throw new NoAvailableItemException("Orders");
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to Fetch Product Details!");
            }
           
        }
    }
}
