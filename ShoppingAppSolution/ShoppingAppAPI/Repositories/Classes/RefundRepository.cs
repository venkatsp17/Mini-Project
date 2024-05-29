using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class RefundRepository : IRepository<int, Refund>
    {
        private readonly ShoppingAppContext _context;
        public RefundRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Refund> Add(Refund item)
        {
            _context.Refunds.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Refund> Delete(int key)
        {
            var item = await Get(key);
            _context.Refunds.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Refund>> Get()
        {
            return await _context.Refunds.ToListAsync() ?? throw new NoAvailableItemException("Refunds");
        }

        public async Task<Refund> Update(Refund item)
        {
            _context.Refunds.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Refund> Get(int key)
        {
            return await _context.Refunds.FirstOrDefaultAsync(c => c.RefundID == key) ?? throw new NotFoundException("Refund");
        }
    }
}
