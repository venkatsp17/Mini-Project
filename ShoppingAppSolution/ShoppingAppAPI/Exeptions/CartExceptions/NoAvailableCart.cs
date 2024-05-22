namespace ShoppingAppAPI.Exeptions.CartExceptions
{
    public class NoAvailableCart : Exception
    {
        string message;
        public NoAvailableCart() {
            message = "No carts available!";
        }
        public override string Message => message;
    }
}
