using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO;

namespace ShoppingAppAPI.Mappers
{
    public class OrderDetailMapper
    {
        public static OrderDetailDTO MapToOrderDetailDTO(OrderDetail orderDetail)
        {
            return new OrderDetailDTO
            {
               OrderDetailID = orderDetail.OrderDetailID,
               OrderID = orderDetail.OrderID,
               ProductID = orderDetail.ProductID,
               Quantity = orderDetail.Quantity,
               Unit_Price = orderDetail.Unit_Price,
            };
        }
    }
}
