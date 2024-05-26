using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class OrderDetailRepository : BaseRepository<int, OrderDetail>
    {
        public OrderDetailRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<OrderDetail> Get(int key)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(c => c.OrderDetailID == key) ?? throw new NotFoundException("OrderDetail");
        }
    }
}
