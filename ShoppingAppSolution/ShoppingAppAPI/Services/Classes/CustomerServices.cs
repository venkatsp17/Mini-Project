using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class CustomerServices : ICustomerServices
    {
        private readonly ICustomerRepository _customerRepository;
        public CustomerServices(ICustomerRepository customerRepository) { 
            _customerRepository = customerRepository;
        }

        public async Task<Customer> UpdateCustomerLastLogin(int CustomerID)
        {
           Customer customer = await _customerRepository.Get(CustomerID);
           customer.Last_Login = DateTime.Now;
           return await _customerRepository.Update(customer);
        }
    }
}
