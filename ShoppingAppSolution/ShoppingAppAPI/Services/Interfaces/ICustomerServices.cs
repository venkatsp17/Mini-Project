using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ICustomerServices
    {
        Task<Customer> UpdateCustomerLastLogin(int CustomerID);

    }
}
