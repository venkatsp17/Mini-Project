namespace ShoppingAppAPI.Models.DTO_s.Review_DTO_s
{
    public class ReviewGetDTO
    {
        public int ProductID { get; set; }
        public int CustomerID { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
    }
}
