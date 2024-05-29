using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Order_DTO_s
{
    public class UpdateOrderDeliveryDetailsDTO
    {
        public int OrderID { get; set; }
        public string Address { get; set; }
        public string Shipping_Method { get; set; }
    }
}
