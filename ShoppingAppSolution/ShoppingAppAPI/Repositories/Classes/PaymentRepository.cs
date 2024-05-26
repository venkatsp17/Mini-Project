using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class PaymentRepository : BaseRepository<int, Payment>
    {
        public PaymentRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Payment> Get(int key)
        {
            return await _context.Payments.FirstOrDefaultAsync(c => c.PaymentID == key) ?? throw new NotFoundException("Payment");
        }
    }
}
