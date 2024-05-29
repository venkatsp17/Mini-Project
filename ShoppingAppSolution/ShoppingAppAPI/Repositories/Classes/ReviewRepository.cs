using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ReviewRepository : IRepository<int, Review>
    {
        private readonly ShoppingAppContext _context;
        public ReviewRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Review> Add(Review item)
        {
            _context.Reviews.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Review> Delete(int key)
        {
            var item = await Get(key);
            _context.Reviews.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Review>> Get()
        {
            return await _context.Reviews.ToListAsync() ?? throw new NoAvailableItemException("Reviews");
        }

        public async Task<Review> Update(Review item)
        {
            _context.Reviews.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Review> Get(int key)
        {
            return await _context.Reviews.FirstOrDefaultAsync(c => c.ReviewID == key) ?? throw new NotFoundException("Review");
        }
    }
}
