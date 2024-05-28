using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppAPI.Services.Classes
{
    public class OrderServices : IOrderServices
    {
        private readonly IRepository<int, Order> _orderRepository;
        private readonly IRepository<int, OrderDetail> _orderDetailRepository;
        private readonly IUnitOfWork _unitOfWork;
        public OrderServices(IRepository<int, Order> orderRepository, IUnitOfWork unitOfWork, IRepository<int, OrderDetail> orderDetailRepository) { 
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderDetailRepository = orderDetailRepository;
        }

        public async Task<CustomerOrderReturnDTO> PlaceOrder(PlaceOrderDTO placeOrderDTO)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    Order order = new Order()
                    {
                        CustomerID = placeOrderDTO.CustomerID,
                        SellerID = placeOrderDTO.SellerID,
                        Order_Date = placeOrderDTO.Order_Date,
                        Status = OrderStatus.Pending,
                        Address = placeOrderDTO.Address,
                        Total_Amount = placeOrderDTO.Total_Amount,
                        Shipping_Method = placeOrderDTO.Shipping_Method,
                        Shipping_Cost = placeOrderDTO.Shipping_Cost,
                    };

                    Order newOrder = await _orderRepository.Add(order);
                    if(newOrder == null)
                    {
                        throw new UnableToAddItemException("Unable to Create Order at this moment!");
                    }


                    ICollection<OrderDetail> orderDetails = placeOrderDTO.OrderDetails.Select(od => new OrderDetail()
                    {
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        Unit_Price = od.Unit_Price
                    }).ToList();


                    ICollection<OrderDetail> newOrderDetails = new List<OrderDetail>(); ;
                    foreach (var orderDetail in orderDetails)
                    {
                         orderDetail.OrderID = newOrder.OrderID;
                         OrderDetail newOrderDetail = await _orderDetailRepository.Add(orderDetail);
                         if(newOrderDetail == null)
                         {
                            throw new UnableToAddItemException("Unable to Create Order at this moment!");
                         }
                         newOrderDetails.Add(newOrderDetail);
                    }
                    await _unitOfWork.Commit();


                    newOrder.OrderDetails = newOrderDetails;
                    return OrderMapper.MapToCustomerOrderReturnDTO(newOrder); 
                }
                catch (Exception ex)
                {
                    await _unitOfWork.Rollback();
                    throw new UnableToAddItemException(ex.Message);
                }
            }

        }

        public async Task<SellerOrderReturnDTO> UpdateOrderStatus(OrderStatus orderStatus, int OrderID)
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
                return OrderMapper.MapToSellerOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<IEnumerable<SellerOrderReturnDTO>> ViewAllSellerActiveOrders(int SellerID)
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.Get();
                if (orders.Count() == 0) 
                {
                    throw new NoAvailableItemException("Products");
                }
                return orders.Where(o => o.SellerID == SellerID)
                    .Where(o => o.Status == OrderStatus.Shipped || o.Status == OrderStatus.Pending || o.Status == OrderStatus.Processing)
                    .Select(o => OrderMapper.MapToSellerOrderReturnDTO(o));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }
    }
}
