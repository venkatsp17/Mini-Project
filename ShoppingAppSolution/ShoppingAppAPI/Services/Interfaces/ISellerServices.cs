using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;
using ShoppingAppAPI.Models.DTO_s;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface ISellerServices
    {
        Task<Seller> UpdateSellerLastLogin(int SellerID);
        Task<SellerDTO> UpdateSeller(SellerUpdateDTO updateDTO);
    }
}
