using ShoppingAppAPI.Exceptions;
using ShoppingAppAPI.Mappers;
using ShoppingAppAPI.Models;
using ShoppingAppAPI.Models.DTO_s;
using ShoppingAppAPI.Models.DTO_s.Product_DTO_s;
using ShoppingAppAPI.Repositories.Interfaces;
using ShoppingAppAPI.Services.Interfaces;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace ShoppingAppAPI.Services.Classes
{
    public class ProductServices : IProductServices
    {
        private readonly IProductRepository _productRepository;
        /// <summary>
        /// Constructor for ProductServices class.
        /// </summary>
        /// <param name="productRepository">Product repository dependency.</param>
        public ProductServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        /// <summary>
        /// Adds a new product.
        /// </summary>
        /// <param name="productDTO">Product information DTO.</param>
        /// <returns>Returns the added product DTO.</returns>
        /// <exception cref="ItemAlreadyExistException">Thrown when product already exist</exception>
        /// <exception cref="UnableToAddItemException">Thrown when unable to add item to database</exception>
        public async Task<SellerGetProductDTO> AddProduct(AddProductDTO productDTO)
        {
            try
            {
                Product existingProduct = await _productRepository.GetProductByName(productDTO.Name);
                if (existingProduct != null)
                {
                    throw new ItemAlreadyExistException("Product");
                }
                Product product = new Product()
                {
                    Name = productDTO.Name,
                    SellerID = productDTO.SellerID,
                    Description = productDTO.Description,
                    Price = productDTO.Price,
                    CategoryID = productDTO.CategoryID,
                    Brand  = productDTO.Brand,
                    Image_URL = productDTO.Image_URL,
                    Stock_Quantity = productDTO.Stock_Quantity,
                    Creation_Date = DateTime.Now,
                    Last_Updated = DateTime.Now,
                };
                Product returnProduct = await _productRepository.Add(product);
                if(returnProduct == null) { 
                    throw new UnableToAddItemException("Unable to Add Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(returnProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToAddItemException(ex.Message);
            }
        }
        /// <summary>
        /// Retrieves products by name.
        /// </summary>
        /// <param name="productName">Name of the product to search.</param>
        /// <returns>Returns a list of products matching the specified name.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        public async Task<IEnumerable<CustomerGetProductDTO>> GetProductsByName(string productName)
        {
            try
            {
                IEnumerable<Product> products = await _productRepository.Get();
                if (products.Count() == 0)
                {
                    throw new NoAvailableItemException("Products");
                }
                return products.Where(p => p.Name.Contains(productName, System.StringComparison.OrdinalIgnoreCase)).Select(p => ProductMapper.MapToCustomerProductDTO(p));
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        /// <summary>
        /// Updates the price of a product.
        /// </summary>
        /// <param name="NewPrice">New price value.</param>
        /// <param name="ProductID">ID of the product to update.</param>
        /// <returns>Returns the updated product DTO.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update item in database</exception>
        public async Task<SellerGetProductDTO> UpdateProductPrice(decimal NewPrice, int ProductID)
        {
            try
            {
                Product product = await _productRepository.Get(ProductID);
                if (product == null)
                {
                    throw new NoAvailableItemException("Product");
                }
                product.Price = NewPrice;
                product.Last_Updated = DateTime.Now;
                Product updatedProduct = await _productRepository.Update(product);
                if (updatedProduct == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(updatedProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
        /// <summary>
        /// Updates the stock quantity of a product.
        /// </summary>
        /// <param name="NewStock">New stock quantity.</param>
        /// <param name="ProductID">ID of the product to update.</param>
        /// <returns>Returns the updated product DTO.</returns>
        /// <exception cref="NoAvailableItemException">Thrown when no items are available</exception>
        /// <exception cref="UnableToUpdateItemException">Thrown when unable to update item in database</exception>
        public async Task<SellerGetProductDTO> UpdateProductStock(int NewStock, int ProductID)
        {
            try
            {
                Product product = await _productRepository.Get(ProductID);
                if (product == null)
                {
                    throw new NoAvailableItemException("Product");
                }
                product.Stock_Quantity = NewStock;
                product.Last_Updated = DateTime.Now;
                Product updatedProduct = await _productRepository.Update(product);
                if (updatedProduct == null)
                {
                    throw new UnableToUpdateItemException("Unable to Update Product at this moment!");
                }
                SellerGetProductDTO sellerGetProductDTO = ProductMapper.MapToSellerProductDTO(updatedProduct);
                return sellerGetProductDTO;
            }
            catch (Exception ex)
            {
                throw new UnableToUpdateItemException(ex.Message);
            }
        }
    }
}
