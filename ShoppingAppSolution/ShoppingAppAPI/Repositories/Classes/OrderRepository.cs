using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class OrderRepository : IRepository<int, Order>
    {
        private readonly ShoppingAppContext _context;
        public OrderRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Order> Add(Order item)
        {
            _context.Orders.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Order> Delete(int key)
        {
            var item = await Get(key);
            _context.Orders.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Order> Update(Order item)
        {
            _context.Orders.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Order> Get(int key)
        {
            return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Customer).FirstOrDefaultAsync(c => c.OrderID == key) ?? throw new NotFoundException("Order");
        }

        public async Task<IEnumerable<Order>> Get()
        { 
         return await _context.Orders.Include(o => o.OrderDetails).Include(o => o.Customer).ToListAsync() ?? throw new NoAvailableItemException("Orders");
        }
    }
}
