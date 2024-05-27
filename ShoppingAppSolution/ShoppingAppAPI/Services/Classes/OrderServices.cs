using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Classes
{
    public class OrderServices : IOrderServices
    {
        private readonly IRepository<int, Order> _orderRepository;
        public OrderServices(IRepository<int, Order> orderRepository) { 
            _orderRepository = orderRepository;
        }

        public async Task<OrderReturnDTO> UpdateOrderStatus(OrderStatus orderStatus, int OrderID)
        {
            try
            {
                Order order = await _orderRepository.Get(OrderID);
                if (order == null)
                {
                    throw new NotFoundException("Order");
                }
                order.Status = orderStatus;
                order.Last_Updated = DateTime.Now;
                Order updatedOrder = await _orderRepository.Update(order);
                if(updatedOrder == null)
                {
                    throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                }
                return OrderMapper.MapToOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<IEnumerable<Order>> ViewAllSellerActiveOrders(int SellerID)
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.Get();
                if (orders.Count() == 0) 
                {
                    throw new NoAvailableItemException("Products");
                }
                return orders.Where(o => o.SellerID == SellerID).Where(o => o.Status == OrderStatus.Shipped || o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
