namespace ShoppingAppAPI.Exceptions
{
    public class CannotLoginException : Exception
    {
        string message;

        public CannotLoginException(string msg)
        {
            message = msg;
        }

        public override string Message => message;
    }
}
