using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models;

namespace ShoppingAppAPI.Mappers
{
    public class CartMapper
    {
        public static CartReturnDTO MapCartToDTO(Cart cart)
        {
            return new CartReturnDTO
            {
                CartID = cart.CartID,
                CartItems = cart.CartItems.Select(ci => new CartItemReturnDTO
                {
                    CartItemID = ci.CartItemID,
                    ProductID = ci.ProductID,
                    Quantity = ci.Quantity,
                    Price = ci.Price,
                    Size = ci.Size,
                    CartID = ci.CartID,
                }).ToList()
            };
        }
    }
}
