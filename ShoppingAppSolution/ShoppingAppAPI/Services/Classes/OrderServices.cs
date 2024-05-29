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
        private readonly IOrderDetailRepository _orderDetailRepository;
        private readonly ICartRepository _cartRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICartServices _cartServices;
        public OrderServices(IRepository<int, Order> orderRepository, 
            IUnitOfWork unitOfWork, 
            IOrderDetailRepository orderDetailRepository,
            ICartRepository cartRepository,
            ICartServices cartServices)
        { 
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
            _orderDetailRepository = orderDetailRepository;
            _cartRepository = cartRepository;
            _cartServices = cartServices;
        }

        public async Task<CustomerOrderReturnDTO> PlaceOrder(PlaceOrderDTO placeOrderDTO)
        {
            using (var transaction = _unitOfWork.BeginTransaction())
            {
                try
                {
                    var cart = await _cartRepository.GetCartByCustomerID(placeOrderDTO.CustomerID);
                    if(cart == null || cart.Cart_Status == CartStatus.Closed || cart.Cart_Status == CartStatus.Empty)
                    {
                        throw new NoAvailableItemException("Cart");
                    }
                    if(cart.CartItems == null || !cart.CartItems.Any())
                    {
                        throw new EmptyCartException();
                    }
                    var totalPrice = cart.CartItems.Sum(od => od.Price);
                    Order order = new Order()
                    {
                        CustomerID = placeOrderDTO.CustomerID,
                        Order_Date = DateTime.Now,
                        Status = OrderStatus.Pending,
                        Address = placeOrderDTO.Address,
                        Total_Amount = (decimal)totalPrice,
                        Shipping_Method = placeOrderDTO.Shipping_Method,
                        Shipping_Cost = placeOrderDTO.Shipping_Cost,
                    };

                    Order newOrder = await _orderRepository.Add(order);
                    if(newOrder == null)
                    {
                        throw new UnableToAddItemException("Unable to Create Order at this moment!");
                    }

                    ICollection<OrderDetail> orderDetails = cart.CartItems.Select(od => new OrderDetail()
                    {
                        ProductID = od.ProductID,
                        Quantity = od.Quantity,
                        OrderID = newOrder.OrderID,
                        SellerID = od.Product.SellerID,
                        Unit_Price = (decimal)od.Price,
                    }).ToList();

                    ICollection<OrderDetail> newOrderDetails = new List<OrderDetail>(); ;
                    foreach (var orderDetail in orderDetails)
                    {
                         OrderDetail newOrderDetail = await _orderDetailRepository.Add(orderDetail);
                         if(newOrderDetail == null)
                         {
                            throw new UnableToAddItemException("Unable to Create Order at this moment!");
                         }
                         newOrderDetails.Add(newOrderDetail);
                    }

                    newOrder.OrderDetails = newOrderDetails;

                    var deletedCart = await _cartServices.CloseCart(cart.CartID);
                    if(deletedCart == null)
                    {
                        throw new UnableToAddItemException("Unable to process order at this moment!");
                    }
                    await _unitOfWork.Commit();
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
                IEnumerable<OrderDetail> ordersDetails = await _orderDetailRepository.GetSellerOrderDetails(SellerID);
                if (!ordersDetails.Any())
                {
                    throw new NoAvailableItemException("Orders");
                }

                var activeOrders = ordersDetails
                    .Where(od => od.Order.Status == OrderStatus.Shipped ||
                                 od.Order.Status == OrderStatus.Pending ||
                                 od.Order.Status == OrderStatus.Processing)
                    .Select(od => od.Order)
                    .Distinct();
                return activeOrders.Select(o => OrderMapper.MapToSellerOrderReturnDTO(o));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message); 
            }
        }

        public async Task<IEnumerable<CustomerOrderReturnDTO>> ViewCustomerOrderHistory(int CustomerID)
        {
            try
            {
                IEnumerable<Order> orders = await _orderRepository.Get();
                if (orders.Count() == 0)
                {
                    throw new NoAvailableItemException("Products");
                }
                return orders.Where(o => o.CustomerID == CustomerID)
                    .Where(o => o.Status == OrderStatus.Delivered || o.Status == OrderStatus.Failed || o.Status == OrderStatus.Canceled || o.Status == OrderStatus.Refunded)
                    .Select(o => OrderMapper.MapToCustomerOrderReturnDTO(o));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<CustomerOrderReturnDTO> CustomerCancelOrder(int OrderID)
        {
            try
            {
                Order order = await _orderRepository.Get(OrderID);
                if (order == null)
                {
                    throw new NotFoundException("Order");
                }
                if(order.Status == OrderStatus.Shipped || order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Refunded || order.Status == OrderStatus.Failed)
                {
                    OrderStatus orderStatus = order.Status;
                    throw new NotAllowedToCancelOrderException(orderStatus.ToString());
                }
                order.Status = OrderStatus.Canceled;
                order.Last_Updated = DateTime.Now;
                Order updatedOrder = await _orderRepository.Update(order);
                if (updatedOrder == null)
                {
                    throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                }
                return OrderMapper.MapToCustomerOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<CustomerOrderReturnDTO> UpdateOrderDeliveryDetails(UpdateOrderDeliveryDetailsDTO updateOrderDeliveryDetailsDTO)
        {
            try
            {
                Order order = await _orderRepository.Get(updateOrderDeliveryDetailsDTO.OrderID);
                if (order == null)
                {
                    throw new NotFoundException("Order");
                }
                order.Address = updateOrderDeliveryDetailsDTO.Address;
                order.Shipping_Method = updateOrderDeliveryDetailsDTO.Shipping_Method;
                order.Last_Updated = DateTime.Now;
                Order updatedOrder = await _orderRepository.Update(order);
                if (updatedOrder == null)
                {
                    throw new UnableToUpdateItemException("Unable to update order status at this moment!");
                }
                return OrderMapper.MapToCustomerOrderReturnDTO(updatedOrder);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
    }
}
