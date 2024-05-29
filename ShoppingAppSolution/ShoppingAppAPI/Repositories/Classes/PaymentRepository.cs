using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class PaymentRepository : IRepository<int, Payment>
    {
        private readonly ShoppingAppContext _context;
        public PaymentRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Payment> Add(Payment item)
        {
            _context.Payments.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Payment> Delete(int key)
        {
            var item = await Get(key);
            _context.Payments.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Payment>> Get()
        {
            return await _context.Payments.ToListAsync() ?? throw new NoAvailableItemException("Payments");
        }

        public async Task<Payment> Update(Payment item)
        {
            _context.Payments.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }


        public async Task<Payment> Get(int key)
        {
            return await _context.Payments.FirstOrDefaultAsync(c => c.PaymentID == key) ?? throw new NotFoundException("Payment");
        }
    }
}
