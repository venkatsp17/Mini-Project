using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Interfaces
{
    public interface IOrderServices
    {
        Task<IEnumerable<Order>> ViewAllSellerActiveOrders(int SellerID);

        Task<OrderReturnDTO> UpdateOrderStatus(OrderStatus orderStatus, int OrderID);
    }
}
