using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s
{
    public class AddProductDTO
    {
        public int SellerID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int CategoryID { get; set; }
        public string Brand { get; set; }
        public string Image_URL { get; set; }
        public int Stock_Quantity { get; set; }
    }
}
