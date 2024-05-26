using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CustomerRepository : BaseRepository<int, Customer>, ICustomerRepository
    {
        public CustomerRepository(ShoppingAppContext context) : base(context)
        {

        }
        public async override Task<Customer> Get(int key)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == key) ?? throw new NotFoundException("Customer");
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers
                 .Include(c => c.Cart)
                 .Include(c => c.Orders)
                 .FirstOrDefaultAsync(c => c.Email == email);
            return customer;
        }
    }
}
