using ShoppingAppAPI.Models.DTO_s.Customer_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.OrderDetail_DTO;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models.DTO_s;

namespace ShoppingAppAPI.Mappers
{
    public class OrderDetailMapper
    {
        public static ReturnOrderDetailDTO MapToReturnOrderDetailDTO(OrderDetail orderDetail)
        {
            return new ReturnOrderDetailDTO
            {
               OrderDetailID = orderDetail.OrderDetailID,
               OrderID = orderDetail.OrderID,
               ProductID = orderDetail.ProductID,
               Quantity = orderDetail.Quantity,
               Price = orderDetail.Price,
               Size = orderDetail.Size,
               Product = ProductMapper.MapToCustomerProductDTO1(orderDetail.Product),
            };
        }
    }
}
