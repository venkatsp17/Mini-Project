using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using ShoppingAppAPI.Models.DTO_s.Seller_DTO_s;

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

        public async Task<SellerDTO> UpdateSeller(SellerUpdateDTO updateDTO)
        {
            try
            {
                Seller seller = await _sellerRepository.Get(updateDTO.SellerID);
                seller.Phone_Number = updateDTO.Phone_Number;
                seller.Address = updateDTO.Address;
                seller.Email = updateDTO.Email;
                seller.Name = updateDTO.Name;
                seller.Profile_Picture_URL = updateDTO.Profile_Picture_URL;
                Seller updatedSeller = await _sellerRepository.Update(seller);
                if (updatedSeller == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Profile at this moment!");
                }
                return SellerMapper.MapToSellerDTO(seller);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
    }
}
