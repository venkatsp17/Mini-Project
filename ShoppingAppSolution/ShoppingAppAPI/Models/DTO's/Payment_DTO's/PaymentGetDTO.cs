using static ShoppingAppAPI.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace ShoppingAppAPI.Models.DTO_s.Payment_DTO_s
{
    public class PaymentGetDTO
    {
        public int OrderID { get; set; }
        public string Payment_Method { get; set; }
        public decimal Amount { get; set; }

    }
}
