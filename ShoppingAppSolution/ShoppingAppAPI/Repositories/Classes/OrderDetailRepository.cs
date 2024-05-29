using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class OrderDetailRepository : IOrderDetailRepository
    {
        private readonly ShoppingAppContext _context;
        public OrderDetailRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<OrderDetail> Add(OrderDetail item)
        {
            _context.OrderDetails.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<OrderDetail> Delete(int key)
        {
            var item = await Get(key);
            _context.OrderDetails.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<OrderDetail>> Get()
        {
            return await _context.OrderDetails.ToListAsync() ?? throw new NoAvailableItemException("Order Details");
        }

        public async Task<OrderDetail> Update(OrderDetail item)
        {
            _context.OrderDetails.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<OrderDetail> Get(int key)
        {
            return await _context.OrderDetails.FirstOrDefaultAsync(c => c.OrderDetailID == key) ?? throw new NotFoundException("OrderDetail");
        }

        public async Task<IEnumerable<OrderDetail>> GetSellerOrderDetails(int SellerID)
        {
            return await _context.OrderDetails
            .Include(od => od.Order).ThenInclude(o => o.Customer)
            .Where(od => od.SellerID == SellerID)
            .ToListAsync();
        }
    }
}
