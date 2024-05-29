using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class SellerRepository : ISellerRepository
    {
        private readonly ShoppingAppContext _context;
        public SellerRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Seller> Add(Seller item)
        {
            _context.Sellers.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Seller> Delete(int key)
        {
            var item = await Get(key);
            _context.Sellers.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Seller>> Get()
        {
            return await _context.Sellers.ToListAsync() ?? throw new NoAvailableItemException("Sellers");
        }

        public async Task<Seller> Update(Seller item)
        {
            _context.Sellers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Seller> Get(int key)
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
