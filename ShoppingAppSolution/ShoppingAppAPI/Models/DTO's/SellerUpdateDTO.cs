namespace ShoppingAppAPI.Models.DTO_s
{
    public class SellerUpdateDTO
    {
        public int SellerID { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string Phone_Number { get; set; }
        public string? Profile_Picture_URL { get; set; }
    }
}
