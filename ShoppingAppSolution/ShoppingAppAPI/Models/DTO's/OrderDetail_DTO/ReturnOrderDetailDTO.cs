using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO
{
    public class ReturnOrderDetailDTO
    {
        public int OrderDetailID { get; set; }
        public int OrderID { get; set; }
        public int ProductID { get; set; }
        public int Quantity { get; set; }
        public decimal Unit_Price { get; set; }


    }
}
