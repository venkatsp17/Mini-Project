using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class ReviewRepository : BaseRepository<int, Review>
    {
        public ReviewRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<Review> Get(int key)
        {
            return await _context.Reviews.FirstOrDefaultAsync(c => c.ReviewID == key) ?? throw new NotFoundException("Review");
        }
    }
}
