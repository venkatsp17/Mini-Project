using System.Runtime.Serialization;

namespace ShoppingAppAPI.Exeptions.CategoryExceptions
{
    public class NoAvailableCategory : Exception
    {
        string message;
        public NoAvailableCategory()
        {
            message = "No Categories Found!";
        }

        public override string Message => message;
    }
}