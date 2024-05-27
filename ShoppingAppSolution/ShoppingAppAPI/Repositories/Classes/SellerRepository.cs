using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class SellerRepository : BaseRepository<int, Seller>, ISellerRepository
    {
        public SellerRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Seller> Get(int key)
        {
            return await _context.Sellers.FirstOrDefaultAsync(c => c.SellerID == key) ?? throw new NotFoundException("Seller");
        }

        public async Task<Seller> GetSellerByEmail(string email)
        {
            var seller = await _context.Sellers.FirstOrDefaultAsync(s => s.Email == email);
            return seller;
        }
    }
}
