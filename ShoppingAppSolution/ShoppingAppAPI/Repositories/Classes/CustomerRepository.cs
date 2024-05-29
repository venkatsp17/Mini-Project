using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;

namespace ShoppingAppAPI.Repositories.Classes
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly ShoppingAppContext _context;
        public CustomerRepository(ShoppingAppContext context)
        {
            _context = context;
        }

        public async Task<Customer> Add(Customer item)
        {
            _context.Customers.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Customer> Delete(int key)
        {
            var item = await Get(key);
            _context.Customers.Remove(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<IEnumerable<Customer>> Get()
        {
            return await _context.Customers.ToListAsync() ?? throw new NoAvailableItemException("Customers");
        }

        public async Task<Customer> Update(Customer item)
        {
            _context.Customers.Attach(item);
            _context.Entry(item).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return item;
        }
        public async Task<Customer> Get(int key)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.CustomerID == key) ?? throw new NotFoundException("Customer");
        }

        public async Task<Customer> GetCustomerByEmail(string email)
        {
            var customer = await _context.Customers
                 .Include(c => c.Cart)
                 .Include(c => c.Orders)
                 .FirstOrDefaultAsync(c => c.Email == email) ?? throw new NotFoundException("Customer");
            return customer;
        }
    }
}
