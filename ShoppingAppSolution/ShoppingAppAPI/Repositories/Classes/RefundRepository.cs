using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class RefundRepository : BaseRepository<int, Refund>
    {
        public RefundRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Refund> Get(int key)
        {
            return await _context.Refunds.FirstOrDefaultAsync(c => c.RefundID == key) ?? throw new NotFoundException("Refund");
        }
    }
}
