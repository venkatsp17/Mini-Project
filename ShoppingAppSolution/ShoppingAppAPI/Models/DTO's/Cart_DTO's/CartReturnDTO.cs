using static ShoppingAppAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Cart_DTO_s
{
    public class CartReturnDTO
    {
        public int CartID { get; set; }
        public ICollection<CartItemReturnDTO> CartItems { get; set; }
    }
}
