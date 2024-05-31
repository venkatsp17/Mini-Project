using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Models.DTO_s.Cart_DTO_s;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Repositories.Classes;
using static ShoppingAppAPI.Models.Enums;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Services.Interfaces;
using ShoppingAppAPI.Repositories.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace ShoppingAppAPI.Services.Classes
{
    public class CartServices : ICartServices
    {
        private readonly ICartRepository _cartRepository;
        private readonly IRepository<int, CartItem> _cartItemRepository;
        private readonly IProductRepository _productRepository;

        public CartServices(ICartRepository cartRepository, IRepository<int, CartItem> cartItemRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _cartItemRepository = cartItemRepository;
            _productRepository = productRepository;
        } 

        public async Task<CartReturnDTO> AddItemToCart(CartItemGetDTO cartItem, int cartID, int CustomerID)
        {
            try
            {
                var cart = await _cartRepository.Get(cartID);

                if (cart == null)
                {
                    // Create a new cart if none exists
                    cart = new Cart
                    {
                        CustomerID = CustomerID,
                        Cart_Status = CartStatus.Open,
                        Last_Updated = DateTime.Now,
                    };
                    cart = await _cartRepository.Add(cart);
                    if(cart == null)
                    {
                        throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                    }
                }
                var product = await _productRepository.Get(cartItem.ProductID);
                if(cart.CartItems ==  null || cart.CartItems.Count() == 0)
                {
                    var newItem = new CartItem
                    {
                        CartID = cart.CartID,
                        ProductID = cartItem.ProductID,
                        Quantity = cartItem.Quantity,
                        Price = cartItem.Price
                    };
                    var newCartItem = await _cartItemRepository.Add(newItem);
                    if (newCartItem == null)
                    {
                        throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                    }
                }
                else
                {
                    var existingItem = cart.CartItems.FirstOrDefault(ci => ci.ProductID == cartItem.ProductID);
                    if (existingItem != null)
                    {
                        existingItem.Quantity += cartItem.Quantity;
                        existingItem.Price = (double)(existingItem.Quantity * product.Price); // Update price based on new quantity
                        var updatedCartItem = await _cartItemRepository.Update(existingItem);
                        if (updatedCartItem == null)
                        {
                            throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                        }
                    }
                    else
                    {
                        var newItem = new CartItem
                        {
                            CartID = cart.CartID,
                            ProductID = cartItem.ProductID,
                            Quantity = cartItem.Quantity,
                            Price = cartItem.Price
                        };
                        var newCartItem = await _cartItemRepository.Add(newItem);
                        if (newCartItem == null)
                        {
                            throw new UnableToAddItemException("Unable to Add Item to Cart at this moment!");
                        }
                    }
                }
                cart.Last_Updated = DateTime.Now;
                await _cartRepository.Update(cart);
    

                return CartMapper.MapCartToDTO(cart);
            }
            catch(Exception ex)
            {
                throw new UnableToAddItemException(ex.Message);
            }
        }

        public async Task<CartReturnDTO> RemoveItemFromCart(int cartItemID)
        {
            try
            {
                var cartItem = await _cartItemRepository.Get(cartItemID);
                if (cartItem == null) throw new NotFoundException("CartItem not found.");

                var cart = await _cartRepository.Get(cartItem.CartID);
                if (cart == null) throw new NotFoundException("Cart not found.");

                var product = await _productRepository.Get(cartItem.ProductID);

                if (cartItem.Quantity > 1)
                {
                    cartItem.Quantity--;
                    cartItem.Price = (double)(product.Price * cartItem.Quantity); // Adjust price
                }
                else
                {
                    var deletedItem = await _cartItemRepository.Delete(cartItemID);
                    if (deletedItem == null)
                    {
                        throw new UnableToUpdateItemException("Unable to remove Item from Cart at this moment!");
                    }
                }
                cart.Last_Updated = DateTime.Now;
                try
                {
                    await _cartRepository.Update(cart);
                }
                catch (Exception ex)
                {

                }
                return CartMapper.MapCartToDTO(cart);
            }
           catch(Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<CartReturnDTO> UpdateCartItemQuantity(int CartItemID, int Quantity)
        {
            try
            {
                var existingcartItem = await _cartItemRepository.Get(CartItemID);
                if (existingcartItem == null) throw new NotFoundException("CartItem not found.");

                var cart = await _cartRepository.Get(existingcartItem.CartID);
                if (cart == null) throw new NotFoundException("Cart not found.");

                var product = await _productRepository.Get(existingcartItem.ProductID);

                existingcartItem.Quantity = Quantity;
                existingcartItem.Price = (double)(product.Price * existingcartItem.Quantity); // Update price based on quantity
                var updatedCartItem = await _cartItemRepository.Update(existingcartItem);
                if (updatedCartItem == null)
                {
                    throw new UnableToUpdateItemException("Unable to update Item from Cart at this moment!");
                }
                cart.Last_Updated = DateTime.Now;

                try
                {
                    await _cartRepository.Update(cart);
                }
                catch (Exception ex)
                {

                }

                return CartMapper.MapCartToDTO(cart);
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }

        public async Task<CartReturnDTO> GetCart(int cartID)
        {
            try
            {
                var cart = await _cartRepository.Get(cartID);
                if (cart == null) throw new NotFoundException("Cart not found.");

                return CartMapper.MapCartToDTO(cart);
            }
            catch(Exception ex)
            {
                throw new Exception("Unable to Get Cart at this moment!");
            }
        }

        public async Task<CartReturnDTO> CloseCart(int cartID)
        {
            try
            {
                var cart = await _cartRepository.Get(cartID);
                if (cart == null) throw new NotFoundException("Cart");

                var deletedCart = await _cartRepository.Delete(cartID);
                if (deletedCart == null)
                {
                    throw new UnableToUpdateItemException("Unable to close Cart at this moment!");
                }

                return CartMapper.MapCartToDTO(cart);
            }
            catch(Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        }
    }
