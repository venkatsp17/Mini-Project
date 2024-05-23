using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exeptions.CartItemExceptions
{
    [Serializable]
    public class NoAvailableCartItem : Exception
    {
        string message;
        public NoAvailableCartItem()
        {
            message = "No carts items available!";
        }
        public override string Message => message;
    }
}