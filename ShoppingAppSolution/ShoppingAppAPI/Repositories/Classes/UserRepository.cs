using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class UserRepository : IUserRepository
    {
        private readonly ShoppingAppContext _context;
        public UserRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<User> Add(User item)
        {
            _context.Users.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User> Delete(int key)
        {
            var item = await Get(key);
            _context.Users.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<User>> Get()
        {
            return await _context.Users.ToListAsync() ?? throw new NoAvailableItemException("Categories");
        }

        public async Task<User> Update(User item)
        {
            _context.Users.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<User> Get(int key)
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
