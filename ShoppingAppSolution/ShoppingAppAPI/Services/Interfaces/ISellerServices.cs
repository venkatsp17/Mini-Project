using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ISellerServices
    {
        Task<Seller> UpdateSellerLastLogin(int SellerID);
    }
}
