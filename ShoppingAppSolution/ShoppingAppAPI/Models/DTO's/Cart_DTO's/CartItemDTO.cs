using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Cart_DTO_s
{
    public class CartItemReturnDTO
    {
        public int CartItemID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
        public int CartID { get; set; }
    }

    public class CartItemGetDTO
    {
        public int ProductID { get; set; }
        public int Quantity { get; set; }
    }
}
