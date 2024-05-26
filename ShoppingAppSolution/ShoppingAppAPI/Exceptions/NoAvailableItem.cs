namespace ShoppingAppAPI.Exceptions
{
    public class NoAvailableItem : Exception
    {
        string message;
        public NoAvailableItem(string Name)
        {
            message = $"No {Name} available!";
        }
        public override string Message => message;
    }
}
