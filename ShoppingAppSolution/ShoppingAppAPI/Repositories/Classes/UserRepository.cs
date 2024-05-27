using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class UserRepository : BaseRepository<int, User>, IUserRepository
    {
        public UserRepository(ShoppingAppContext context) : base(context)
        {
        }

        public async override Task<User> Get(int key)
        {
            return await _context.Users.FirstOrDefaultAsync(c => c.UserID == key) ?? throw new NotFoundException("User");
        }

        public async Task<User> GetCustomerDetailByEmail(string email)
        {
            var user = await _context.Users
                  .Include(u => u.Customer)
                  .FirstOrDefaultAsync(u => u.Customer.Email == email);
            return user;
        }

        public async Task<User> GetSellerDetailByEmail(string email)
        {
            var user = await _context.Users
                  .Include(u => u.Seller)
                  .FirstOrDefaultAsync(u => u.Seller.Email == email);
            return user;
        }
    }
}
