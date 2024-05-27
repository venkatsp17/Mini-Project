using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;

namespace ShoppingAppAPI.Services.Classes
{
    public class SellerServices : ISellerServices
    {
        private readonly ISellerRepository _sellerRepository;
        public SellerServices(ISellerRepository sellerRepository) { 
            _sellerRepository = sellerRepository;
        }

        public async Task<Seller> UpdateSellerLastLogin(int SellerID)
        {
            Seller seller = await _sellerRepository.Get(SellerID);
            seller.Last_Login = DateTime.Now;
            return await _sellerRepository.Update(seller);
        }
    }
}
