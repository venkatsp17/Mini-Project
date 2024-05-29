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

        public ProductServices(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

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
