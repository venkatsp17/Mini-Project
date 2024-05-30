using Microsoft.EntityFrameworkCore;
using ShoppingAppAPI.Contexts;
using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Order_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Classes;
using ShoppingAppAPI.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ShoppingAppAPI.Models.Enums;

namespace ShoppingAppTest.Service_Tests
{
    [TestFixture]
    public class OrderServicesTests
    {
        private ShoppingAppContext _context;
        private IRepository<int, Order> _orderRepository;
        private IRepository<int, Refund> _refundRepository;
        private IRepository<int, CartItem> _cartItemRepository;
        private IOrderDetailRepository _orderDetailRepository;
        private ICartRepository _cartRepository;
        private ICustomerRepository _customerRepository;
        private IProductRepository _productRepository;
        private ICartServices _cartServices;
        private IUnitOfWork _unitOfWork;
        private OrderServices _orderServices;

        private ShoppingAppContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<ShoppingAppContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var context = new ShoppingAppContext(options);
            context.Database.EnsureCreated();
            return context;
        }

        [SetUp]
        public void Setup()
        {
            _context = GetInMemoryDbContext();
            _orderRepository = new OrderRepository(_context);
            _refundRepository = new RefundRepository(_context);
            _orderDetailRepository = new OrderDetailRepository(_context);
            _cartRepository = new CartRepository(_context);
            _customerRepository = new CustomerRepository(_context);
            _productRepository = new ProductRepository(_context);
            _cartItemRepository = new CartItemRepository(_context);
            _unitOfWork = new UnitOfWorkServices(_context);

            // Seed the database with test data
            var product = new Product
            {
                ProductID = 1,
                Name = "Test Product",
                Price = 100.0m,
                SellerID = 1,
                Description = "",
                CategoryID = 1,
                Brand ="",
                Image_URL = "",
                Stock_Quantity =10,
                Last_Updated=DateTime.Now

            };
            var cartItem = new CartItem
            {
                CartID = 1,
                ProductID = product.ProductID,
                Quantity = 2,
                Price = (double)product.Price
            };
            var cart = new Cart
            {
                CartID = 1,
                CustomerID = 1,
                Cart_Status = CartStatus.Open,
                CartItems = new List<CartItem> { cartItem }
            };
            var customer = new Customer
            {
                CustomerID = 1,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"
            };

            _productRepository.Add(product);
            _cartItemRepository.Add(cartItem);
            _cartRepository.Add(cart);
            _customerRepository.Add(customer);

           
            _cartServices = new CartServices(_cartRepository, _cartItemRepository, _productRepository);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
        }

        //[Test]
        //public async Task PlaceOrder_ValidOrder_ReturnsCustomerOrderReturnDTO()
        //{
        //    // Arrange
        //    var placeOrderDTO = new PlaceOrderDTO
        //    {
        //        CustomerID = 1,
        //        Address = "123 Main St",
        //        Shipping_Method = "Standard",
        //        Shipping_Cost = 5.0m
        //    };

        //    // Act
        //    var result = await _orderServices.PlaceOrder(placeOrderDTO);

        //    // Assert
        //    Assert.That(result, Is.Not.Null);
        //    Assert.That(result.OrderDetails, Is.Not.Empty);
        //}

        //[Test]
        //public void PlaceOrder_EmptyCart_ThrowsEmptyCartException()
        //{
        //    // Arrange
        //    var emptyCart = new Cart
        //    {
        //        CartID = 2,
        //        CustomerID = 1,
        //        Cart_Status = CartStatus.Empty
        //    };
        //    _context.Carts.Add(emptyCart);
        //    _context.SaveChanges();

        //    var placeOrderDTO = new PlaceOrderDTO
        //    {
        //        CustomerID = 1,
        //        Address = "123 Main St",
        //        Shipping_Method = "Standard",
        //        Shipping_Cost = 5.0m
        //    };

        //    // Act & Assert
        //    var ex = Assert.ThrowsAsync<EmptyCartException>(async () => await _orderServices.PlaceOrder(placeOrderDTO));
        //    Assert.That(ex.Message, Is.EqualTo("The cart is empty."));
        //}

        [Test]
        public async Task UpdateOrderStatus_ValidOrder_ReturnsUpdatedOrder()
        {
            // Arrange
            var order = new Order
            {
                CustomerID = 1,
                Order_Date = DateTime.Now,
                Status = OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 200.0m,
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m
            };
            await _orderRepository.Add(order);

            // Act
            var result = await _orderServices.UpdateOrderStatus(OrderStatus.Shipped, order.OrderID);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Status, Is.EqualTo(OrderStatus.Shipped));
        }

        [Test]
        public async Task ViewAllSellerActiveOrders_ValidSeller_ReturnsActiveOrders()
        {
            // Arrange
            var customer = new Customer
            {
                CustomerID = 2,
                Name = "John Doe",
                Email = "john.doe@example.com",
                Address = "123 Main St",
                Phone_Number = "123-456-7890",
                Profile_Picture_URL = "http://example.com/profile.jpg",
                Last_Login = DateTime.Now,
                Account_Status = "Active"

            };
            var order = new Order
            {
                CustomerID = 2,
                Order_Date = DateTime.Now,
                Status = OrderStatus.Pending,
                Address = "123 Main St",
                Total_Amount = 200.0m,
                Shipping_Method = "Standard",
                Shipping_Cost = 5.0m,
                Customer = customer
            };
            var orderDetail = new OrderDetail
            {
                ProductID = 1,
                Quantity = 2,
                OrderID = order.OrderID,
                SellerID = 1,
                Unit_Price = 100.0m,
                Order = order,
            };
            await _orderRepository.Add(order);
            await _orderDetailRepository.Add(orderDetail);
            _orderServices = new OrderServices(_orderRepository, _unitOfWork, _orderDetailRepository, _cartRepository, _cartServices, _refundRepository);
            // Act
            var result = await _orderServices.ViewAllSellerActiveOrders(1);

            // Assert
            Assert.That(result, Is.Not.Empty);
        }
    }
}
