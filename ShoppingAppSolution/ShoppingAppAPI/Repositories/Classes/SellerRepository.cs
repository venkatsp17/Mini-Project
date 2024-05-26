using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class SellerRepository : BaseRepository<int, Seller>
    {
        public SellerRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Seller> Get(int key)
        {
            return await _context.Sellers.FirstOrDefaultAsync(c => c.SellerID == key) ?? throw new NotFoundException("Seller");
        }
    }
}
