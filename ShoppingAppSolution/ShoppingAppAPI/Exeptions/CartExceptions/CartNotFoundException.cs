namespace ShoppingAppAPI.Exeptions.CartExceptions
{
    public class CartNotFoundException : Exception
    {
        string message;
        public CartNotFoundException() {
            message = "Cart with given ID Not Found!";
        }
        public override string Message => message;
    }
}
