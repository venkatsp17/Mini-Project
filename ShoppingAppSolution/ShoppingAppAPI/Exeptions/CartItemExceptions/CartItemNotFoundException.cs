using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exeptions.CartItemExceptions
{
    public class CartItemNotFoundException : Exception
    {
        string message;
        public CartItemNotFoundException()
        {
            message = "Cart Item with given ID Not Found!";
        }
        public override string Message => message;
    }
}